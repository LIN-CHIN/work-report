﻿using Microsoft.IO;
using Newtonsoft.Json;
using Serilog.Context;
using System.Net;
using System.Text;
using WorkReportAPI.AppLogs;

namespace WorkReportAPI.Middlewares
{
    /// <summary>
    /// 紀錄Log的Middleware
    /// </summary>
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LoggingMiddleware> _logger;
        private readonly RecyclableMemoryStreamManager _recyclableMemoryStreamManager;

        public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger) 
        {
            _next = next;
            _logger = logger;
            _recyclableMemoryStreamManager = new RecyclableMemoryStreamManager();
        }

        public async Task InvokeAsync(HttpContext context) 
        {
            LogModel request = new LogModel() { LogMessageType = LogMessageTypeEnum.Request};
            LogModel response = new LogModel() { LogMessageType = LogMessageTypeEnum.Response };

            string eventId = context.Request.Headers["EventId"].ToString();
            if (string.IsNullOrWhiteSpace(eventId)) 
            {
                throw new Exception("Header一定要有EventId");
            }
            
            request.EventId = eventId;
            request.Body = await GetRequestBody(context.Request);
            _logger.LogInformation(JsonConvert.SerializeObject(request));

            response.EventId = eventId;
            response.Body = await GetResponseBody(context);
            _logger.LogInformation(JsonConvert.SerializeObject(response));

        }

        /// <summary>
        /// 取得Request body 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private async Task<object?> GetRequestBody(HttpRequest request) 
        {
            request.EnableBuffering();

            using (StreamReader reader = new StreamReader(
                request.Body,
                Encoding.UTF8,
                detectEncodingFromByteOrderMarks: false,
                leaveOpen: true)) 
            {
                string body = await reader.ReadToEndAsync();
                request.Body.Position = 0;
                return JsonConvert.DeserializeObject(body);
            }
        }

        /// <summary>
        /// 取得Response body
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private async Task<object?> GetResponseBody(HttpContext context) 
        {
            await using var responseBody = _recyclableMemoryStreamManager.GetStream();
            context.Response.Body = responseBody;

            await _next(context);

            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var body = await new StreamReader(responseBody).ReadToEndAsync();
            context.Response.Body.Seek(0, SeekOrigin.Begin);

            return JsonConvert.DeserializeObject(body);
        }
    }
}

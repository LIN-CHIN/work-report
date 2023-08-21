using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Serilog;
using Serilog.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkReportClient.AppLogs;

namespace WorkReportClient.Services.LogServices
{
    /// <summary>
    /// Log Service
    /// </summary>
    public class LogService : ILogService
    {
        private readonly ILogger<LogService> _logger;
        private readonly string _errorPrefix = "Error: ";
        private readonly string _infoPrefix = "Info: ";

        public LogService(ILogger<LogService> logger) 
        {
            _logger = logger;
        }

        ///<inheritdoc/>
        public void WriteInfoLog(string message)
        {
            Console.WriteLine($"{_infoPrefix}{message}");
            _logger.LogError(message);
        }

        ///<inheritdoc/>
        public void WriteErrorLog(string message)
        {
            Console.WriteLine($"{_errorPrefix}{message}");
            _logger.LogError(message);
        }

        ///<inheritdoc/>
        public void WriteBody(string eventId, LogMessageTypeEnum messageType, object? body)
        {
            LogModel logModel = new LogModel
            {
                EventId = eventId,
                LogMessageType = messageType,
                Body = body
            };

            _logger.LogInformation($"{_infoPrefix}{JsonConvert.SerializeObject(body)}");
        }
    }
}

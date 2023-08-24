using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RecordWorker.AppLogs;
using Serilog.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordWorker.Services.LogServices
{
    /// <summary>
    /// Log Service
    /// </summary>
    public class LogService : ILogService
    {
        private readonly ILogger<LogService> _logger;

        public LogService(ILogger<LogService> logger)
        {
            _logger = logger;
        }

        ///<inheritdoc/>
        public void WriteInfoLog(string message)
        {
            _logger.LogInformation(message);
        }

        ///<inheritdoc/>
        public void WriteInfoLog(string message, string eventId)
        {
            using (LogContext.PushProperty("EventID", eventId, true))
            {
                _logger.LogInformation(message);
            }
        }

        ///<inheritdoc/>
        public void WriteErrorLog(string message)
        {
            _logger.LogError(message);
        }

        ///<inheritdoc/>
        public void WriteErrorLog(string message, string eventId)
        {
            using (LogContext.PushProperty("EventID", eventId, true))
            {
                _logger.LogError(message);
            }
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

            using (LogContext.PushProperty("EventID", eventId, true))
            {
                _logger.LogInformation(JsonConvert.SerializeObject(logModel));
            }
        }
    }
}

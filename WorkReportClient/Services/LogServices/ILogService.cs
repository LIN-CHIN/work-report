using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkReportClient.AppLogs;

namespace WorkReportClient.Services.LogServices
{
    /// <summary>
    /// Log Service interface
    /// </summary>
    public interface ILogService
    {
        /// <summary>
        /// 紀錄info log
        /// </summary>
        /// <param name="message"></param>
        void WriteInfoLog(string message);
        
        /// <summary>
        /// 紀錄error log
        /// </summary>
        /// <param name="message"></param>
        void WriteErrorLog(string message);

        /// <summary>
        /// 將body轉成LogModel後 記錄log
        /// </summary>
        /// <param name="eventId">事件id</param>
        /// <param name="messageType">訊息類型</param>
        /// <param name="body"></param>
        void WriteBody(string eventId, LogMessageTypeEnum messageType, object? body);
    }
}

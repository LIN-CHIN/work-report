using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkReportClient.AppLogs
{
    /// <summary>
    /// 紀錄Log用的Model
    /// </summary>
    public class LogModel
    {
        /// <summary>
        /// 事件ID
        /// </summary>
        public string EventId { get; set; }

        /// <summary>
        /// 訊息類型
        /// </summary>
        public LogMessageTypeEnum LogMessageType { get; set; }

        /// <summary>
        /// Request/Response Body
        /// </summary>
        public object? Body { get; set; }
    }
}

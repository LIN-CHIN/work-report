namespace WorkReportAPI.AppLogs
{
    /// <summary>
    /// 紀錄Log用的Model
    /// </summary>
    public class LogModel
    {
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

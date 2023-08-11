namespace WorkReportAPI
{
    public class AppSettings
    {
        /// <summary>
        /// RabbitMQ 主機名稱
        /// </summary>
        public string RabbitMQHostName { get; private set; }

        /// <summary>
        /// RabbitMQ Port號
        /// </summary>
        public int RabbitMQPort { get; private set; }
    }
}

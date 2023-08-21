using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;
using WorkReportAPI.DTOs;
using WorkReportAPI.RabbitMQ;

namespace WorkReportAPI.Services
{
    /// <summary>
    /// 報工Service
    /// </summary>
    public class WorkReportService : IWorkReportService
    {
        private readonly IRabbitMQHelper _rabbitMQHelper;
        private readonly ILogger<WorkReportService> _logger;
        private readonly string _exchangeName = "workReport";

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="rabbitMQHelper"></param>
        public WorkReportService(IRabbitMQHelper rabbitMQHelper, ILogger<WorkReportService> logger) 
        {
            _rabbitMQHelper = rabbitMQHelper;
            _logger = logger;
        }

        ///<inheritdoc/>
        public void Report(ReportDTO reportModel)
        {
            Console.WriteLine("Preparing to put the information into the queue.");
            _logger.LogInformation("Preparing to put the information into the queue.");

            using var connection = _rabbitMQHelper.Connect();
            using var channel = _rabbitMQHelper.CreateModel(connection);

            //設定Exchange
            channel.ExchangeDeclare(
                exchange: _exchangeName,
                type: ExchangeType.Fanout);

            ReportModel result = ReportModel.ConvertReportModel(reportModel);

            var body = Encoding.UTF8.GetBytes( 
                JsonConvert.SerializeObject(result));

            channel.BasicPublish(
                exchange: _exchangeName,
                routingKey: string.Empty,
                basicProperties: null,
                body: body);

            Console.WriteLine("Placement completed.");
            _logger.LogInformation("Placement completed.");
        }
    }
}

using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;
using WorkReportAPI.DTOs;

namespace WorkReportAPI.Services
{
    /// <summary>
    /// 報工Service
    /// </summary>
    public class WorkReportService : IWorkReportService
    {
        ///<inheritdoc/>
        public void Report(ReportDTO reportModel)
        {
            Console.WriteLine("Preparing to put the information into the queue.");
            var factory = new ConnectionFactory { 
                HostName = "rabbitmq",
                Port = 5672 };

            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            const string EXCHANGE_NAME = "workReport";

            //設定Exchange
            channel.ExchangeDeclare(
                exchange: EXCHANGE_NAME,
                type: ExchangeType.Fanout);

            ReportModel result = ReportModel.ConvertReportModel(reportModel);

            var body = Encoding.UTF8.GetBytes( 
                JsonConvert.SerializeObject(result));

            channel.BasicPublish(
                exchange: EXCHANGE_NAME,
                routingKey: string.Empty,
                basicProperties: null,
                body: body);

            Console.WriteLine("Placement completed.");
        }
    }
}

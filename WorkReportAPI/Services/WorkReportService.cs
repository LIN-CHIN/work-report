using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace WorkReportAPI.Services
{
    /// <summary>
    /// 報工Service
    /// </summary>
    public class WorkReportService : IWorkReportService
    {
        ///<inheritdoc/>
        public void Report(ReportModel reportModel)
        {
            var factory = new ConnectionFactory { HostName = "localhost" };

            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare(
                queue: "workReport",
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            var body = Encoding.UTF8.GetBytes( 
                JsonConvert.SerializeObject(reportModel));

            channel.BasicPublish(
                exchange: string.Empty,
                routingKey: "workReport",
                basicProperties: null,
                body: body);

        }
    }
}

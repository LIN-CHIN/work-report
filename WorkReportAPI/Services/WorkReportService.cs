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
            var factory = new ConnectionFactory { HostName = "localhost" };

            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare(
                queue: "workReport",
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            ReportModel result = new ReportModel
            {
                EventId = Guid.NewGuid(),
                MachineNumber = reportModel.MachineNumber,
                SpendTimeHour = reportModel.SpendTimeHour,
                SpendTimeMinute = reportModel.SpendTimeMinute,
                SpendTimeSecond = reportModel.SpendTimeSecond
            };

            var body = Encoding.UTF8.GetBytes( 
                JsonConvert.SerializeObject(result));

            channel.BasicPublish(
                exchange: string.Empty,
                routingKey: "workReport",
                basicProperties: null,
                body: body);

        }
    }
}

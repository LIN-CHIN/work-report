using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RecordWorker.AppLogs;
using RecordWorker.Context;
using RecordWorker.Entities;
using RecordWorker.RabbitMQ;
using RecordWorker.Services.LogServices;
using RecordWorker.Services.WorkReports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace RecordWorker
{
    public class Application
    {
        private static ManualResetEvent _manualResetEvent = new ManualResetEvent(false);
        private readonly IWorkReportRecordService _workReportRecordService;
        private readonly IRabbitMQHelper _rabbitMQHelper;
        private readonly ILogService _logService;
        private readonly string _exchangeName = "workReport";
        private readonly string _queueName = "workReportA";

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="workReportRecordService"></param>
        /// <param name="rabbitMQHelper"></param>
        public Application(IWorkReportRecordService workReportRecordService,
            IRabbitMQHelper rabbitMQHelper,
            ILogService logService)
        {
            _workReportRecordService = workReportRecordService;
            _rabbitMQHelper = rabbitMQHelper;
            _logService = logService;
        }

        /// <summary>
        /// 程式起始點
        /// </summary>
        public void Run()
        {
            using var connection = _rabbitMQHelper.Connect();
            using var channel = _rabbitMQHelper.CreateModel(connection);

            SetQueue(channel);
            SetExchange(channel);
            BindQueue(channel);

            var consumer = new EventingBasicConsumer(channel);
          
            consumer.Received += (model, ea) =>
            {
                Console.WriteLine("The Record Worker has received the message");
                var body = ea.Body.ToArray();
                ReportModel? reportModel = JsonConvert.DeserializeObject<ReportModel>(
                    Encoding.UTF8.GetString(body));

                string eventId = null;
                if (reportModel != null) 
                {
                    eventId = reportModel.EventId.ToString();

                    _logService.WriteBody(eventId, LogMessageTypeEnum.Request, reportModel);
                    _workReportRecordService.Insert(new WorkReportRecord 
                    {
                        EventId = reportModel.EventId,
                        MachineNumber = reportModel.MachineNumber,
                        SpendTimeHour = reportModel.SpendTimeHour,
                        SpendTimeMinute = reportModel.SpendTimeMinute,
                        SpendTimeSecond = reportModel.SpendTimeSecond
                    });

                    _logService.WriteInfoLog("The addition of the record to the database has been completed.", eventId);
                }

                channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                _logService.WriteInfoLog("The 'received' event of the record worker has been completed.", eventId);
            };

            channel.BasicConsume(queue: _queueName,
                     autoAck: false,
                     consumer: consumer);

            _manualResetEvent.WaitOne();
        }

        /// <summary>
        /// 設定Queue
        /// </summary>
        /// <param name="channel"></param>
        private void SetQueue(IModel channel) 
        {
            channel.QueueDeclare(
                    queue: _queueName,
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null
            );
        }

        /// <summary>
        /// 設定Exchange
        /// </summary>
        /// <param name="channel"></param>
        private void SetExchange(IModel channel)
        {
            channel.ExchangeDeclare(
                exchange: _exchangeName,
                type: ExchangeType.Fanout
            );
        }

        /// <summary>
        /// 綁定Queue & Exchange
        /// </summary>
        /// <param name="channel"></param>
        private void BindQueue(IModel channel) 
        {
            channel.QueueBind(
                queue: _queueName,
                exchange: _exchangeName,
                routingKey: string.Empty
            );
        }
    }
}
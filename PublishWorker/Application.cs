using Newtonsoft.Json;
using PublishWorker.AppLogs;
using PublishWorker.DTOs.LineNotifyDTOs;
using PublishWorker.RabbitMQ;
using PublishWorker.Services.LineNotifyServices;
using PublishWorker.Services.LogServices;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PublishWorker
{
    public class Application
    {
        private static ManualResetEvent _manualResetEvent = new ManualResetEvent(false);
        private readonly IRabbitMQHelper _rabbitMQHelper;
        private readonly ILogService _logService;
        private readonly ILineNotifyService _lineNotifyService;
        private readonly string _calculatedResultQueueName = "calculatedResult";
        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="rabbitMQHelper"></param>
        /// <param name="logService"></param>
        /// <param name="lineNotifyService"></param>
        public Application(IRabbitMQHelper rabbitMQHelper,
            ILogService logService,
            ILineNotifyService lineNotifyService) 
        {
            _rabbitMQHelper = rabbitMQHelper;
            _logService = logService;
            _lineNotifyService = lineNotifyService;
        }

        /// <summary>
        /// 程式進入點
        /// </summary>
        public void Run()
        {
            using var connection = _rabbitMQHelper.Connect();
            using var channel = _rabbitMQHelper.CreateModel(connection);

            SetQueue(channel);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                CalculationResult? calculationResult = JsonConvert.DeserializeObject<CalculationResult>(
                    Encoding.UTF8.GetString(body));

                string eventId = null;
                if (calculationResult != null) 
                {
                    eventId = calculationResult.EventId.ToString();
                    _logService.WriteBody(eventId,
                        LogMessageTypeEnum.Request,
                        calculationResult);

                    string notification = "\n" +
                                          $"MachineNumber : {calculationResult.MachineNumber}" + "\n" +
                                          $"TotalHour : {calculationResult.TotalHour}" + "\n" +
                                          $"TotalMinute : {calculationResult.TotalMinute}" + "\n" +
                                          $"TotalSecond : {calculationResult.TotalSecond}" + "\n" +
                                          $"TotalCount : {calculationResult.TotalCount}" + "\n" ; 
                    
                    Console.WriteLine(notification);

                    _lineNotifyService.Publish(new PublishNotifyDTO
                    {
                        Message = notification
                    });
                    _logService.WriteInfoLog("The message has already been sent.", eventId);
                }

                channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                _logService.WriteInfoLog("The 'received' event of the publish worker has been completed.", eventId);
            };

            channel.BasicConsume(queue: _calculatedResultQueueName,
                     autoAck: false,
                     consumer: consumer);

            _manualResetEvent.WaitOne();
        }

        /// <summary>
        /// 設定Queue
        /// </summary>
        /// <param name="channel"></param>
        public void SetQueue(IModel channel) 
        {
            channel.QueueDeclare(
                    queue: _calculatedResultQueueName,
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);
        }
    }
}

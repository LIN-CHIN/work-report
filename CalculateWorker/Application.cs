using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using CalculateWorker.RabbitMQ;
using CalculateWorker.Services.CacheServices;
using CalculateWorker.AppLogs;
using CalculateWorker.Services.LogServices;

namespace CalculateWorker
{
    public class Application
    {
        private static ManualResetEvent _manualResetEvent = new ManualResetEvent(false);
        private readonly ICacheService _cacheService;
        private readonly IRabbitMQHelper _rabbitMQHelper;
        private readonly ILogService _logService;
        private readonly string _exchangeName = "workReport";
        private readonly string _workReportQueueName = "workReportB";
        private readonly string _calculatedResultQueueName = "calculatedResult";

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="cacheService"></param>
        /// <param name="rabbitMQHelper"></param>
        public Application(ICacheService cacheService,
            IRabbitMQHelper rabbitMQHelper,
            ILogService logService) 
        {
            _cacheService = cacheService;
            _rabbitMQHelper = rabbitMQHelper;
            _logService = logService;
        }

        /// <summary>
        /// 程式進入點
        /// </summary>
        public void Run() 
        {
            using var connection = _rabbitMQHelper.Connect();
            using var channel = _rabbitMQHelper.CreateModel(connection);

            SetQueue(channel);
            SetExchange(channel);
            BindExchange(channel);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                Console.WriteLine("The calculate worker has received the message");
                var body = ea.Body.ToArray();
                ReportModel? reportModel = JsonConvert.DeserializeObject<ReportModel>(
                    Encoding.UTF8.GetString(body));

                string eventId = null;
                if (reportModel != null)
                {
                    eventId = reportModel.EventId.ToString();
                    _logService.WriteBody(eventId, LogMessageTypeEnum.Request, reportModel);

                    //取得當地的時區資訊
                    TimeZoneInfo localTimeZone = TimeZoneInfo.Local;

                    //取得今天的日期
                    DateTimeOffset today = DateTimeOffset.Now.Date;
                    DateTimeOffset tonightMidnight = today.AddHours(24).AddTicks(-1);

                    //將時間轉換為當地時區
                    DateTimeOffset expiratimeTime = TimeZoneInfo.ConvertTime(tonightMidnight, localTimeZone);

                    string hourKey = $"{reportModel.MachineNumber}_Hour";
                    string minuteKey = $"{reportModel.MachineNumber}_Minute";
                    string secondKey = $"{reportModel.MachineNumber}_Second";
                    string countKey = $"{reportModel.MachineNumber}_Count";

                    CalculationResult calculationResult = new CalculationResult();
                    calculationResult.EventId = reportModel.EventId;
                    calculationResult.MachineNumber = reportModel.MachineNumber;

                    //計算時間
                    calculationResult.TotalHour = CalculateTime(hourKey, reportModel.SpendTimeHour, expiratimeTime);
                    calculationResult.TotalMinute = CalculateTime(minuteKey, reportModel.SpendTimeMinute, expiratimeTime);
                    calculationResult.TotalSecond = CalculateTime(secondKey, reportModel.SpendTimeSecond, expiratimeTime);

                    //總數計算
                    calculationResult.TotalCount = _cacheService.IncreaseKey(countKey);

                    //發送訊息
                    channel.BasicPublish(
                        exchange: string.Empty,
                        routingKey: _calculatedResultQueueName,
                        basicProperties: null,
                        body: Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(calculationResult)) );

                    _logService.WriteInfoLog("The computation has been completed and the message has been pushed to the queue.",
                       eventId);
                }

                channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                _logService.WriteInfoLog("The 'received' event of the calculate worker has been completed.", eventId);
            };

            channel.BasicConsume(queue: _workReportQueueName,
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
                queue: _workReportQueueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            channel.QueueDeclare(
               queue: _calculatedResultQueueName,
               durable: true,
               exclusive: false,
               autoDelete: false,
               arguments: null);
        }

        /// <summary>
        /// 設定Exchange
        /// </summary>
        /// <param name="channel"></param>
        private void SetExchange(IModel channel)
        {
            channel.ExchangeDeclare(
                exchange: _exchangeName,
                type: ExchangeType.Fanout);
        }

        /// <summary>
        /// 將綁定Queue 綁定 Exchange
        /// </summary>
        /// <param name="channel"></param>
        private void BindExchange(IModel channel)
        {
            //綁定Queue & Exchange
            channel.QueueBind(
                queue: _workReportQueueName,
                exchange: _exchangeName,
                routingKey: string.Empty
            );
        }

        /// <summary>
        /// 計算時間
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expiratimeTime"></param>
        private int CalculateTime(string key, int value, DateTimeOffset expiratimeTime) 
        {
            int result = 0;

            //小時計算
            if (_cacheService.IsKeyExist(key))
            {
                result = _cacheService.GetData<int>(key) + value;
            }
            else
            {
                result = value;
            }

            _cacheService.SetData<int>(key, result, expiratimeTime);
            return result;
        }
    }
}

using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using CalculateWorker.CacheServices;

namespace CalculateWorker
{
    public class Application
    {
        private static ManualResetEvent _manualResetEvent = new ManualResetEvent(false);
        private readonly ICacheService _cacheService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="cacheService"></param>
        public Application(ICacheService cacheService) 
        {
            _cacheService = cacheService;
        }

        /// <summary>
        /// 程式進入點
        /// </summary>
        public void Run() 
        {
            var factory = new ConnectionFactory
            {
                HostName = "rabbitmq",
                Port = 5672
            };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            const string EXCHANGE_NAME = "workReport";
            const string WORK_REPORT_QUEUE_NAME = "workReportB";
            const string CALCULATED_RESULT_QUEUE_NAME = "calculatedResult";

            channel.QueueDeclare(
                queue: WORK_REPORT_QUEUE_NAME,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            channel.QueueDeclare(
               queue: CALCULATED_RESULT_QUEUE_NAME,
               durable: true,
               exclusive: false,
               autoDelete: false,
               arguments: null);

            //設定Exchange
            channel.ExchangeDeclare(
                exchange: EXCHANGE_NAME,
                type: ExchangeType.Fanout);

            //綁定Queue & Exchange
            channel.QueueBind(
                queue: WORK_REPORT_QUEUE_NAME,
                exchange: EXCHANGE_NAME,
                routingKey: string.Empty
                );

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                Console.WriteLine("The calculate worker has received the message");
                var body = ea.Body.ToArray();
                ReportModel? reportModel = JsonConvert.DeserializeObject<ReportModel>(
                    Encoding.UTF8.GetString(body));
                
                if (reportModel != null)
                {
                    //取得當地的時區資訊
                    TimeZoneInfo localTimeZone = TimeZoneInfo.Local;

                    //取得今天的日期
                    DateTimeOffset today = DateTimeOffset.Now.Date;
                    
                    DateTimeOffset tonightMidnight = today.AddHours(24).AddTicks(-1);

                    // 將時間轉換為當地時區
                    DateTimeOffset localTonightMidnight = TimeZoneInfo.ConvertTime(tonightMidnight, localTimeZone);
                    Console.WriteLine("今天的晚上 12:00 分在當地時區的時間為：" + localTonightMidnight);

                    string hourKey = $"{reportModel.MachineNumber}_Hour";
                    string minuteKey = $"{reportModel.MachineNumber}_Minute";
                    string secondKey = $"{reportModel.MachineNumber}_Second";
                    string countKey = $"{reportModel.MachineNumber}_Count";

                    CalculationResult calculationResult = new CalculationResult();
                    calculationResult.MachineNumber = reportModel.MachineNumber;

                    //小時計算
                    if (_cacheService.IsKeyExist(hourKey))
                    {
                        calculationResult.TotalHour = _cacheService.GetData<int>(hourKey) + reportModel.SpendTimeHour; ; 
                        _cacheService.SetData<int>(hourKey, calculationResult.TotalHour, localTonightMidnight);
                    }
                    else 
                    {
                        calculationResult.TotalHour = reportModel.SpendTimeHour;
                        _cacheService.SetData<int>(hourKey, reportModel.SpendTimeHour, localTonightMidnight);
                    }

                    //分鐘計算
                    if (_cacheService.IsKeyExist(minuteKey))
                    {
                        calculationResult.TotalMinute = _cacheService.GetData<int>(minuteKey) + reportModel.SpendTimeMinute;
                        _cacheService.SetData<int>(minuteKey, calculationResult.TotalMinute, localTonightMidnight);
                    }
                    else
                    {
                        calculationResult.TotalMinute = reportModel.SpendTimeMinute;
                        _cacheService.SetData<int>(minuteKey, reportModel.SpendTimeMinute, localTonightMidnight);
                    }

                    //秒數計算
                    if (_cacheService.IsKeyExist(secondKey))
                    {
                        calculationResult.TotalSecond = _cacheService.GetData<int>(secondKey) + reportModel.SpendTimeSecond;
                        _cacheService.SetData<int>(secondKey, calculationResult.TotalSecond, localTonightMidnight);
                    }
                    else
                    {
                        calculationResult.TotalSecond = reportModel.SpendTimeSecond;
                        _cacheService.SetData<int>(secondKey, reportModel.SpendTimeSecond, localTonightMidnight);
                    }

                    //總數計算
                    calculationResult.TotalCount = _cacheService.IncreaseKey(countKey);

                    //發送訊息
                    channel.BasicPublish(
                        exchange: string.Empty,
                        routingKey: CALCULATED_RESULT_QUEUE_NAME,
                        basicProperties: null,
                        body: Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(calculationResult)) );

                    Console.WriteLine("The calculate worker has published the message");
                }

                channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                Console.WriteLine("The calculate worker has been completed.");
            };

            channel.BasicConsume(queue: WORK_REPORT_QUEUE_NAME,
                     autoAck: false,
                     consumer: consumer);
             _manualResetEvent.WaitOne();
        }
    }
}

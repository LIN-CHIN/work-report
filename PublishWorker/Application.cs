using Newtonsoft.Json;
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

        public Application() 
        {
        
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

            const string CALCULATED_RESULT_QUEUE_NAME = "calculatedResult";

            channel.QueueDeclare(
                queue: CALCULATED_RESULT_QUEUE_NAME,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                CalculationResult? calculationResult = JsonConvert.DeserializeObject<CalculationResult>(
                    Encoding.UTF8.GetString(body));

                if (calculationResult != null) 
                {
                    Console.WriteLine($"MachineNumber : {calculationResult.MachineNumber}");
                    Console.WriteLine($"TotalHour : {calculationResult.TotalHour}");
                    Console.WriteLine($"TotalMinute : {calculationResult.TotalMinute}");
                    Console.WriteLine($"TotalSecond : {calculationResult.TotalSecond}");
                    Console.WriteLine($"TotalCount : {calculationResult.TotalCount}");
                    Console.WriteLine($"--------------------------------------------");
                }

                channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
            };

            channel.BasicConsume(queue: CALCULATED_RESULT_QUEUE_NAME,
                     autoAck: false,
                     consumer: consumer);

            _manualResetEvent.WaitOne();
        }
    }
}

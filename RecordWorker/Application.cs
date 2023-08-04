﻿using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RecordWorker.Context;
using RecordWorker.Entities;
using RecordWorker.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordWorker
{
    public class Application
    {
        private readonly IWorkReportRecordService _workReportRecordService;
        private static ManualResetEvent _manualResetEvent = new ManualResetEvent(false);

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="workReportRecordService"></param>
        public Application(IWorkReportRecordService workReportRecordService)
        {
           _workReportRecordService = workReportRecordService;
        }

        /// <summary>
        /// 程式起始點
        /// </summary>
        public void Run()
        {
            var factory = new ConnectionFactory {
                HostName = "rabbitmq",
                Port = 5672
            };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare(
                queue: "workReport",
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                Console.WriteLine("The Record Worker has received the message");
                var body = ea.Body.ToArray();
                ReportModel? reportModel = JsonConvert.DeserializeObject<ReportModel>(
                    Encoding.UTF8.GetString(body));

                if (reportModel != null) 
                {
                    _workReportRecordService.Insert(new WorkReportRecord 
                    {
                        EventId = reportModel.EventId,
                        MachineNumber = reportModel.MachineNumber,
                        SpendTimeHour = reportModel.SpendTimeHour,
                        SpendTimeMinute = reportModel.SpendTimeMinute,
                        SpendTimeSecond = reportModel.SpendTimeSecond
                    });
                }

                channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                Console.WriteLine("The record has been completed.");
            };

            channel.BasicConsume(queue: "workReport",
                     autoAck: false,
                     consumer: consumer);

            while (true) { };
        }
    }
}
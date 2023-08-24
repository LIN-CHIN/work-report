using RabbitMQ.Client;
using RecordWorker.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordWorker.RabbitMQ
{
    /// <summary>
    /// 實現RabbitMQ功能的Helper 
    /// </summary>
    public class RabbitMQHelper : IRabbitMQHelper
    {
        private readonly ConnectionFactory _factory;
        private readonly AppSettings _appSettings;
        public RabbitMQHelper(AppSettings appSettings) 
        {
            _appSettings = appSettings;
            _factory = new ConnectionFactory
            {
                HostName = _appSettings.RabbitMQHostName,
                Port = _appSettings.RabbitMQPort
            };
        }

        ///<inheritdoc/>
        public IConnection Connect()
        {
            return _factory.CreateConnection();
        }

        ///<inheritdoc/>
        public IModel CreateModel(IConnection connection)
        {
            return connection.CreateModel();
        }
    }
}

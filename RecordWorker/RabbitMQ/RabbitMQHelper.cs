using RabbitMQ.Client;
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
        public RabbitMQHelper() 
        {
            _factory = new ConnectionFactory
            {
                HostName = "rabbitmq",
                Port = 5672
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

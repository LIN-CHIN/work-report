using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculateWorker.Settings
{
    /// <summary>
    /// App設定
    /// </summary>
    public class AppSettings
    {
        /// <summary>
        /// RabbitMQ 主機名稱
        /// </summary>
        public string RabbitMQHostName { get; private set; }

        /// <summary>
        /// RabbitMQ Port號
        /// </summary>
        public int RabbitMQPort { get; private set; }

        /// <summary>
        /// Redis 主機名稱
        /// </summary>
        public string RedisHostName { get; private set; }

        /// <summary>
        /// Redis Port號
        /// </summary>
        public int RedisPort { get; private set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordWorker
{
    /// <summary>
    /// App設定
    /// </summary>
    public class AppSettings
    {
        /// <summary>
        /// 連線字串
        /// </summary>
        public string ConnectionString { get; private set; }

        /// <summary>
        /// RabbitMQ 主機名稱
        /// </summary>
        public string RabbitMQHostName { get; private set; }

        /// <summary>
        /// RabbitMQ Port號
        /// </summary>
        public int RabbitMQPort { get; private set; }
    }
}

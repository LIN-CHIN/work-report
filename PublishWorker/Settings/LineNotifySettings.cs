using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PublishWorker.Settings
{
    /// <summary>
    /// Line Notify 設定
    /// </summary>
    public class LineNotifySettings
    {
        /// <summary>
        /// Line Notify 綁定的Bearer Token
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// Line Notify Host
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// Line Notify Api Prefix
        /// </summary>
        public string Prefix { get; set; }

        /// <summary>
        /// Line Notify 發布訊息的路由
        /// </summary>
        public string PublishRoute { get; set; }

        /// <summary>
        /// 取得發布訊息的Url
        /// </summary>
        /// <returns></returns>
        public string GetPublishUrl() 
        {
            return $"{Host}{Prefix}{PublishRoute}";
        }



        
    }
}

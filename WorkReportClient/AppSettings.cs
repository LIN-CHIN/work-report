using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkReportClient
{
    public class AppSettings
    {
        /// <summary>
        /// 報工API Host
        /// </summary>
        public string WorkReportAPIHost { get; private set; }

        /// <summary>
        /// 報工路由
        /// </summary>
        public string WorkReportRoute { get; private set; }

        /// <summary>
        /// 報工URL
        /// </summary>
        public string WorkReportUrl { 
            get 
            {
                return $"{WorkReportAPIHost}{WorkReportRoute}";
            }
        }
    }
}

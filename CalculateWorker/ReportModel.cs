using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculateWorker
{
    /// <summary>
    /// 報工用的Model
    /// </summary>
    public class ReportModel
    {
        /// <summary>
        /// 事件id
        /// </summary>
        [Required]
        public Guid EventId { get; set; }

        /// <summary>
        /// 設備代碼
        /// </summary>
        [Required]
        public string MachineNumber { get; set; }

        /// <summary>
        /// 花費時間(小時)
        /// </summary>
        [Required]
        public int SpendTimeHour { get; set; }

        /// <summary>
        /// 花費時間(分鐘)
        /// </summary>
        [Required]
        public int SpendTimeMinute { get; set; }

        /// <summary>
        /// 花費時間(秒)
        /// </summary>
        [Required]
        public int SpendTimeSecond { get; set; }
    }
}

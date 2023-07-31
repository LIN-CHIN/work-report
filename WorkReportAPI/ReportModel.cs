using System.ComponentModel.DataAnnotations;

namespace WorkReportAPI
{
    /// <summary>
    /// 報工用的Model
    /// </summary>
    public class ReportModel
    {
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

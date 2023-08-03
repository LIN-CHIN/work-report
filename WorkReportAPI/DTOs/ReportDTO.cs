using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace WorkReportAPI.DTOs
{
    /// <summary>
    /// 報工DTO
    /// </summary>
    public class ReportDTO
    {
        /// <summary>
        /// 設備代碼
        /// </summary>
        [JsonRequired]
        public string MachineNumber { get; set; }

        /// <summary>
        /// 花費時間(小時)
        /// </summary>
        [JsonRequired]
        public int SpendTimeHour { get; set; }

        /// <summary>
        /// 花費時間(分鐘)
        /// </summary>
        [JsonRequired]
        public int SpendTimeMinute { get; set; }

        /// <summary>
        /// 花費時間(秒)
        /// </summary>
        [JsonRequired]
        public int SpendTimeSecond { get; set; }
    }
}

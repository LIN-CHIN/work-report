using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using WorkReportAPI.DTOs;

namespace WorkReportAPI
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

        /// <summary>
        /// 轉換成ReportModel
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public static ReportModel ConvertReportModel(ReportDTO dto) 
        {
            return new ReportModel
            {
                EventId = dto.EventId,
                MachineNumber = dto.MachineNumber,
                SpendTimeHour = dto.SpendTimeHour,
                SpendTimeMinute = dto.SpendTimeMinute,
                SpendTimeSecond = dto.SpendTimeSecond
            };
        }
    }
}

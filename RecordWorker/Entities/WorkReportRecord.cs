using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordWorker.Entities
{
    [Table("work_report_record")]
    public class WorkReportRecord
    {
        /// <summary>
        /// 流水號
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        [Column("id")]
        public long Id { get; set; }

        /// <summary>
        /// 事件id
        /// </summary>
        [Required]
        [Column("event_id", TypeName = "text")]
        public string EventId { get; set; }

        /// <summary>
        /// 設備代碼
        /// </summary>
        [Required]
        [Column("machine_number", TypeName = "varchar(50)")]
        public string MachineNumber { get; set; }

        /// <summary>
        /// 花費時間(小時)
        /// </summary>
        [Required]
        [Column("spend_time_hour")]
        public int SpendTimeHour { get; set; }

        /// <summary>
        /// 花費時間(分鐘)
        /// </summary>
        [Required]
        [Column("spend_time_minute")]
        public int SpendTimeMinute { get; set; }

        /// <summary>
        /// 花費時間(秒)
        /// </summary>
        [Required]
        [Column("spend_time_second")]
        public int SpendTimeSecond { get; set; }
    }
}

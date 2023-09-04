using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PublishWorker.DTOs.LineNotifyDTOs
{
    /// <summary>
    /// 發送通知的Request DTO
    /// </summary>
    public class PublishNotifyDTO
    {
        /// <summary>
        /// 訊息
        /// </summary>
        [MaxLength(1000)]
        [Required]
        public string Message { get; set; }
    }
}

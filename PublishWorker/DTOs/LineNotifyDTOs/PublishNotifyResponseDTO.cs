using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PublishWorker.DTOs.LineNotifyDTOs
{
    /// <summary>
    /// 發送通知的Response DTO
    /// </summary>
    public class PublishNotifyResponseDTO
    {
        /// <summary>
        /// 狀態代碼
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 訊息
        /// </summary>
        public string Message { get; set; }
    }
}

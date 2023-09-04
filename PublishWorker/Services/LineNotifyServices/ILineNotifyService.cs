using PublishWorker.DTOs.LineNotifyDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PublishWorker.Services.LineNotifyServices
{
    /// <summary>
    /// Line Notify Service Interface
    /// </summary>
    public interface ILineNotifyService
    {
        /// <summary>
        /// 發布訊息
        /// </summary>
        /// <param name="publishNotifyDTO">發布通知用的DTO</param>
        /// <returns></returns>
        public PublishNotifyResponseDTO? Publish(PublishNotifyDTO publishNotifyDTO);
    }
}

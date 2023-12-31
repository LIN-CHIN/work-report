﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculateWorker
{
    /// <summary>
    /// 計算結果
    /// </summary>
    public class CalculationResult
    {
        /// <summary>
        /// 事件ID
        /// </summary>
        public Guid EventId { get; set; }

        /// <summary>
        /// 設備代碼
        /// </summary>
        [Required]
        public string MachineNumber { get; set; }

        /// <summary>
        /// 總小時數
        /// </summary>
        public int TotalHour { get; set; }

        /// <summary>
        /// 總分鐘數
        /// </summary>
        public int TotalMinute { get; set; }

        /// <summary>
        /// 總秒數
        /// </summary>
        public int TotalSecond { get; set; }

        /// <summary>
        /// 總產出數量
        /// </summary>
        public long TotalCount { get; set; }
    }
}

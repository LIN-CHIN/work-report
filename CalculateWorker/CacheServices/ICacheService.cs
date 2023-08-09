using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculateWorker.CacheServices
{
    /// <summary>
    /// Cache Service Interface
    /// </summary>
    public interface ICacheService
    {
        /// <summary>
        /// 根據key取得資料
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        T GetData<T>(string key);

        /// <summary>
        /// 設定Data
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expirationTime"></param>
        /// <returns></returns>
        bool SetData<T>(string key, T value, DateTimeOffset expirationTime);

        /// <summary>
        /// 刪除Data
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        object RemoveData(string key);

        /// <summary>
        /// 檢查key是否存在
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        bool IsKeyExist(string key);

        /// <summary>
        /// 遞增
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        long IncreaseKey(string key, long value = 1);
    }
}

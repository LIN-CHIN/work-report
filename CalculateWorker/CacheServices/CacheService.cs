using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculateWorker.CacheServices
{
    /// <summary>
    /// Cache Service
    /// </summary>
    public class CacheService : ICacheService
    {
        private IDatabase _db;
        private readonly AppSettings _appSettings;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="appSettings"></param>
        public CacheService(AppSettings appSettings)
        {
            _appSettings = appSettings;
            ConfigureRedis();
        }
        
        /// <summary>
        /// 設定Redis
        /// </summary>
        private void ConfigureRedis()
        {
            string redisHost = _appSettings.RedisHostName;
            int redisPort = _appSettings.RedisPort; 

            string connectionString = $"{redisHost}:{redisPort},allowAdmin=true,abortConnect=false";
            ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(connectionString);
            _db = redis.GetDatabase(0);

        }

        ///<inheritdoc/>
        public T GetData<T>(string key)
        {
            var value = _db.StringGet(key);
            if (!string.IsNullOrEmpty(value))
            {
                return JsonConvert.DeserializeObject<T>(value);
            }
            return default;
        }

        ///<inheritdoc/>
        public bool SetData<T>(string key, T value, DateTimeOffset expirationTime)
        {
            TimeSpan expiryTime = expirationTime.DateTime.Subtract(DateTime.Now);
            var isSet = _db.StringSet(key, JsonConvert.SerializeObject(value), expiryTime);
            return isSet;
        }

        ///<inheritdoc/>
        public object RemoveData(string key)
        {
            bool _isKeyExist = _db.KeyExists(key);
            if (_isKeyExist == true)
            {
                return _db.KeyDelete(key);
            }
            return false;
        }

        ///<inheritdoc/>
        public bool IsKeyExist(string key) 
        {
            return _db.KeyExists(key);
        }

        ///<inheritdoc/>
        public long IncreaseKey(string key, long value = 1)
        {
           return _db.StringIncrement(key, value);
        }
    }
}

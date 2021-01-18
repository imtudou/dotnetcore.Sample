using StackExchange.Redis;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace RedisHelper.Sample
{
    /// <summary>
    /// RedisHelper
    /// </summary>
    public class RedisHelper
    {
        private static readonly int DbNum;
        private static readonly ConnectionMultiplexer Connection;
        private static readonly IDatabase Database;
        private static readonly string CustomKey;

        #region 构造
        static RedisHelper()
        {
            DbNum = RedisConnectionHelper.DbNum;
            Connection = RedisConnectionHelper.Instanse ?? RedisConnectionHelper.GetConnectionMultiplexer();
            Database = Connection.GetDatabase(DbNum);
            CustomKey = RedisConnectionHelper.SysCustomKey;
        }
        #endregion



        /// <summary>
        /// 保存单个Key Value
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expires"></param>
        /// <returns></returns>
        public bool StringSet(string key, string value,TimeSpan? expires = default)
        {
            return Database.StringSet(AddSysCustomKey(key), value, expires);
        }

        public bool StringSet(List<KeyValuePair<RedisKey, RedisValue>> keyValuePair)
        {
            //var newKeyValueList = keyValuePair.Select(s=>s.) 
            return false;
        
        }

        #region 辅助方法
        private string AddSysCustomKey(string oldKey)
        {
            var prefixKey = CustomKey ?? RedisConnectionHelper.SysCustomKey;
            return prefixKey + oldKey;
        }

        private string ConvertToJson<T>(T value)
        {
            return value is string ? value.ToString() : JsonSerializer.Serialize<T>(value);
        } 
        #endregion

    }
}

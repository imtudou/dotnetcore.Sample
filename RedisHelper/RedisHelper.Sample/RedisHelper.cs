using Newtonsoft.Json;

using StackExchange.Redis;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        /// <param name="key">Redis Key</param>
        /// <param name="value">保存的值</param>
        /// <param name="expires">过期时间</param>
        /// <returns></returns>
        public bool StringSet(string key, string value,TimeSpan? expires = default)
        {
            return Database.StringSet(AddSysCustomKey(key), value, expires);
        }

        /// <summary>
        /// 保存多个Key Value
        /// </summary>
        /// <param name="keyValuePair">键值对</param>
        /// <returns></returns>
        public bool StringSet(List<KeyValuePair<RedisKey, RedisValue>> keyValuePair)
        {
            var newKeyValueList = keyValuePair.Select(s => new KeyValuePair<RedisKey, RedisValue>(AddSysCustomKey(s.Key), s.Value)).ToList();
            return Database.StringSet(newKeyValueList.ToArray());
        }

        /// <summary>
        /// 保存单个对象
        /// </summary>
        /// <typeparam name="T">obj</typeparam>
        /// <param name="Key">Redis Key</param>
        /// <param name="value">保存的值</param>
        /// <param name="expires">过期时间</param>
        /// <returns></returns>
        public bool StringSet<T>(string Key,T value, TimeSpan? expires = default)
        {
            Key = AddSysCustomKey(Key);
            return Database.StringSet(Key, value.ToJson(), expires);
        }

        /// <summary>
        /// 获取单个对象
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        public string StringGet(string Key)
        {
            Key = AddSysCustomKey(Key);
            return Database.StringGet(Key);
        }

        public RedisValue[] StringGet(List<string> Key)
        { 
        
        }

        #region 辅助方法
        private string AddSysCustomKey(string oldKey)
        {
            var prefixKey = CustomKey ?? RedisConnectionHelper.SysCustomKey;
            return prefixKey + oldKey;
        }
        #endregion

    }

    public static class RedisHelperExtension
    {
        public static string ToJson<T>(this T t)
        {
            return t is string ? t.ToString() : JsonConvert.SerializeObject(t);
        }

        public string AddSysCustomKey( this string oldKey)
        {
            var prefixKey = CustomKey ?? RedisConnectionHelper.SysCustomKey;
            return prefixKey + oldKey;
        }

    }
}

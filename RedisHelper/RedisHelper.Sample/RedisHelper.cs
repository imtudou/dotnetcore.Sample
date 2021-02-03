using Newtonsoft.Json;

using StackExchange.Redis;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedisHelper.Sample
{

    #region 扩展方法
    public static class RedisHelperExtension
    {
        public static string ToJson<T>(this T t)
        {
            if (t is null)
            {
                return string.Empty;
            }
            return t is string ? t.ToString() : JsonConvert.SerializeObject(t);
        }

        public static string ToXml<T>(this T t) where T : class, new()
        {
            var sb = new StringBuilder();
            var type = t.GetType();
            sb.Append("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
            sb.Append("<" + nameof(t) + ">");

            foreach (var info in type.GetProperties())
            {
                sb.Append("<" + info?.Name + ">");
                sb.Append(type.GetProperty(info?.Name).GetValue(t));
                sb.Append("</" + info?.Name + ">");
            }

            sb.Append("</" + nameof(t) + ">");
            return sb.ToString();
        }


    }
    #endregion


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



        #region String
        #region 同步方法
        /// <summary>
        /// 保存单个Key Value
        /// </summary>
        /// <param name="key">Redis Key</param>
        /// <param name="value">保存的值</param>
        /// <param name="expires">过期时间</param>
        /// <returns></returns>
        public bool StringSet(string key, string value, TimeSpan? expires = default)
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
        public bool StringSet<T>(string Key, T value, TimeSpan? expires = default)
        {
            Key = AddSysCustomKey(Key);
            return Database.StringSet(Key, value.ToJson(), expires);
        }

        /// <summary>
        /// 获取单个对象
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        public string StringGet(string key)
        {
            key = AddSysCustomKey(key);
            return Database.StringGet(key);
        }

        /// <summary>
        /// 获取单个对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T StringGet<T>(string key)
        {
            key = AddSysCustomKey(key);
            return ConvertObject<T>(Database.StringGet(key));
        }

        /// <summary>
        /// 获取多个Key的值
        /// </summary>
        /// <param name="ListKey"></param>
        /// <returns></returns>
        public RedisValue[] StringGet(List<string> ListKey)
        {
            var newKeys = ListKey.Select(AddSysCustomKey).ToList();
            return Database.StringGet(ConvertRedisKeys(newKeys));
        }


        /// <summary>
        /// 计数器：自增
        /// <code>
        ///     db.StringSet("age", "11");
        ///     db.StringIncrement("age");
        ///     RedisValue age = db.StringGet("age");//结果：12
        ///     age = db.StringIncrement("age", 5);//结果：17
        /// </code>
        /// <see cref="StringIncrement"/>
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public double StringIncrement(string key, double val = 1)
        {
            return Database.StringIncrement(AddSysCustomKey(key), val);
        }

        /// <summary>
        /// 计数器：自减
        /// <code>
        ///     db.StringSet("age", "17");
        ///     age = db.StringDecrement("age");结果：16
        ///     age = db.StringDecrement("age", 5);结果：11
        /// </code>
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val">可以为负</param>
        /// <returns></returns>
        public double StringDecrement(string key, double val = 1)
        {
            return Database.StringDecrement(AddSysCustomKey(key), val);
        }
        #endregion

        #region 异步方法
        /// <summary>
        /// 保存单个Key Vakue
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expires"></param>
        /// <returns></returns>
        public async Task<bool> StringSetAsync(string key, string value, TimeSpan? expires = default)
        {
            return await Database.StringSetAsync(AddSysCustomKey(key), value, expires);
        }

        /// <summary>
        /// 保存多个Key Value
        /// </summary>
        /// <param name="keyValuePairs">键值对</param>
        /// <returns></returns>
        public async Task<bool> StringSetAsync(List<KeyValuePair<RedisKey, RedisValue>> keyValuePairs)
        {
            var newValues = keyValuePairs.Select(s => new KeyValuePair<RedisKey, RedisValue>(AddSysCustomKey(s.Key), s.Value)).ToArray();
            return await Database.StringSetAsync(newValues);
        }


        /// <summary>
        /// 保存单个对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        /// <param name="expries"></param>
        /// <returns></returns>
        public async Task<bool> StringSetAsync<T>(string key, T obj, TimeSpan? expries = default)
        {
            return await Database.StringSetAsync(AddSysCustomKey(key), obj.ToJson(), expries);
        }


        /// <summary>
        /// 获取单个key的值
        /// </summary>
        /// <param name="key">Redis Key</param>
        /// <returns></returns>
        public async Task<string> StringGetAsync(string key)
        {
            return await Database.StringGetAsync(AddSysCustomKey(key));
        }

        /// <summary>
        /// 获取多个Key
        /// </summary>
        /// <param name="listKey">Redis Key集合</param>
        /// <returns></returns>
        public async Task<RedisValue[]> StringGetAsync(List<string> listKey)
        {
            List<string> newKeys = listKey.Select(AddSysCustomKey).ToList();
            return await Database.StringGetAsync(ConvertRedisKeys(newKeys));
        }

        /// <summary>
        /// 获取一个key的对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<T> StringGetAsync<T>(string key)
        {
            key = AddSysCustomKey(key);
            string result = await Database.StringGetAsync(key);
            return ConvertObject<T>(result);
        }

        /// <summary>
        /// 为数字增长val
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val">可以为负</param>
        /// <returns>增长后的值</returns>
        public async Task<double> StringIncrementAsync(string key, double val = 1)
        {  
            return await Database.StringIncrementAsync(AddSysCustomKey(key), val);
        }

        /// <summary>
        /// 为数字减少val
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val">可以为负</param>
        /// <returns>减少后的值</returns>
        public async Task<double> StringDecrementAsync(string key, double val = 1)
        {
            return await Database.StringDecrementAsync(AddSysCustomKey(key), val);
        }

        #endregion
        #endregion


        #region Hash

        #region 同步方法

        /// <summary>
        /// 判断某个数据是否已经被缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <returns></returns>
        public bool HashExists(string key, string dataKey)
        {
            return Database.HashExists(AddSysCustomKey(key), dataKey);
        }

        /// <summary>
        /// 存储数据到hash表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool HashSet<T>(string key, string dataKey, T t)
        {
            return Database.HashSet(AddSysCustomKey(key), dataKey, t.ToJson());
        }

        /// <summary>
        /// 移除hash中的某值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <returns></returns>
        public bool HashDelete(string key, string dataKey)
        {
            return Database.HashDelete(AddSysCustomKey(key), dataKey);
        }

        /// <summary>
        /// 移除hash中的多个值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dataKeys"></param>
        /// <returns></returns>
        public long HashDelete(string key, List<RedisValue> dataKeys)
        {
            return Database.HashDelete(AddSysCustomKey(key), dataKeys.ToArray());
        }

        /// <summary>
        /// 从hash表获取数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <returns></returns>
        public T HashGet<T>(string key, string dataKey)
        {
                string result = Database.HashGet(AddSysCustomKey(key), dataKey);
                return ConvertObject<T>(result);
             
        }

        /// <summary>
        /// 为数字增长val
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <param name="val">可以为负</param>
        /// <returns>增长后的值</returns>
        public double HashIncrement(string key, string dataKey, double val = 1)
        {
            return Database.HashIncrement(AddSysCustomKey(key), dataKey, val);
        }

        /// <summary>
        /// 为数字减少val
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <param name="val">可以为负</param>
        /// <returns>减少后的值</returns>
        public double HashDecrement(string key, string dataKey, double val = 1)
        {
            return Database.HashDecrement(AddSysCustomKey(key), dataKey, val);
        }

        /// <summary>
        /// 获取hashkey所有Redis key
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public List<T> HashKeys<T>(string key)
        {
                RedisValue[] values = Database.HashKeys(AddSysCustomKey(key));
                return ConvertList<T>(values);
        }

        #endregion 同步方法

        #region 异步方法

        /// <summary>
        /// 判断某个数据是否已经被缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <returns></returns>
        public async Task<bool> HashExistsAsync(string key, string dataKey)
        {
            return await Database.HashExistsAsync(AddSysCustomKey(key), dataKey);
        }

        /// <summary>
        /// 存储数据到hash表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public async Task<bool> HashSetAsync<T>(string key, string dataKey, T t)
        {
            return await Database.HashSetAsync(AddSysCustomKey(key), dataKey, t.ToJson());
        }

        /// <summary>
        /// 移除hash中的某值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <returns></returns>
        public async Task<bool> HashDeleteAsync(string key, string dataKey)
        {
            return await Database.HashDeleteAsync(AddSysCustomKey(key), dataKey);
        }

        /// <summary>
        /// 移除hash中的多个值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dataKeys"></param>
        /// <returns></returns>
        public async Task<long> HashDeleteAsync(string key, List<RedisValue> dataKeys)
        {
            return await Database.HashDeleteAsync(AddSysCustomKey(key), dataKeys.ToArray());
        }

        /// <summary>
        /// 从hash表获取数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <returns></returns>
        public async Task<T> HashGeAsync<T>(string key, string dataKey)
        {
            string value = await Database.HashGetAsync(AddSysCustomKey(key), dataKey);
            return ConvertObject<T>(value);
        }

        /// <summary>
        /// 为数字增长val
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <param name="val">可以为负</param>
        /// <returns>增长后的值</returns>
        public async Task<double> HashIncrementAsync(string key, string dataKey, double val = 1)
        {
            return await Database.HashIncrementAsync(AddSysCustomKey(key), dataKey, val);
        }

        /// <summary>
        /// 为数字减少val
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <param name="val">可以为负</param>
        /// <returns>减少后的值</returns>
        public async Task<double> HashDecrementAsync(string key, string dataKey, double val = 1)
        {
            return await Database.HashDecrementAsync(AddSysCustomKey(key), dataKey, val);
        }

        /// <summary>
        /// 获取hashkey所有Redis key
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<List<T>> HashKeysAsync<T>(string key)
        {
            RedisValue[] values = await Database.HashKeysAsync(AddSysCustomKey(key));
            return ConvertList<T>(values);
        }

        #endregion 异步方法

        #endregion Hash

        #region List

        #region 同步方法

        /// <summary>
        /// 移除指定ListId的内部List的值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void ListRemove<T>(string key, T value)
        {
            Database.ListRemove(AddSysCustomKey(key), ConvertJson(value));
        }

        /// <summary>
        /// 获取指定key的List
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public List<T> ListRange<T>(string key)
        {
            var values = Database.ListRange(AddSysCustomKey(key));
            return ConvertList<T>(values);
        }

        /// <summary>
        /// 入队
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void ListRightPush<T>(string key, T value)
        {
            Database.ListRightPush(AddSysCustomKey(key), value.ToJson());
        }

        /// <summary>
        /// 出队
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T ListRightPop<T>(string key)
        {
            var value = Database.ListRightPop(AddSysCustomKey(key));
            return ConvertObject<T>(value);
        }

        /// <summary>
        /// 入栈
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void ListLeftPush<T>(string key, T value)
        {
            Database.ListLeftPush(AddSysCustomKey(key), ConvertJson(value));
        }

        /// <summary>
        /// 出栈
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T ListLeftPop<T>(string key)
        {
            var value = Database.ListLeftPop(AddSysCustomKey(key));
            return ConvertObject<T>(value);
        }

        /// <summary>
        /// 获取集合中的数量
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public long ListLength(string key)
        {
            return Database.ListLength(AddSysCustomKey(key));
        }

        #endregion 同步方法

        #region 异步方法

        /// <summary>
        /// 移除指定ListId的内部List的值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public async Task<long> ListRemoveAsync<T>(string key, T value)
        {
            return await Database.ListRemoveAsync(AddSysCustomKey(key), ConvertJson(value));
        }

        /// <summary>
        /// 获取指定key的List
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<List<T>> ListRangeAsync<T>(string key)
        {
            var values = await Database.ListRangeAsync(AddSysCustomKey(key));
            return ConvertList<T>(values);
        }

        /// <summary>
        /// 入队
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public async Task<long> ListRightPushAsync<T>(string key, T value)
        {
            key = AddSysCustomKey(key);
            return await Database.ListRightPushAsync(key, ConvertJson(value));
        }

        /// <summary>
        /// 出队
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<T> ListRightPopAsync<T>(string key)
        {
            var value = await Database.ListRightPopAsync(AddSysCustomKey(key));
            return ConvertObject<T>(value);
        }

        /// <summary>
        /// 入栈
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public async Task<long> ListLeftPushAsync<T>(string key, T value)
        {
            return await Database.ListLeftPushAsync(AddSysCustomKey(key), value.ToJson());
        }

        /// <summary>
        /// 出栈
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<T> ListLeftPopAsync<T>(string key)
        {
            var value = await Database.ListLeftPopAsync(AddSysCustomKey(key));
            return ConvertObject<T>(value);
        }

        /// <summary>
        /// 获取集合中的数量
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<long> ListLengthAsync(string key)
        {
            return await Database.ListLengthAsync(AddSysCustomKey(key));
        }

        #endregion 异步方法

        #endregion List

        #region SortedSet 有序集合

        #region 同步方法

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="score"></param>
        public bool SortedSetAdd<T>(string key, T value, double score)
        {
            return Database.SortedSetAdd(AddSysCustomKey(key), value.ToJson<T>(), score);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public bool SortedSetRemove<T>(string key, T value)
        {
            return Database.SortedSetRemove(AddSysCustomKey(key), ConvertJson(value));
        }

        /// <summary>
        /// 获取全部
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public List<T> SortedSetRangeByRank<T>(string key)
        {
            var values = Database.SortedSetRangeByRank(AddSysCustomKey(key));
            return ConvertList<T>(values);
        }

        /// <summary>
        /// 获取集合中的数量
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public long SortedSetLength(string key)
        {
            return Database.SortedSetLength(AddSysCustomKey(key));
        }

        #endregion 同步方法

        #region 异步方法

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="score"></param>
        public async Task<bool> SortedSetAddAsync<T>(string key, T value, double score)
        {
            return await Database.SortedSetAddAsync(AddSysCustomKey(key), value.ToJson<T>(), score);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public async Task<bool> SortedSetRemoveAsync<T>(string key, T value)
        {
            return await Database.SortedSetRemoveAsync(AddSysCustomKey(key), value.ToJson<T>());
        }

        /// <summary>
        /// 获取全部
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<List<T>> SortedSetRangeByRankAsync<T>(string key)
        {
            var values = await Database.SortedSetRangeByRankAsync(AddSysCustomKey(key));
            return ConvertList<T>(values);
        }

        /// <summary>
        /// 获取集合中的数量
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<long> SortedSetLengthAsync(string key)
        {
            return await Database.SortedSetLengthAsync(AddSysCustomKey(key));
        }

        #endregion 异步方法

        #endregion SortedSet 有序集合

        #region key

        /// <summary>
        /// 删除单个key
        /// </summary>
        /// <param name="key">redis key</param>
        /// <returns>是否删除成功</returns>
        public bool KeyDelete(string key)
        {
            return Database.KeyDelete(AddSysCustomKey(key));
        }

        /// <summary>
        /// 删除多个key
        /// </summary>
        /// <param name="keys">rediskey</param>
        /// <returns>成功删除的个数</returns>
        public long KeyDelete(List<string> keys)
        {
            List<string> newKeys = keys.Select(AddSysCustomKey).ToList();
            return Database.KeyDelete(ConvertRedisKeys(newKeys));
        }

        /// <summary>
        /// 判断key是否存储
        /// </summary>
        /// <param name="key">redis key</param>
        /// <returns></returns>
        public bool KeyExists(string key)
        {
            return Database.KeyExists(AddSysCustomKey(key));
        }

        /// <summary>
        /// 重新命名key
        /// </summary>
        /// <param name="key">就的redis key</param>
        /// <param name="newKey">新的redis key</param>
        /// <returns></returns>
        public bool KeyRename(string key, string newKey)
        {
            return Database.KeyRename(AddSysCustomKey(key), newKey);
        }

        /// <summary>
        /// 设置Key的时间
        /// </summary>
        /// <param name="key">redis key</param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        public bool KeyExpire(string key, TimeSpan? expiry = default(TimeSpan?))
        {
            return Database.KeyExpire(AddSysCustomKey(key), expiry);
        }

        #endregion key

        #region 发布订阅

        /// <summary>
        /// Redis发布订阅  订阅
        /// </summary>
        /// <param name="subChannel"></param>
        /// <param name="handler"></param>
        public void Subscribe(string subChannel, Action<RedisChannel, RedisValue> handler = null)
        {
            ISubscriber sub = Connection.GetSubscriber();
            sub.Subscribe(subChannel, (channel, message) =>
            {
                if (handler == null)
                {
                    Console.WriteLine(subChannel + " 订阅收到消息：" + message);
                }
                else
                {
                    handler(channel, message);
                }
            });
        }

        /// <summary>
        /// Redis发布订阅  发布
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="channel"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public long Publish<T>(string channel, T msg)
        {
            ISubscriber sub = Connection.GetSubscriber();
            return sub.Publish(channel, ConvertJson(msg));
        }

        /// <summary>
        /// Redis发布订阅  取消订阅
        /// </summary>
        /// <param name="channel"></param>
        public void Unsubscribe(string channel)
        {
            ISubscriber sub = Connection.GetSubscriber();
            sub.Unsubscribe(channel);
        }

        /// <summary>
        /// Redis发布订阅  取消全部订阅
        /// </summary>
        public void UnsubscribeAll()
        {
            ISubscriber sub = Connection.GetSubscriber();
            sub.UnsubscribeAll();
        }

        #endregion 发布订阅



        #region 辅助方法
        private string AddSysCustomKey(string oldKey)
        {
            var prefixKey = CustomKey ?? RedisConnectionHelper.SysCustomKey;
            return prefixKey + oldKey;
        }

        private string ConvertJson<T>(T value)
        {
            return value is string ? value?.ToString() : JsonConvert.SerializeObject(value);
        }

        private T ConvertObject<T>(RedisValue value)
        {
            if (typeof(T).Name.Equals(typeof(string).Name))
            {
                return JsonConvert.DeserializeObject<T>($"'{value}'");
            }
            return JsonConvert.DeserializeObject<T>(value);
        }

        private List<T> ConvertList<T>(RedisValue[] values)
        {
            var list = new List<T>();
            foreach (var item in values)
            {
                var model = ConvertObject<T>(item);
                list.Add(model);
            }
            return list;
        }

        private RedisKey[] ConvertRedisKeys(List<string> redisKey)
        {
            return redisKey.Select(s => (RedisKey)s).ToArray();
        }
        #endregion

    }

   
}

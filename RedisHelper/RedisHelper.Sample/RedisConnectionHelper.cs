using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using System.Collections.Concurrent;

namespace RedisHelper.Sample
{
    /// <summary>
    /// ConnectionMultiplexer 对象管理帮助类
    /// </summary>
    public class RedisConnectionHelper
    {
        //https://github.com/qq1206676756/RedisHelp/blob/master/RedisHelp/RedisConnectionHelp.cs

        private static readonly IConfiguration _configuration;
        private static readonly object _lock = new object();
        private static ConnectionMultiplexer _instance;

        public static string SysCustomKey;
        public static string RedisConnectionString;
        public static int DbNum;
        public static ConcurrentDictionary<string, ConnectionMultiplexer> ConnectionCache;

        // 静态构造
        static RedisConnectionHelper()
        {
            _configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appseetings.json")
                .Build();

            SysCustomKey = _configuration.GetSection("RedisKey").Value;
            RedisConnectionString = _configuration.GetConnectionString("RedisConnectionString").Trim();
            
            int.TryParse(_configuration.GetSection("RedisDBNumber").Value, out int dbNum);
            DbNum = dbNum;

            ConnectionCache = new ConcurrentDictionary<string, ConnectionMultiplexer>();
        }


        /// <summary>
        ///  单例获取
        /// </summary>
        public static ConnectionMultiplexer Instanse
        {
            get {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null || _instance.IsConnected)
                        {
                            _instance = GetConnectionMultiplexer();
                        }
                    }        
                }
                return _instance;          
            }
        }

        public static ConnectionMultiplexer GetConnectionMultiplexer(string connectionString = null)
        {
            connectionString ??= RedisConnectionString;

            if (!ConnectionCache.ContainsKey(connectionString))
                ConnectionCache[connectionString] = GetConnection(connectionString);

            return ConnectionCache[connectionString];
        }
        

        private static ConnectionMultiplexer GetConnection(string connectionString)
        {
            var connect = ConnectionMultiplexer.Connect(connectionString);

            //注册事件
            connect.ConnectionFailed += Connect_ConnectionFailed;
            connect.ConnectionRestored += Connect_ConnectionRestored;
            connect.ErrorMessage += Connect_ErrorMessage;
            connect.ConfigurationChanged += Connect_ConfigurationChanged;
            connect.HashSlotMoved += Connect_HashSlotMoved;
            connect.InternalError += Connect_InternalError;

            if (ConnectionCache.ContainsKey(connectionString))
            {
                ConnectionCache.TryAdd(connectionString, connect);
            }

            return connect;

        }

        #region 事件

        /// <summary>
        /// Redis异常
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void Connect_InternalError(object sender, InternalErrorEventArgs e)
        {
            throw new NotImplementedException("【Redis异常：InternalError】", e.Exception);
        }

        /// <summary>
        /// 更改集群
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void Connect_HashSlotMoved(object sender, HashSlotMovedEventArgs e)
        {
            var msg = "{\"NewEndPoint\":"+e.NewEndPoint+",\"OldEndPoint\":"+e.OldEndPoint+"}";
            throw new NotImplementedException("【更改集群：HashSlotMoved】:"+ msg + "");
        }

        /// <summary>
        /// 配置更改时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void Connect_ConfigurationChanged(object sender, EndPointEventArgs e)
        {
            var msg = "{\"EndPoint\":" + e.EndPoint + "}";
            throw new NotImplementedException("【配置更改：ConfigurationChanged】:" + msg + "");
        }

        /// <summary>
        /// Redis错误
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void Connect_ErrorMessage(object sender, RedisErrorEventArgs e)
        {
            var msg = "{\"EndPoint\":" + e.EndPoint + ",\"Message\":" + e.Message + "}";
            throw new NotImplementedException("【Redis错误：ErrorMessage】:" + msg + "");
        }

        /// <summary>
        /// 重新建立连接发生错误时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void Connect_ConnectionRestored(object sender, ConnectionFailedEventArgs e)
        {
            throw new NotImplementedException("【Redis错误：ConnectionRestored】:",e.Exception);
        }

        /// <summary>
        /// 连接失败，如果重新连接成功将不会进入此方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void Connect_ConnectionFailed(object sender, ConnectionFailedEventArgs e)
        {
            throw new NotImplementedException("【连接失败：ConnectionFailed】:", e.Exception);
        } 
        #endregion
    }
}

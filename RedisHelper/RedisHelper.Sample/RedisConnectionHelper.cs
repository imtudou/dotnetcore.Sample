using System;

using Microsoft.Extensions.Configuration;

using StackExchange.Redis;

namespace RedisHelper.Sample
{
    /// <summary>
    /// ConnectionMultiplexer 对象管理帮助类
    /// </summary>
    public static class RedisConnectionHelper 
    {
        //https://github.com/qq1206676756/RedisHelp/blob/master/RedisHelp/RedisConnectionHelp.cs
        public static IConfiguration configuration;
        public static readonly string SysCustomerKey = configuration.GetSection("RedisKey").Value;

    }
}

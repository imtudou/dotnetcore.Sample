using Newtonsoft.Json.Linq;

using RedisHelper.Sample;

using StackExchange.Redis;

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

using Xunit;

namespace RedisHelper.XUnitTestProject1
{
    public class StringUnitTest
    {
        private static ConnectionMultiplexer multiplexer;
        private static IDatabase database;


        public StringUnitTest()
        {
            multiplexer = RedisConnectionHelper.Instanse;//��ʼ��
            database = multiplexer.GetDatabase(0);//ָ�����ӵĿ� 0
        }




        
        
    }

}




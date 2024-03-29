using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using RedisHelper.Sample;

using RedisTestDto;

using StackExchange.Redis;

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Xunit;
using Xunit.Abstractions;

namespace RedisHelper.XUnitTestProject1
{

    public class StringUnitTest
    {
        RedisHelper.Sample.RedisHelper redisHelper;
        private readonly ConnectionMultiplexer Connection;
        private readonly IDatabase Database;
        protected readonly ITestOutputHelper testOutputHelper;
        public StringUnitTest(ITestOutputHelper testOutput)
        {
            redisHelper = new Sample.RedisHelper();
            Connection = RedisConnectionHelper.Instanse ?? RedisConnectionHelper.GetConnectionMultiplexer();
            Database = Connection.GetDatabase(0);
            testOutputHelper = testOutput;
        }

        /*
         * String, list  set hash zset
         */

        #region 基础数据结构
        [Fact]
        public void String()
        {
            var userInfo = new UserInfo_StringSetDto
            {
                Id = Guid.NewGuid().ToString(),
                Name = "张小三",
                Phone = "13500000001",
                Address = new Address
                {
                    Province = "上海",
                    City = "",
                    Town = "",
                    PostalCode = ""
                }

            };
            var serializeData = JsonConvert.SerializeObject(userInfo);
            var result = redisHelper.StringSet("String", serializeData);
            Assert.True(result);
            var result1 = redisHelper.StringGet<UserInfo_StringSetDto>("String");
        }

        [Fact]
        public void Hash()
        {
            //无序字典
            string guid = "Hash";
            var userInfo = new UserInfo_StringSetDto
            {
                Id = "1",
                Name = "张小三",
                Phone = "13500000001",
                Address = new Address
                {
                    Province = "上海",
                    City = "",
                    Town = "",
                    PostalCode = ""
                }
            };

            redisHelper.HashSet<string>(guid, nameof(userInfo.Id), "1");
            redisHelper.HashSet<string>(guid, nameof(userInfo.Id), "1");
            redisHelper.HashSet<string>(guid, nameof(userInfo.Id), "1");
            redisHelper.HashSet<string>(guid, nameof(userInfo.Name), userInfo.Name);
            redisHelper.HashSet<string>(guid, nameof(userInfo.Phone), userInfo.Phone);
            redisHelper.HashSet<Address>(guid, nameof(userInfo.Address), userInfo.Address);

            var name = redisHelper.HashGet<string>(guid, nameof(userInfo.Name));
        }

        [Fact]
        public void ListLeftPush()
        {
            // 先进先出
            string guid = "ListLeftPush";

            for (int i = 1; i < 5; i++)
            {
                Thread.Sleep(new TimeSpan(0, 0, 2));
                Database.ListLeftPush(guid, DateTime.Now.ToString());
            }


            var count = Database.ListLength(guid);
            for (int i = 0; i < count; i++)
            {
                var cc = Database.ListRightPop(guid);
            }




        }

        [Fact]
        public void ListLeftPush2()
        {
            string guid = "ListLeftPush2";

            for (int i = 1; i < 10; i++)
            {
                Thread.Sleep(new TimeSpan(0, 0, i));
                Database.ListLeftPush(guid, DateTime.Now.ToString());
            }

            Task.Run(() =>
            {
                var count = Database.ListLength(guid);
                for (int i = 0; i < count; i++)
                {
                    var cc = Database.ListLeftPop(guid);
                }
            });


        }

        [Fact]
        public void Set()
        {
            var userInfo = new UserInfo_StringSetDto
            {
                Id = Guid.NewGuid().ToString(),
                Name = "张小三",
                Phone = "13500000001",
                Address = new Address
                {
                    Province = "上海",
                    City = "",
                    Town = "",
                    PostalCode = ""
                }

            };

            var serializeData = JsonConvert.SerializeObject(userInfo);
            Database.SetAdd("Set", serializeData);
            for (int i = 0; i < Database.SetLength("Set"); i++)
            {
                var cc = Database.SetPop("Set");
            }

        }

        [Fact]
        public void Zset()
        {
            //有序列表
            /*
             *      一方面它是一个 set，保证了内部
                    value 的唯一性，另一方面它可以给每个 value 赋予一个 score，代表这个 value 的排序权
                    重。它的内部实现用的是一种叫着「跳跃列表」的数据结构。
             */
            string guid = "SortedSet";
            var userInfo1 = new UserInfo_StringSetDto
            {
                Id = "1",
                Name = "张小三",
                Phone = "13500000001",
                Address = new Address
                {
                    Province = "上海",
                    City = "",
                    Town = "",
                    PostalCode = "1"
                }
            };

            var userInfo2 = new UserInfo_StringSetDto
            {
                Id = "1",
                Name = "张小三",
                Phone = "13500000001",
                Address = new Address
                {
                    Province = "上海",
                    City = "",
                    Town = "",
                    PostalCode = "1"
                }
            };

            var userInfo3 = new UserInfo_StringSetDto
            {
                Id = "1",
                Name = "张小三",
                Phone = "13500000001",
                Address = new Address
                {
                    Province = "上海",
                    City = "",
                    Town = "",
                    PostalCode = "1"
                }
            };
            redisHelper.SortedSetAdd<UserInfo_StringSetDto>(guid, userInfo1, 1);
            redisHelper.SortedSetAdd<UserInfo_StringSetDto>(guid, userInfo2, 2);
            redisHelper.SortedSetAdd<UserInfo_StringSetDto>(guid, userInfo3, 3);

            var sortedSetResult = Database.SortedSetRangeByScore(guid);


        }
        #endregion

        #region 应用一：分布式锁
        [Fact]
        public void RedisLock()
        {
            //redis 锁:
            Thread t1 = new Thread(AddVal);
            Thread t2 = new Thread(AddVal);
            t1.Start();
            t2.Start();
        }

        public void AddVal()
        {
            for (int i = 0; i < 50000; i++)
            {
                var info = "name-" + Environment.MachineName;
                //如果5秒不释放锁 自动释放。避免死锁
                if (Database.LockTake("name", info, TimeSpan.FromSeconds(5)))
                {
                    try
                    {
                        var result = Database.StringGet("RedisLock");
                        Database.StringSet("RedisLock", (int)result + 1);
                    }
                    catch (Exception ex)
                    {

                    }
                    finally
                    {

                        Database.LockRelease("name", info);
                    }
                }

                var cc = Database.StringGet("RedisLock");
            }
        }
        #endregion


        #region 应用二：延迟队列
        // 使用Blocking 
        // Blocking 阻塞读 ListLeftPush/ListRightPop 
        #endregion


        [Fact]
        public void PushlisherSubscriber()
        {
            // 注：在多个实例中需要先运行订阅的代码在运行发布的代码
            var channel = "redisPubSubMsg";
            var msg = $"{DateTime.Now.ToString()}";

            ISubscriber sub = Connection.GetSubscriber();
            sub.Subscribe(channel, (channel, message) =>
            {
                testOutputHelper.WriteLine($"订阅消息1：{(string)message}");
            });
            sub.Subscribe(channel, (channel, message) =>
            {
                testOutputHelper.WriteLine($"订阅消息2：{(string)message}");
            });

            ISubscriber pub = Connection.GetSubscriber();
            var pubresult = pub.Publish(channel, msg);
            testOutputHelper.WriteLine($"发布消息{msg.ToString()}");
        }



        [Fact]
        public void ConvertBase64()
        {
            string a = "abc";
            byte[] bt = Encoding.Default.GetBytes(a);
            var encrypt = Convert.ToBase64String(bt);
            testOutputHelper.WriteLine("encryptStr:" + encrypt);

            var decode = Encoding.Default.GetString(Convert.FromBase64String(encrypt));
            testOutputHelper.WriteLine("decode:" + decode);

        }
    }

}




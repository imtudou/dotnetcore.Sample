﻿using System;
using System.Text;
using System.Threading;

using RabbitMQ.Client;

namespace Send
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("------发送端：------");
            //1.实例化连接工厂
            var factory = new ConnectionFactory { HostName = "localhost" };

            //2.建立连接
            using (var connection = factory.CreateConnection())
            {
                //3.创建信道
                using (var channel = connection.CreateModel())
                {
                    //4. 申明队列 (指定durable:true,告知rabbitmq对消息进行持久化)
                    channel.QueueDeclare(queue: "hello", durable: true, exclusive: false, autoDelete: false, arguments: null);
                    //将消息标记为持久性 - 将IBasicProperties.SetPersistent设置为true
                    var properties = channel.CreateBasicProperties();
                    properties.Persistent = true;

                    //5.构建byte消息数据包
                    string msg = $"Hello RabbitMQ! { DateTime.Now.Ticks}";
                    var body = Encoding.UTF8.GetBytes(msg);

                    //6.发送数据包
                    channel.BasicPublish(exchange: "", routingKey: "hello", basicProperties: properties, body: body);
                    Console.WriteLine(" send {0}", msg);
                }
            }
            Console.ReadKey();
        }
    }   
}

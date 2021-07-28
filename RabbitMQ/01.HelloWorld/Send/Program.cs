using System;
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
            using (var connection = factory.CreateConnection("http://localhost:15672/"))
            {
                //3.创建信道
                using (var channel = connection.CreateModel())
                {
                    //4. 申明队列
                    channel.QueueDeclare(queue: "hello", durable: false, exclusive: false, autoDelete: false, arguments: null);

                    //5.构建byte消息数据包
                    string msg = args.Length > 0 ? args[0] : $"Hello RabbitMQ! { DateTime.Now.Ticks}";
                    var body = Encoding.UTF8.GetBytes(msg);

                    //6.发送数据包
                    channel.BasicPublish(exchange: "", routingKey: "hello", basicProperties: null, body: body);
                    Console.WriteLine(" send {0}", msg);
                }
            }
            Console.ReadKey();
        }
    }
}

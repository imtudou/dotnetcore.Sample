using System;
using System.Text;
using System.Threading;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Receive
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("------接收端：------");

            //1.实例化连接工厂
            var factory = new ConnectionFactory { HostName = "localhost" };

            //2.建立连接
            using (var connection = factory.CreateConnection("http://localhost:15672/"))
            {
                //3.创建信道
                using (var channel = connection.CreateModel())
                {
                    //4.申明队列
                    channel.QueueDeclare(queue: "hello");

                    //5.构建消费者实例
                    var consumer = new EventingBasicConsumer(channel);

                    //6.绑定消息接收后台的事件委托
                    consumer.Received += (model, ea) =>
                    {
                        var message = Encoding.UTF8.GetString(ea.Body.Span);
                        Console.WriteLine(" [x] Received {0}", message);
                        Thread.Sleep(3000);//模拟耗时
                        Console.WriteLine(" [x] Done");
                    };

                    //7. 启动消费者
                    channel.BasicConsume(queue: "hello", autoAck: true, consumer: consumer);
                    Console.WriteLine(" Press [enter] to exit.");
                    Console.ReadLine();
                }
            }
        }
    }
}

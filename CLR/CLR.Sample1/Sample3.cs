using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CLR.Sample1
{
    public class Sample3
    {
        // 协作式取消和超时

        public static void Cancellation()
        {
            CancellationTokenSource cts = new CancellationTokenSource();
            ThreadPool.QueueUserWorkItem(state => Count(cts.Token, 1000));
            Console.WriteLine("Press <Enter> to cancel the operation.");
            Console.ReadLine();
            cts.Cancel();// 如果Count 方法已返回， Cancel 没有任何效果
            Console.ReadKey();
        }


        public static void CancellationRegister()
        {
            // 注册一个或者多个要取消的方法
            CancellationTokenSource cts = new CancellationTokenSource();
            cts.Token.Register(() => { Console.WriteLine("cancel 1"); });
            cts.Token.Register(() => { Console.WriteLine("cancel 2"); });
            cts.Cancel();

            /*
                 output
                  cancel  2
                  cancel  1
             
             */

        }


        public static void CancellationRegisterMulti()
        {
            //创建一个CancellationTolenSource
            var cts1 = new CancellationTokenSource();
            cts1.Token.Register(() => Console.WriteLine("cts1 canceled"));

            var cts2 = new CancellationTokenSource();
            cts2.Token.Register(() => Console.WriteLine("cts2 canceled"));

            // 创建一个新的CancellationTolenSource,他在cts1 或cts2 取消时取消
            var likedCts = CancellationTokenSource.CreateLinkedTokenSource(cts1.Token,cts2.Token);
            likedCts.Token.Register(() => Console.WriteLine("likeedCts canceled"));

            // 取消其中一个CancellationTokenSource 对象
            cts2.Cancel();

            // 显示哪个CancellationTolenSource 被取消了
            Console.WriteLine("cts1 = {0},cts2 = {1}, likedCts = {2}",cts1.IsCancellationRequested,cts2.IsCancellationRequested,likedCts.IsCancellationRequested);
            Console.ReadKey();

            /*
             * output:
                likeedCts canceled
                cts2 canceled
                cts1 = False,cts2 = True, likedCts = True
             */

        }


        public static void TaskUsing()
        { 
        
        }


        private static void Count(CancellationToken token, int num)
        {
            for (int i = 0; i < num; i++)
            {
                if (token.IsCancellationRequested)
                {
                    Console.WriteLine("取消操作！");
                    break;// 退出循环停止操作
                }
                Console.WriteLine(i);
                Thread.Sleep(200);
            }
            Console.WriteLine("Count is done!");
        }
    }
}

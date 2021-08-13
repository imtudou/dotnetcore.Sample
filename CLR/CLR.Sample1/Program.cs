using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CLR.Sample1
{
    class Program
    {
        static void Main(string[] args)
        {
            //sample0

            /*
                以下方法演示如何创建一个线程并异步调用一个方法
             */
            //Thread dedicatedThread = new Thread(Sample1.ComputeBoundOp);  // 默认为前台线程
            //dedicatedThread.IsBackground = true; // 使线程成为后台线程
            //dedicatedThread.Start(5);
            //Thread.Sleep(5000);        // 模拟做其他耗时工作5s
            //dedicatedThread.Join();    // 等待线程终止

            /*
                 如果 dedicatedThread 是前台线程，则应大约10s后终止
                                       后台线程，这应立即终止
             */




            //sample1
            //Console.WriteLine("Main Thread Start .....");
            //ThreadPool.QueueUserWorkItem(Sample1.ComputeBoundOp, 5);
            //Console.WriteLine("Child Thread DoSomething .....");
            //Thread.Sleep(1000);
            //Console.WriteLine("Main Thread End .....");

            /*
              output

                    Main Thread Start .....
                    Child Thread DoSomething .....
                    In ComputeBoundOp:5
                    Main Thread End .....
                
                or

             
              
             */




            // sample2
            //Sample2.ExecutionContextMethod();
            // output
            /*
                  name = 
                  name = zhangsan 
             */


            // sample3
            //Sample3.Cancellation();
            //Sample3.CancellationRegister();
            //Sample3.CancellationRegisterMulti();


            // sample4
            //Sample4.GetTaskResultWaitForTaskComplete();
            Sample4.CancelTask();






            Console.ReadKey();

        }
    }
}

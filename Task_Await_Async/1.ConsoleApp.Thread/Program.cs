using System;
using System.Threading;

namespace _1.ConsoleAppThread
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("begin");

            Thread thread = new Thread(() => TestMethod(2));
            thread.IsBackground = true;//设置为后台线程，默认前台线程
            thread.Start();

            Thread thread1 = new Thread(() => TestMethod1());
            //设置thread1优先级为最高，系统尽可能单位时间内调度该线程，默认为Normal
            thread1.Priority = ThreadPriority.Highest;
            thread1.Start();

            Thread thread2 = new Thread((state) => TestMethod2(state));
            thread2.Start("data");
            thread2.Join();//等待thread2执行完成
            Console.WriteLine("end");


        }

        static void TestMethod(int a)
        {
            Thread.Sleep(1000);
            Console.WriteLine($"TestMethod: run on Thread id :{Thread.CurrentThread.ManagedThreadId},is threadPool:{Thread.CurrentThread.IsThreadPoolThread}" +
                $",is Backgound:{Thread.CurrentThread.IsBackground}, result:{a}");
        }

        static void TestMethod1()
        {
            Thread.Sleep(1000);
            Console.WriteLine($"TestMethod1: run on Thread id :{Thread.CurrentThread.ManagedThreadId},is threadPool:{Thread.CurrentThread.IsThreadPoolThread}" +
                $",is Backgound:{Thread.CurrentThread.IsBackground},no result ");
        }

        static void TestMethod2(object state)
        {
            Thread.Sleep(2000);
            Console.WriteLine($"TestMethod2 :run on Thread id :{Thread.CurrentThread.ManagedThreadId},is threadPool:{Thread.CurrentThread.IsThreadPoolThread}" +
               $",is Backgound:{Thread.CurrentThread.IsBackground},result:{state}");
        }
    }
}

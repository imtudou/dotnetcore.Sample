using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncAndAwait.Sample
{

    /*
     点外卖场景：
        1.点外卖（30分钟）DoTakeOut()...
        2.看视频（4h）    WathcTv()...
        3.洗衣服 (40分钟) Wash()...
     
     */
    class Program
    {
        static void Main(string[] args)
        {
            //Stopwatch stopwatch = new Stopwatch();
            //stopwatch.Start();
            //Console.WriteLine("Begin...");
            //DoSomething();
            //Console.WriteLine("End...");
            //stopwatch.Stop();
            //TimeSpan ts = stopwatch.Elapsed;
            //// Format and display the TimeSpan value.
            //string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
            //    ts.Hours, ts.Minutes, ts.Seconds,
            //    ts.Milliseconds / 10);
            //Console.WriteLine("同步执行共耗时:{0}s...",elapsedTime);
            //Console.WriteLine("===================================");

            //Stopwatch stopwatch1 = new Stopwatch();
            //stopwatch1.Start();
            //Console.WriteLine("Begin...");
            //DoSomethingTask();
            //Console.WriteLine("End...");
            //stopwatch1.Stop();
            //TimeSpan ts1 = stopwatch1.Elapsed;
            //// Format and display the TimeSpan value.
            //string elapsedTime1 = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
            //    ts1.Hours, ts1.Minutes, ts1.Seconds,
            //    ts1.Milliseconds / 10);
            //Console.WriteLine("异步执行共耗时:{0}s...", elapsedTime1);


            Stopwatch stopwatch1 = new Stopwatch();
            stopwatch1.Start();
            Console.WriteLine("Begin...");
            DoSomethingAsync();
            Console.WriteLine("End...");
            stopwatch1.Stop();
            TimeSpan ts1 = stopwatch1.Elapsed;
            string elapsedTime1 = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts1.Hours, ts1.Minutes, ts1.Seconds,
                ts1.Milliseconds / 10);
            Console.WriteLine("异步执行共耗时:{0}s...", elapsedTime1);






            Console.ReadKey();
        }


        static void DoSomething()
        {
            DoTakeOut();
            WathcTv();
            Wash();
        }

        static void DoSomethingTask()
        {
            Task task1 = new Task(() => DoTakeOut());
            Task task2 = new Task(() => WathcTv());
            Task task3 = new Task(() => Wash());
            task1.Start();
            task2.Start();
            task3.Start();
            Task.WaitAll(task1, task2, task3);
        }


        async static void DoSomethingAsync()
        {
            await DoTakeOutAsync();
            await WathcTvAsync();
            await WashAsync();
        }   

        static void DoTakeOut()
        {
            Thread.Sleep(11000);
            Console.WriteLine("点外卖（11s）DoTakeOut()...");        
        }

        async static Task<int> DoTakeOutAsync()
        {
            await Task.Delay(1);
            Thread.Sleep(11000);
            Console.WriteLine("点外卖（11s）DoTakeOut()...");
            return 0;
        }

        static void WathcTv()
        {
            Thread.Sleep(6000);
            Console.WriteLine("看视频（6s）WathcTv()...");
        }
        async static Task<int> WathcTvAsync()
        {
            await Task.Delay(1);
            Thread.Sleep(6000);
            Console.WriteLine("看视频（6s）WathcTv()...");
            return 0;
        }

        static void Wash()
        {
            Thread.Sleep(14000);
            Console.WriteLine("洗衣服 (14s) Wash()...");
        }

        async static Task<int> WashAsync()
        {
            await Task.Delay(1);    
            Thread.Sleep(14000);
            Console.WriteLine("洗衣服 (14s) Wash()...");
            return 0;
        }

        public async Task<string> GetHtmlAsync()
        {
            using (var client = new HttpClient())
            {
                return await client.GetStringAsync("http://www.baidu.com");
            }
        }
    }
}

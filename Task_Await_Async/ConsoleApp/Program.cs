using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp
{

    /*
     点外卖场景：
        1.点外卖（3s）DoTakeOut()...
        2.看视频（9s）WathcTv()...
        3.洗衣服 (3s) Wash()...
     */
    class Program
    {
        static void Main(string[] args)
        {
            //同步单线程执行
            //Method1();//00:00:15.04s

            //多线程执行
            //Method1Task();//00:00:09.25s


            //一个业务中存在多个线程，且需要对线程进行管理，相对麻烦，从而引出了异步方法。
            /*
                　观点结论1：从 Method1Async() 中可以得出一个结论，async中必须要有await运算符才能起到异步方法的作用，且await 运算符只能加在 系统类库默认提供的异步方法或者新线程（如：Task.Run）前面。
             */
            //Method1Async();//00:00:21.17s

            //Method2Async();//00:00:15.20s

           
            DoSomething3();
            DoSomething4();
            GetHtmlAsync();
            Console.WriteLine("主线程{0}", Thread.GetCurrentProcessorId());


            Console.ReadKey();
        }

        static void Method1()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            Console.WriteLine("Begin...");

            Console.WriteLine(DoTakeOut());
            Console.WriteLine(WathcTv());
            Console.WriteLine(Wash());

            Console.WriteLine("End...");
            stopwatch.Stop();
            TimeSpan ts = stopwatch.Elapsed;
            // Format and display the TimeSpan value.
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);
            Console.WriteLine("同步执行共耗时:{0}s...", elapsedTime);
            Console.WriteLine("===================================");
        }
        static void Method1Task()
        {
            Stopwatch stopwatch1 = new Stopwatch();
            stopwatch1.Start();
            Console.WriteLine("Begin...");

            var task1 = Task.Run(() =>
            {
                return DoTakeOut();
            });
            var task2 = Task.Run(() =>
            {
                return WathcTv();
            });
            var task3 = Task.Run(() =>
            {
                return Wash();
            });

            //主线程进行等待
            Task.WaitAll(task1, task2, task3);
            Console.WriteLine($"{task1.Result}");
            Console.WriteLine($"{task2.Result}");
            Console.WriteLine($"{task3.Result}");

            Console.WriteLine("End...");
            stopwatch1.Stop();
            TimeSpan ts1 = stopwatch1.Elapsed;
            // Format and display the TimeSpan value.
            string elapsedTime1 = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts1.Hours, ts1.Minutes, ts1.Seconds,
                ts1.Milliseconds / 10);
            Console.WriteLine("多线程执行共耗时:{0}s...", elapsedTime1);
        }
        static void Method1Async()
        {
            Stopwatch stopwatch1 = new Stopwatch();
            stopwatch1.Start();
            Console.WriteLine("Begin...");

            Console.WriteLine(DoTakeOut1Async().Result);
            Console.WriteLine(Wash1Async().Result);
            Console.WriteLine(WathcTv1Async().Result);

            Console.WriteLine("End...");
            stopwatch1.Stop();
            TimeSpan ts1 = stopwatch1.Elapsed;
            string elapsedTime1 = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts1.Hours, ts1.Minutes, ts1.Seconds,
                ts1.Milliseconds / 10);
            Console.WriteLine("异步方法执行共耗时:{0}s...", elapsedTime1);
        }
        static async void Method2Async()
        {
            Stopwatch stopwatch1 = new Stopwatch();
            stopwatch1.Start();
            Console.WriteLine("Begin...");
            var result1 = await DoTakeOut2Async();
            var result2 = await Wash2Async();
            var result3 = await WathcTv2Async();
            Console.WriteLine(result1);
            Console.WriteLine(result2);
            Console.WriteLine(result3);

            Console.WriteLine("End...");
            stopwatch1.Stop();
            TimeSpan ts1 = stopwatch1.Elapsed;
            string elapsedTime1 = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts1.Hours, ts1.Minutes, ts1.Seconds,
                ts1.Milliseconds / 10);
            Console.WriteLine("异步方法执行共耗时:{0}s...", elapsedTime1);
        }




        static string DoTakeOut()
        {
            Thread.Sleep(3000);
            return "点外卖3s";
        }
        static string Wash()
        {
            Thread.Sleep(3000);
            return "洗衣服3s";
        }
        static string WathcTv()
        {
            Thread.Sleep(9000);
            return "看视频9s";
        }

        static async Task<string> DoTakeOut1Async()
        {
            Thread.Sleep(3000);
            return "点外卖3s";
        }
        static async Task<string> Wash1Async()
        {
            Thread.Sleep(3000);
            return "洗衣服3s";
        }
        static async Task<string> WathcTv1Async()
        {
            Thread.Sleep(9000);
            return "看视频9s";
        }

        static async Task<string> DoTakeOut2Async()
        {
            return await Task.Run(() =>
            {
                Thread.Sleep(3000);
                return "点外卖3s";
            });
        }
        static async Task<string> Wash2Async()
        {
            return await Task.Run(() =>
            {
                Thread.Sleep(3000);
                return "洗衣服3s";
            });
        }
        static async Task<string> WathcTv2Async()
        {
            return await Task.Run(() =>
            {
                Thread.Sleep(9000);
                return "看视频9s";
            });
        }
        static Task<string> GetHtmlAsync()
        {
            Task<string> str = null;
            using (var client = new HttpClient())
            {
                str = client.GetStringAsync("http://www.imtudou.cn");
            }
            Console.WriteLine($"{str}");
            return str;
        }


        static Task<string> DoSomething3()
        {
            return Task.Run<string>(() =>
            {
                Thread.Sleep(5000);
                Console.WriteLine("子线程{0}：DoSomething3", Thread.GetCurrentProcessorId());
                return "DoSomething3";

            });
        }

        static async Task<string> DoSomething4()
        {
            return await Task.Run<string>(() =>
            {
                Thread.Sleep(3000);
                Console.WriteLine("子线程{0}：DoSomething4", Thread.GetCurrentProcessorId());
                return "DoSomething4";
            });
        }
    }
}

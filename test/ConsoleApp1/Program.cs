using System;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            Random random = new Random();
        

            for (int i = 0; i < 100; i++)
            {
                int r = random.Next(1, 30);
                Console.WriteLine(r);

            }
            //TimeSpan timeSpan = new TimeSpan(0, 1, r);

            //Timer timer = new Timer(Callback, null, timeSpan, Timeout.InfiniteTimeSpan);

            //// 防止主线程退出，以便观察Timer的回调
            //Console.WriteLine(DateTime.Now);
            //Console.WriteLine("Waiting for the random trigger...");
            Console.ReadLine();   
        }


        private static void Callback(Object o)
        {
            Console.WriteLine("Triggered at a random time within 1 minute!");
            // 在这里执行你想要的操作
            Console.WriteLine(DateTime.Now);
        }

        public static void test2()
        {

        }

        public static void test1()
        {
            //var counter = 0;
            //var max = args.Length != 0 ? Convert.ToInt32(args[0]) : -1;
            //while (max == -1 || counter < max)
            //{
            //    Console.WriteLine($"Counter: {++counter}");
            //    Task.Delay(1000);
            //}
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            var str = string.Empty;
            for (int i = 0; i < 10000; i++)
            {
                str += i + ";";
            }
            TimeSpan ts = stopwatch.Elapsed;
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
              ts.Hours, ts.Minutes, ts.Seconds,
              ts.Milliseconds / 10);
            Console.WriteLine(str);
            Console.WriteLine("String:{0}", elapsedTime);


            Stopwatch stopwatch1 = new Stopwatch();
            stopwatch1.Start();
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < 10000; i++)
            {
                sb.Append(i + ";");
            }
            TimeSpan ts1 = stopwatch1.Elapsed;
            string elapsedTime1 = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
              ts1.Hours, ts1.Minutes, ts1.Seconds,
              ts1.Milliseconds / 10);
            Console.WriteLine(sb.ToString());
            Console.WriteLine("StringBuilder:{0}", elapsedTime1);
        }
    }
}

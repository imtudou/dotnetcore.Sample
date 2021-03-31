using System;
using System.Threading;

namespace Console.Observer3.BeginInvoke2
{
    public delegate int AddCalculatorEventHandler(int a, int b);
    public class Calculator
    {
        public int Add(int x, int y)
        {
            if (Thread.CurrentThread.IsThreadPoolThread)
            {
                Thread.CurrentThread.Name = nameof(Calculator);
            }
            System.Console.WriteLine("------Calculator.Add.Begin");

            // 执行某些事情，模拟需要执行2秒钟
            for (int i = 1; i <= 2; i++)
            {
                Thread.Sleep(TimeSpan.FromSeconds(i));
                System.Console.WriteLine("{0}: Add()计算耗时 {1} s",
                    Thread.CurrentThread.Name, i);
            }
            System.Console.WriteLine("------Calculator.Add.End");
            return x + y;
        }
    }



    /// <summary>
    /// 委托和方法的异步调用
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            System.Console.WriteLine("Client application started!\n");
            Thread.CurrentThread.Name = nameof(Program);

            Calculator cal = new Calculator();
            AddCalculatorEventHandler addCalculator = new AddCalculatorEventHandler(cal.Add);
           IAsyncResult asyncResult = addCalculator.BeginInvoke(2,5, null, null);//异步调用方法
          
            // 做某些其它的事情，模拟需要执行3秒钟
            for (int i = 1; i <= 3; i++)
            {   
                Thread.Sleep(TimeSpan.FromSeconds(i));
                System.Console.WriteLine("{0}: Client 耗时 {1} s",
                    Thread.CurrentThread.Name, i);
            }

            int result = addCalculator.EndInvoke(asyncResult);
            System.Console.WriteLine("Result: {0}\n", result);



            System.Console.WriteLine("Client application end!\n");

            System.Console.WriteLine("\nPress any key to exit...");
            System.Console.ReadKey();
        }

        public static void MyAsyncCallback(IAsyncResult asyncResult)
        {

        }
    }
}

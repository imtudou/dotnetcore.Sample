using System;

namespace Console.Delegate.Sample2._1
{
    // https://www.cnblogs.com/JimmyZhang/archive/2007/09/23/903360.html
    /// <summary>
    /// 定义委托类型
    /// </summary>
    /// <param name="name"></param>
    public delegate void GreetingDelegate(string name);
    class Program
    {
        static void Main(string[] args)
        {
            /*
             * 方式1
             */
            //Speack speack = new Speack();
            //speack.GreetingManage("zss", Speack.EnglishGreeting);

            /*
             方式2
             */
            Speack speack = new Speack();
            GreetingDelegate greetingDelegate;
            greetingDelegate = Speack.ChineseGreeting;
            greetingDelegate += Speack.EnglishGreeting;
            speack.GreetingManage("zss", greetingDelegate);


            System.Console.ReadKey();
        }
    }

    public class Speack 
    {
        public void GreetingManage(string name,GreetingDelegate greetingDelegate)
        {
            greetingDelegate(name);
        }
        public static void EnglishGreeting(string name)
        {
            System.Console.WriteLine("Morning, " + name);
        }

        public static void ChineseGreeting(string name)
        {
            System.Console.WriteLine("早上好, " + name);
        }

    }

}

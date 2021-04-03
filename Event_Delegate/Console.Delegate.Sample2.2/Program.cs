using System;

namespace Console.Delegate.Sample2._2
{
    /// <summary>
    /// 定义委托
    /// </summary>
    /// <param name="name"></param>
    public delegate void GreatingDelegate(string name);

    class Program
    {
        static void Main(string[] args)
        {
            Speack speack = new Speack();
            speack.greatingDelegate = Speack.SayChinese;
            speack.greatingDelegate += Speack.SayEnglish;
            speack.GreatingManage("zss");

            System.Console.ReadKey();
        }
    }

    public class Speack
    {
        /// <summary>
        /// 声明委托变量
        /// </summary>
        public GreatingDelegate greatingDelegate;

        public void GreatingManage(string name)
        {
            if (greatingDelegate != null) // 如果有方法注册委变量
            {
                // 通过委托调用方法
                greatingDelegate(name);
            }
        
        }

        public static void SayChinese(string name) 
        {
            System.Console.WriteLine($"早上好：{name}");        
        }

        public static void SayEnglish(string name)
        {
            System.Console.WriteLine($"Morning：{name}");
        }

    }
}

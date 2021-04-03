using System;

namespace Console.Delegate.Sample2._3_Event
{
    /// <summary>
    /// 定义委托类型
    /// </summary>
    /// <param name="name"></param>
    public delegate void GreatingDelegate(string name);
    class Program
    {
        static void Main(string[] args)
        {
            Speack s = new Speack();
            s.greatingDelegate += Speack.SayChinese;
            s.greatingDelegate += Speack.SayEnglish;
            s.GreatingManage("zsss");


            System.Console.ReadKey();
        }
    }

    public class Speack
    {
        /// <summary>
        /// 声明委托变量(用事件限定访问,声明一个事件即声明了一个封装了委托类型的变量）
        /// 在类的内部，不管你声明它是public还是protected，它总是private的。
        /// 在类的外部，注册“+=”和注销“-=”的访问限定符与你在声明事件时使用的访问符相同。
        /// </summary>
        public event GreatingDelegate greatingDelegate;

        public void GreatingManage(string name)
        {
            greatingDelegate(name);
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

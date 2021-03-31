using System;

namespace Console.Observer1.WhyReturnVoid
{
    /// <summary>
    /// 定义委托
    /// </summary>
    public delegate string NumberChangedEventHandler(int count);

    /// <summary>
    /// 定义事件发布者
    /// </summary>
    public class Publisher
    {
        //public NumberChangedEventHandler NumberChanged; //定义委托变量
        public event NumberChangedEventHandler NumberChanged; // 

        public string PublishNumber(int count)
        {
            if (NumberChanged != null)
            {
               return NumberChanged(count);
            }

            return "";
        }

    }

    /// <summary>
    /// 定义事件订阅者
    /// </summary>
    public class Subscriber1
    {
        public string OnNumberChanged(int count)
        {
            return $"Subscriber1:{count}";
        }
    }

    public class Subscriber2
    {
        public string OnNumberChanged(int count)
        {
            return $"Subscriber2:{count}";
        }
    }

    public class Subscriber3
    {
        public string OnNumberChanged(int count)
        {
            return $"Subscriber3:{count}";
        }
    }

    //为什么委托定义的返回值通常都为void？
    /*
        这是因为委托变量可以供多个订阅者注册，如果定义了返回值，
    那么多个订阅者的方法都会向发布者返回数值，
    结果就是后面一个返回的方法值将前面的返回值覆盖掉了，
    因此，实际上只能获得最后一个方法调用的返回值。可以运行下面的代码测试一下。
    除此以外，发布者和订阅者是松耦合的，发布者根本不关心谁订阅了它的事件、
    为什么要订阅，更别说订阅者的返回值了，
    所以返回订阅者的方法返回值大多数情况下根本没有必要。
     */

    class Program
    {
        static void Main(string[] args)
        {
            Publisher pu = new Publisher();
            Subscriber1 sub1 = new Subscriber1();
            Subscriber2 sub2 = new Subscriber2();
            Subscriber3 sub3 = new Subscriber3();

            pu.NumberChanged += sub1.OnNumberChanged;
            pu.NumberChanged += sub2.OnNumberChanged;
            pu.NumberChanged += sub3.OnNumberChanged;

            var result = pu.PublishNumber(100);
            System.Console.WriteLine(result);

            System.Console.ReadKey();
        }
    }
}

using System;

namespace Console.Observer
{
    /// <summary>
    /// 定义委托
    /// </summary>
    public delegate void NumberChangedEventHandler(int count);

    /// <summary>
    /// 定义事件发布者
    /// </summary>
    public class Publisher
    {
        private int count;
        //public NumberChangedEventHandler NumberChanged; //定义委托变量
        public event NumberChangedEventHandler NumberChanged; // 

        public void PublishNumber()
        {
            if (NumberChanged != null)
            {
                NumberChanged(count);
            }        
        }

    }

    /// <summary>
    /// 定义事件订阅者
    /// </summary>
    public class Subscriber
    {
        public void OnNumberChanged(int count) 
        {
            System.Console.WriteLine($"Subscriber.OnNumberChanged:{count}");
        
        }
    
    }



    class Program
    {
        static void Main(string[] args)
        {
            Publisher pub = new Publisher();
            Subscriber sub = new Subscriber();

            pub.NumberChanged += new NumberChangedEventHandler(sub.OnNumberChanged);
            pub.PublishNumber();          // 应该通过PublishNumber()来触发事件
            //pub.NumberChanged(100);     // 但可以被这样直接调用，对委托变量的不恰当使用

            System.Console.ReadKey();
        }
    }
}

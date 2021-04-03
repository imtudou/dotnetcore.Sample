using System;
using System.Collections.Generic;

namespace Console.Observer2.ReturnMulti
{
    /// <summary>
    /// 定义委托
    /// </summary>
    public delegate string NumberChangedEventHandler(int count);

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

    /// <summary>
    /// 定义事件发布者
    /// </summary>
    public class Publisher
    {
        //public NumberChangedEventHandler NumberChanged; //定义委托变量
        public event NumberChangedEventHandler NumberChanged; // 

        public List<string> PublishNumber(int count)
        {
            List<string> list = new List<string>();
            if (NumberChanged == null) return list;

            // 获得委托数组
            Delegate[] delegateArray = NumberChanged.GetInvocationList();

            foreach (var item in delegateArray)
            {
                // 进行一个向下转换
                NumberChangedEventHandler method = (NumberChangedEventHandler)item;
                if (NumberChanged == null) return list;
                list.Add(method(100));       // 调用方法并获取返回值
            }
            return list;
        }

    }


    /// <summary>
    /// 获得多个返回值与异常处理
    /// </summary>
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

            foreach (var item in pu.PublishNumber(100))
            {
                System.Console.WriteLine(item);
            }
            System.Console.ReadKey();
        }
    }
}

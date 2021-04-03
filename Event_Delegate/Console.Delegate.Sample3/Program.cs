using System;

namespace Console.Delegate.Sample3
{
    /*
        委托高级语法
        Action<>;
        Func>
     */
    class Program
    {
        static void Main(string[] args)
        {
            //使用委托1
            /*
             * 泛型委托Action<T>和Func<T>
             */
            System.Console.WriteLine("Action<T>,无返回值");
            Action<string> action1 = new Action<string>(Sepack_Say.SayChinese);
            action1("李四");
            Action<string, string> action2 = new Action<string, string>(Sepack_Say.SayChinese);
            action2("李四", "上海");

            System.Console.WriteLine("Func<T>,有回值");
            Func<string, string> func1 = new Func<string, string>(Sepack_Say.SayChinese_str);
            System.Console.WriteLine(func1("王五"));

            Func<string, string, string> func2 = new Func<string, string, string>(Sepack_Say.SayChinese_str);
            System.Console.WriteLine(func2("王五", "北京"));


            //使用委托2
            /*
             * 匿名方法和Lambda表达式
             */
            string func_para_name = "张小三";
            Action<string> action2_anonymou = delegate (string func_para_name)
             {
                 System.Console.WriteLine(func_para_name);
             };

            Func<string, string> func2__anonymou = delegate (string func_para_name)
            {
                return $"{func_para_name}用中文:说你好！";
            };

            Func<string, string> func2_lambda = str =>
            {
                return $"{func_para_name}用中文:说你好！";
            };

            System.Console.ReadKey();
        }
    }
}

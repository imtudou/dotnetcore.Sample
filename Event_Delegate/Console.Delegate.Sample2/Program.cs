using System;

namespace Console.Delegate.Sample2
{

    public class Program
    {
        static void Main(string[] args)
        {
            System.Console.WriteLine($"----------------{nameof(Sepack_Vsersion1)}:Begin");
            Sepack_Vsersion1 s1 = new Sepack_Vsersion1();
            s1.SaySomething(LanguageEnum.Chinese,"zs");
            System.Console.WriteLine($"----------------{nameof(Sepack_Vsersion1)}:End");


            // 使用委托实现1
            /*
                1.定义委托
                2.声明委托实例
                3.调用               
             */
            System.Console.WriteLine($"----------------{nameof(Sepack_Vsersion2)}:Begin");
            System.Console.WriteLine($"无返回值");
            Sepack_Vsersion2 s2 = new Sepack_Vsersion2();
            //s2.SaySomething("zsss", Sepack_Say.SayChinese);
            //s2.SaySomething("zsss", Sepack_Say.SayEnglish);
            //s2.SaySomething("zsss", Sepack_Say.SayJapanese);

            /*
             或者如下调用：
             */
            s2.greetingDelegate = Sepack_Say.SayChinese;
            s2.greetingDelegate += Sepack_Say.SayEnglish;
            s2.greetingDelegate("ssss");

            System.Console.WriteLine($"----------------{nameof(Sepack_Vsersion2)}:End");


            System.Console.WriteLine($"----------------{nameof(Sepack_Vsersion2)}:Begin");
            System.Console.WriteLine($"有返回值，多播委托");
            Sepack_Vsersion2 s3 = new Sepack_Vsersion2();
            s3.greetingDelegateStr = Sepack_Say.SayChinese_str;
            s3.greetingDelegateStr += Sepack_Say.SayEnglish_str;
            s3.greetingDelegateStr += Sepack_Say.SayJapanese_str;
            s3.greetingDelegateStr -= Sepack_Say.SayChinese_str;
            System.Console.WriteLine(s3.SaySomething("lsss", s3.greetingDelegateStr));// 多播委托方法有返回值 只得到最后的一个结果
            System.Console.WriteLine($"----------------{nameof(Sepack_Vsersion2)}:End");


            System.Console.ReadKey();
        }


    }
}

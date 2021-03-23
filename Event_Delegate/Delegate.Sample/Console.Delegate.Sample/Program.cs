using System;

namespace Console.Delegate.Sample
{
    //https://www.cnblogs.com/fire-dragon/p/5918790.html

    public enum Language { Chinese, English }
    public delegate void SayDelegate(string name);
    class Program
    {

        public static void SayChinese(string name)
        { 
            System.Console.WriteLine($"{name},早上好！");
        }

        public static void SayEnglish(string name)
        {
            System.Console.WriteLine($"{name},Good Morning！");
        }

        //public static void Say(string name,Language lang)
        //{
        //    switch (lang)
        //    {
        //        case Language.Chinese:
        //            SayChinese(name);
        //            break;
        //        case Language.English:
        //            SayEnglish(name);
        //            break;

        //            // todo 
        //            // 韩语，日语，法语......
        //            //example case Kra
        //            // 委托
        //    }

        //}

        public static void Say(string name, SayDelegate sayDelegate)
        {
            sayDelegate(name);
        }


        static void Main(string[] args)
        {
          var cc =  TestStaticClass.CurrentDateTime.GetDateTime();
          TestStaticClass.GetZero();


            /*使用方式1：*/
            //Say("张三", SayChinese);
            //Say("LI LEI", SayEnglish);


            /*使用方式2：
             * 
             * 总结：委托是一个类，它定义了方法的类型，使得可以将方法当做参数来传递，这种
             *      将方法动态的赋值给参数的做法可以避免在程序中大量的使用if-else/switch
             *      语句，同是使程序具有更好的扩展性
             
             */
            //SayDelegate sayChineseDelegate, sayEnglishDelegate;
            //sayChineseDelegate = SayChinese;
            //sayChineseDelegate = SayEnglish;
            //Say("张三", sayChineseDelegate);
            //Say("LI LEI", sayChineseDelegate);

            /*
             方式3：

             */

            SayDelegate sayDelegate;
            sayDelegate = SayChinese;
            sayDelegate += SayEnglish;

            System.Console.ReadKey();
        }
    }

    public static class TestStaticClass
    {
        private static readonly DateTime dateTime;

        static TestStaticClass()
        {
            dateTime = DateTime.Now;
        }

        public static DateTime CurrentDateTime { get { return dateTime; } private set { } }


        /// <summary>
        /// 嘻嘻嘻GetDateTime
        /// <example>
        /// <code>
        /// public int add(int a, b)
        /// {
        ///     return a + b;
        /// }
        /// </code>
        /// </example>
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTime GetDateTime(this DateTime dateTime)
        {
            return dateTime.AddDays(1);

        }


        /// <summary>
        /// 嘻嘻嘻
        /// <example>
        /// This sample shows how to call the <see cref="GetZero"/> method.
        /// <code>
        /// TestStaticClass.GetZero()
        /// </code>
        /// </example>
        /// </summary>
        /// <returns></returns>
        public static int GetZero()
        {
            return 0;
        }
    }
}

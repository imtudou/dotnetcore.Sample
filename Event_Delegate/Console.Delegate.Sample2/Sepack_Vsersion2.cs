using System;
using System.Collections.Generic;
using System.Text;

namespace Console.Delegate.Sample2
{
    /// <summary>
    /// 定义委托类型
    /// </summary>
    /// <param name="name"></param>
    public delegate void GreetingDelegate(string name);
    // 无参
    public delegate string GreetingDelegateStr(string name);

    public class Sepack_Vsersion2
    {
        public GreetingDelegate greetingDelegate;
        public GreetingDelegateStr greetingDelegateStr;


        public void SaySomething(string name)
        {
            if (greetingDelegate!= null)
            {
                greetingDelegate(name);
            }
            
        }

        public void SaySomething(string name, GreetingDelegate greeting)
        {
            greeting(name);
        }

        public string SaySomething(string name, GreetingDelegateStr greeting)
        {
           return greeting(name);
        }


        // dosomething ...

    }
}

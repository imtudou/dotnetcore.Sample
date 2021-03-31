using System;

namespace Console.Event.Sample2
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
                在本例中，事情发生的顺序应该是这样的：
                1. 警报器和显示器告诉热水器，它对它的温度比较感兴趣(注册)。
                2.热水器知道后保留对警报器和显示器的引用。
                3. 热水器进行烧水这一动作，当水温超过95度时，通过对警报器和显示器的引用，
                   自动调用警报器的MakeAlert()方法、显示器的ShowMsg()方法。

                观察者模式：Observer
             */

            Heater h = new Heater();
            h.BoilEvent += Alarm.ShowAlarm;  //注册静态方法
            h.BoilEvent += Display.ShowMsg;
            h.Boilwater(); //烧水，会自动调用注册过对象的方法


            System.Console.ReadKey();
        }
    }
}

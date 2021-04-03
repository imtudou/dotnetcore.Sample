using System;

namespace Console.Event.Sample3
{
    class Program
    {
        static void Main(string[] args)
        {
            Heater h = new Heater();
            h.Boil += Alarm.ShowAlarm;  //注册静态方法
            h.Boil += Display.ShowMsg;
            h.Boilwater(); //烧水，会自动调用注册过对象的方法
            System.Console.ReadKey();
        }
    }
}

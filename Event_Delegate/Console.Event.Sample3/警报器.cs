using System;
using System.Collections.Generic;
using System.Text;

namespace Console.Event.Sample3
{
    public class Alarm
    {
        /// <summary>
        /// 报警器
        /// </summary>
        public static void ShowAlarm(object sender, BoilEventArgs e)
        {
            Heater heater = (Heater)sender;
            System.Console.WriteLine("报警器：型号：{0}", heater.type);
            System.Console.WriteLine("报警器：产地：{0}", heater.area);
            System.Console.WriteLine("报警器：滴滴滴~~~~ 警报,警报！！！ 水温{0}度", e.temperature);
        }
    }
}

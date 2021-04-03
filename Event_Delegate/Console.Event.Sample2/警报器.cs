using System;
using System.Collections.Generic;
using System.Text;

namespace Console.Event.Sample2
{
    public class Alarm
    {
        /// <summary>
        /// 报警器
        /// </summary>
        public static void ShowAlarm(int para)
        {
            System.Console.WriteLine("报警器：滴滴滴~~~~ 警报,警报！！！ 水温{0}度", para);
        }
    }
}

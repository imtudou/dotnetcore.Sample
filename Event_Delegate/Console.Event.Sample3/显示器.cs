using System;
using System.Collections.Generic;
using System.Text;

namespace Console.Event.Sample3
{
    public class Display
    {
        /// <summary>
        /// 显示器
        /// </summary>
        public static void ShowMsg(Object sender, BoilEventArgs e)
        {
            Heater heater = (Heater)sender;
            System.Console.WriteLine("显示器：型号：{0}", heater.type);
            System.Console.WriteLine("显示器：产地：{0}", heater.area);
            System.Console.WriteLine("显示器：水已开，当前温度{0}", e.temperature);
        }
    }
}

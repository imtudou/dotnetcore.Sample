using System;
using System.Collections.Generic;
using System.Text;

namespace Console.Event.Sample2
{
    public class Display
    {
        /// <summary>
        /// 显示器
        /// </summary>
        public static void ShowMsg(int para)
        {
            System.Console.WriteLine("显示器：水已开，当前温度{0}", para);
        }
    }
}

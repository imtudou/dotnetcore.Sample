using System;
using System.Collections.Generic;
using System.Text;

namespace Console.Event.Sample
{
    /// <summary>
    /// 烧水
    /// 当水温超过95度的时候：
    /// 1、扬声器会开始发出语音，告诉你水的温度；
    /// 2、液晶屏也会改变水温的显示，来提示水已经快烧开了。
    /// </summary>
    public class WaterHeater
    {
        /// <summary>
        /// 水温
        /// </summary>
        private int temperature;

        /// <summary>
        /// 烧水
        /// </summary>
        public void Boilwater()
        {
            for (int i = 0; i < 100; i++)
            {
                temperature = i;
                System.Console.WriteLine("Boilwater：正在烧水 水温{0}度", temperature);
                if (temperature == 90)
                {           
                    Alarm(temperature);
                    ShowMsg(temperature);
                    return;
                }
            }
        
        }

        /// <summary>
        /// 报警器
        /// </summary>
        private void Alarm(int para)
        {
            System.Console.WriteLine("Alarm：滴滴滴~~~~ 警报,警报！！！ 水温{0}度", para);
        }


        /// <summary>
        /// 显示器
        /// </summary>
        private void ShowMsg(int para)
        {
            System.Console.WriteLine("ShowMsg：水已开，当前温度{0}", para);
        }


    }
}

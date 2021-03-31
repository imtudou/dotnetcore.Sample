using System;
using System.Collections.Generic;
using System.Text;

namespace Console.Event.Sample2
{
    /// <summary>
    /// 声明委托类型
    /// </summary>
    /// <param name="para"></param>
    public delegate void BoilDelegate(int para);


    /// <summary>
    /// 烧水
    /// 当水温超过95度的时候：
    /// 1、扬声器会开始发出语音，告诉你水的温度；
    /// 2、液晶屏也会改变水温的显示，来提示水已经快烧开了。
    /// </summary>
    public class Heater
    {
        /// <summary>
        /// 声明事件
        /// </summary>
        public event BoilDelegate BoilEvent;


        /// <summary>
        /// 水温
        /// </summary>
        private int temperature { get; set; }

        /// <summary>
        /// 烧水
        /// </summary>
        public void Boilwater()
        {
            System.Console.WriteLine("烧水中......");
            for (int i = 0; i < 100; i++)
            {
                temperature = i;
                if (temperature == 90)
                {
                    if (BoilEvent != null)
                    {
                        BoilEvent(temperature);
                    }                                    
                    return;
                }
                System.Console.WriteLine($"当前温度： {temperature}");
            }        
        }

    }
}

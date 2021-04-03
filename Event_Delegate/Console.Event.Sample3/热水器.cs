using System;
using System.Collections.Generic;
using System.Text;

namespace Console.Event.Sample3
{
    /// <summary>
    /// 声明委托类型
    /// 
    /// 1.委托类型的名称都应该以EventHandler结束。
    /// 2.委托的原型定义：有一个void返回值，并接受两个输入参数：一个Object 类型，一个 EventArgs类型(或继承自EventArgs)。
    /// 3.事件的命名为 委托去掉 EventHandler之后剩余的部分。
    /// 4.继承自EventArgs的类型应该以EventArgs结尾。
    /// </summary>
    /// <param name="para"></param>
    public delegate void BoilEventHandler(Object sender, BoilEventArgs e);


    /// <summary>
    /// 烧水
    /// </summary>
    public class Heater
    {
        /// <summary>
        /// 水温
        /// </summary>
        private int temperature { get; set; }
        public string type = "RealFire 001";       // 添加型号作为演示
        public string area = "China Xian";         // 添加产地作为演示

        /// <summary>
        /// 声明事件（）
        /// </summary>
        public event BoilEventHandler Boil;

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
                    //建立BoiledEventArgs 对象。
                    BoilEventArgs e = new BoilEventArgs(temperature);   
                    if (Boil != null)
                    {
                        Boil(this, e);
                    }
                    return;
                }
                System.Console.WriteLine($"当前温度： {temperature}");
            }        
        }

    }
}

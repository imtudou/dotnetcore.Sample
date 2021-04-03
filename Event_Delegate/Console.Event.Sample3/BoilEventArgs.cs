using System;
using System.Collections.Generic;
using System.Text;

namespace Console.Event.Sample3
{
    // 定义BoiledEventArgs类，传递给Observer所感兴趣的信息
    public class BoilEventArgs : EventArgs
    {
        public int temperature;

        public BoilEventArgs(int t)
        {
            temperature = t;
        }
    }
}

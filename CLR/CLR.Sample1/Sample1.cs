using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CLR.Sample1
{
    public class Sample1
    {
        //以下示例演示了如何让一个线程池线程
        //以异步的方式调用一个方法

        public static void ComputeBoundOp(object state)
        {
            // 该方法由一个线程池线程执行
            Console.WriteLine($"In ComputeBoundOp:{state}");
            Thread.Sleep(5000);

            // 这个方法返回后 线程回到线程池中 等待另一个任务

        }
    }
}

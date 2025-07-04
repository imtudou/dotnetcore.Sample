using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CLR.Sample1
{
    public class Sample2
    {

        // 执行上下文

        public static void ExecutionContextMethod()
        {
            // 执行上下文
            // 下例展示了向CLR 的线程池队列添加一个工作项的时候了，如何
            // 通过阻止执行上下文的流动来音响线程逻辑调用上下文的数据


            // 将一些数据放到Main 线程的逻辑调用上下文中
            CallContext.LogicalSetData("Name", "zhangsan");


            // 初始化要有一个线程池线程做的一些工作
            // 线程池线程能访问逻辑调用上下文数据
            ThreadPool.QueueUserWorkItem(state => Console.WriteLine($"name = {CallContext.LogicalGetData("Name")}"));


            // 阻止Main 线程的执行上下文的流动
            ExecutionContext.SuppressFlow();


            ThreadPool.QueueUserWorkItem(state => Console.WriteLine($"name = {CallContext.LogicalGetData("Name")}"));


            // 回复Main线程的执行上下文的流动
            // 一面将来使用更多的线程池线程
            ExecutionContext.RestoreFlow();
        }
    }
}

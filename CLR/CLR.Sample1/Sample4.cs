using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CLR.Sample1
{
    public class Sample4
    {

        // Task
        // 任务
        // 1.简单使用
        // 2.等待任务完成并获取结果
        // 3.取消任务
        // 4.任务完成后自动启动新的任务
        // 5.任务启动子任务
        // 6.任务工厂
        // 7.任务调度器 TaskScheduler


        public static void GetTaskResultWaitForTaskComplete()
        {
            var task = new Task<int>(s=>Sum((int)s),1000);
            task.Start();
            task.Wait();
            Console.WriteLine($"GetTaskResultWaitForTaskComplete:{task.Result}");
        }

        public static void CancelTask()
        {
            CancellationTokenSource cts = new CancellationTokenSource();
            var task = Task.Run(()=> Sum(1000,cts.Token));

            // 在之后的某个时间取消CancellationTokenSource 取消Task
            cts.Cancel(); // 异步请求 Task 可能已经完成了

            try
            {
                // 如果任务取消 result 会抛出ArgumentException
                Console.WriteLine($"CancelTask Result:{task.Result}");
            }
            catch (AggregateException ex)
            {
                // 将任何 OperationCancenedException 兑现都视为已处理
                // 其他任何异常都造成抛出一个新的 AggregateException
                // 其中只包含未处理的异常

                ex.Handle(e => e is OperationCanceledException);
            }
        
        }


        public static void TaskCompleteAutoStartNewTask()
        {
            //创建并启动一个Task 继续另一个任务
            var t = Task.Run(()=> Sum(10000, CancellationToken.None));

            //ContinueWith 返回一个Task 但一般都不需要在使用该对象(任务完成后启动新任务)
            // TaskContinuationOptions.OnlyOnRanToCompletion 标志 指定新任务只有在第一个任务顺利完成（中途没有取消，也没有抛出未处理异常）时才执行
            Task cwt = t.ContinueWith(s => Console.WriteLine("The sum is :" + t.Result), TaskContinuationOptions.OnlyOnRanToCompletion);

            //TaskContinuationOptions.OnlyOnFaulted 标志指定新任务只有在第一个任务排除未处理的异常时才执行
            t.ContinueWith(s => Console.WriteLine("Sun throw:" + t.Exception.InnerException), TaskContinuationOptions.OnlyOnFaulted);

            //TaskContinuationOptions.OnlyOnCanceled 标志指定新任务只有在第一个任务被取消时才执行 
            t.ContinueWith(s => Console.WriteLine("Sun Canceled:"), TaskContinuationOptions.OnlyOnCanceled);

            if (t.IsCompleted)
            {
                   // dosomething
            }

            if (t.AsyncState is null)
            {

            }

           

        }


        public static void TaskCanStartChildrenTask()
        {
            Task<int[]> parent = new Task<int[]>(() =>
            {
                var results = new int[3]; // 创建一个数组来存储结果

                new Task(() => results[0] = Sum(1000), TaskCreationOptions.AttachedToParent).Start();
                new Task(() => results[1] = Sum(2000), TaskCreationOptions.AttachedToParent).Start();
                new Task(() => results[2] = Sum(3000), TaskCreationOptions.AttachedToParent).Start();

                // 返回对数组的引用（即使数组还没有初始化）
                return results;
            });

            // 父任务及其子任务运行完成后 用一个延续任务显示结果
            var cwt = parent.ContinueWith(s => Array.ForEach(parent.Result, Console.WriteLine));

            // 启动服务人员并与启动他的子任务
            parent.Start();

        }

        public static void TaskFactory()
        {

            var parent = new Task(() =>
            {
                var cts = new CancellationTokenSource();


                var tf = new TaskFactory<int>(cts.Token, TaskCreationOptions.AttachedToParent, TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);

                // 这个任务创建并启动3个任务
                var childrenTasks = new[]
                {
                    tf.StartNew(() => Sum(10000,cts.Token)),
                    tf.StartNew(() => Sum(2000,cts.Token)),
                    tf.StartNew(() => Sum(int.MaxValue,cts.Token)),// 太大 抛出 overflowException

                };

                // 任何子任务抛出异常 就取消其余子任务
                for (int i = 0; i < childrenTasks.Length; i++)
                {
                    childrenTasks[i].ContinueWith(s => cts.Cancel(), TaskContinuationOptions.OnlyOnFaulted);
                }

                // 所有子任务完成后 从未出错/未取消的任务获取返回的最大值
                // 然后将最大值传给另一个任务来显示最大结果
                tf.ContinueWhenAll(childrenTasks, completedTaks => completedTaks.Where(s => !s.IsFaulted && !s.IsCanceled).Max(s => s.Result), CancellationToken.None)
                .ContinueWith(s => Console.WriteLine("Max Sum is :" + s.Result), TaskContinuationOptions.ExecuteSynchronously);
            });

            // 子任务完成后 也显示任何未处理的异常
            parent.ContinueWith(s =>
            {



            },TaskContinuationOptions.OnlyOnFaulted);

            // 启动父任务时启动子任务
            parent.Start();
        }
    





        private static int Sum(int n)
        {
            int sum = 0;
            for (; n > 0; n--)
                checked { sum += n; } // 如果n太大会抛出 OverflowException
            return sum;
        }

        private static int Sum(int n,CancellationToken cancellationToken)
        {
            int sum = 0;
            for (; n > 0; n--)
                // 在取消标志引用的 CancellationTokenSource 上调用Cancel
                cancellationToken.ThrowIfCancellationRequested();// 抛出 OperationCancenedException
                checked { sum += n; } // 如果n太大会抛出 OverflowException
            return sum;
        }
    }

}

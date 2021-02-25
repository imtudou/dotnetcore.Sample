using ConsoleApp.Quartz.NET.Jobs;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp.Quartz.NET
{
    class Program
    {
        #region 构造函数及字段
        private static readonly string triggerName = "triggerName";
        private static readonly string jobName = "jobName";
        private static readonly string groupName = "groupName";
        private static readonly string crontimes = $"{ConfigurationManager.AppSettings["crons"].ToString()}";
        private static readonly int seconds = Convert.ToInt32(ConfigurationManager.AppSettings["seconds"]);

        private readonly ISchedulerFactory _schedulerFactory;
        private IScheduler _scheduler;
        public Program()
        {
            _schedulerFactory = new StdSchedulerFactory(); //实例化调度器工厂
        }
        #endregion


        static void Main(string[] args)
        {
            try
            {
                DLogs.WriteOPLog("江苏中社定时任务", "读取付款指令状态", "定时任务启动......");
                Console.WriteLine("控制台应用程序启动......" + DateTime.Now);
                Console.WriteLine("实例化调度器工厂开始......" + DateTime.Now);

                new Program().InitScheduler();

                Console.WriteLine("实例化调度器工厂结束......" + DateTime.Now);
                Console.WriteLine("控制台应用程序启动成功......" + DateTime.Now);
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                DLogs.WriteOPLog("江苏中社定时任务", "读取付款指令状态", "定时任务启动异常："+ex.Message);

                Console.WriteLine("控制台应用程序启动异常......" + DateTime.Now);
                Console.WriteLine("异常原因：......" + DateTime.Now);
                Console.WriteLine(ex.Message);
            }
        }



        #region 实例化调度器工厂
        /// <summary>
        /// 实例化调度器工厂
        /// </summary>
        public async void InitScheduler()
        {

            //1、通过调度工厂获得调度器 
            _scheduler = await _schedulerFactory.GetScheduler();//实例化调度器
            //2、开启调度器
            await _scheduler.Start();
            //3、创建一个触发器



            ITrigger trigger;
            if (string.IsNullOrEmpty(crontimes))
            {

                //trigger = TriggerBuilder.Create()
                //            .WithIdentity(triggerName, groupName)
                //            .WithSimpleSchedule(x => x.WithIntervalInSeconds(5*60)//每两秒执行一次
                //            .WithRepeatCount(20)//执行20次
                //                                //.RepeatForever()//不间断重复执行
                //            )
                //            .Build();

                trigger = TriggerBuilder.Create()
                            .WithIdentity(triggerName, groupName)
                            .WithSimpleSchedule(x => x.WithIntervalInSeconds(seconds)//每两秒执行一次
                            .RepeatForever()//不间断重复执行
                            )
                            .Build();
            }
            else
            {
                //使用 cron表达式 执行定时任务
                trigger = TriggerBuilder.Create()
                                              .WithIdentity(triggerName, groupName)
                                              .WithCronSchedule(crontimes)
                                              .Build();
            }








            //4、创建任务
            var jobDetail = JobBuilder.Create<MyJob>()
                            .WithIdentity(jobName, groupName)
                            .Build();



            //5、将触发器和任务器绑定到调度器中
            await _scheduler.ScheduleJob(jobDetail, trigger);

        }
        #endregion
    }
}

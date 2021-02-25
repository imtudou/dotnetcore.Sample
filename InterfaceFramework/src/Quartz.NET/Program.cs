using System;
using System.IO;
using System.Threading.Tasks;
using Quartz;
using Quartz.Impl;


namespace Quartz.NET
{
    class Program
    {

        #region 构造函数及字段
        private static readonly string triggerName = "triggerName";
        private static readonly string jobName = "jobName";
        private static readonly string groupName = "groupName";

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
                Console.WriteLine("");
                Program program = new Program();
                program.InitScheduler();
                Console.WriteLine("Hello World!");
                Console.ReadKey();
            }
            catch (Exception ex)
            {

                throw;
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
            var trigger = TriggerBuilder.Create()
                            .WithIdentity(triggerName, groupName)
                            .WithSimpleSchedule(x => x.WithIntervalInSeconds(2)//每两秒执行一次
                            .WithRepeatCount(20)//执行20次
                            //.RepeatForever()//不间断重复执行
                            )
                            .Build();


            //4、创建任务
            var jobDetail = JobBuilder.Create<MyJob>()
                            .WithIdentity(jobName, groupName)
                            .Build();



            //5、将触发器和任务器绑定到调度器中
            await _scheduler.ScheduleJob(jobDetail, trigger);

        }
        #endregion


        #region 清除任务和触发器
        /// <summary>
        /// 清除任务和触发器
        /// </summary>
        private void ClearJobAndTrigger()
        {
            TriggerKey triggerKey = new TriggerKey(triggerName, groupName);
            JobKey jobKey = new JobKey(jobName, groupName);
            _scheduler.PauseTrigger(triggerKey);//停止触发器
            _scheduler.UnscheduleJob(triggerKey);//移除触发器
            _scheduler.DeleteJob(jobKey);//删除任务
        } 
        #endregion



    }
}

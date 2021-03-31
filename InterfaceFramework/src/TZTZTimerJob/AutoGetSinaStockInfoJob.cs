
using MyFramework.Core.Log;
using Quartz;
using System;
using System.Threading.Tasks;

namespace TZTZTimerJob
{
    class AutoGetSinaStockInfoJob: IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {

            //Job要执行的逻辑代码 
            string guid = Guid.NewGuid().ToString();
            NLogHelper.Info("-----------------------定时任务AutoGetSinaStockInfoJob（）开始-----------------------:" + DateTime.Now);

            //TZTZ_SinaStockInfo.AutoGetStockInfo(guid);

            NLogHelper.Info("-----------------------定时任务AutoGetSinaStockInfoJob（）结束-----------------------:" + DateTime.Now);

        }

        
    }
}

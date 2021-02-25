using System;
using System.IO;
using System.Threading.Tasks;
using System.Text;
using System.Collections.Generic;
using Quartz;

namespace WindowsFormsApp.Quartz.Net.Jobs
{
    public class MyJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            string path = @"D:\\test.txt";
            string value = DateTime.Now.ToString() + "\r\n";
            await Task.Run(() =>
            {
                 File.AppendAllText(path, value, Encoding.UTF8);
            });
           
   
        }
    }
}
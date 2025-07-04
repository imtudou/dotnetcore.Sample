using Coldairarrow.Util;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDHelper
{
    class Program
    {
        static void Main(string[] args)
        {
            new IdHelperBootstrapper()
                //设置WorkerId
                .SetWorkderId(1)
                .Boot();

            Console.WriteLine($"WorkerId:{IdHelper.WorkerId},Id:{IdHelper.GetId()}");

            //Stopwatch watch = new Stopwatch();
            //watch.Start();
            //List<Task> tasks = new List<Task>();
            //BlockingCollection<string> ids = new BlockingCollection<string>();
            //for (int i = 0; i < 4; i++)
            //{
            //    tasks.Add(Task.Run(() =>
            //    {
            //        for (int j = 0; j < 1000000; j++)
            //        {
            //            ids.Add(IdHelper.GetId());
            //        }
            //    }));
            //}
            //Task.WaitAll(tasks.ToArray());
            //watch.Stop();
            //Console.WriteLine($"耗时:{watch.ElapsedMilliseconds}ms,是否有重复:{ids.Count != ids.Distinct().Count()}");

            Console.ReadLine();
        }
    }
}

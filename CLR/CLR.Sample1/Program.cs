using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CLR.Sample1
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("Main Thread Start .....");
            ThreadPool.QueueUserWorkItem(Sample1.ComputeBoundOp, 5);
            Console.WriteLine("Child Thread DoSomething .....");
            Thread.Sleep(1000);
            Console.WriteLine("Main Thread End .....");

            /*
              output

                    Main Thread Start .....
                    Child Thread DoSomething .....
                    In ComputeBoundOp:5
                    Main Thread End .....
                
                or

             
              
             */




            // Sample2.ExecutionContextMethod(); 
            // output
            /*
                  name = 
                  name = zhangsan 
             */




            Console.ReadKey();

        }
    }
}

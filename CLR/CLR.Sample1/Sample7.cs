using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace CLR.Sample1
{

    // 编译器如何将异步函数转换成状态机
    public class Sample7
    {
        public static Task<TResult> Log<TResult>()
        {
            return default;
        }


        private static async Task<string> MyMethodAsync(int argument)
        {
            int local = argument;
            try
            {
                Type1 result1 = await Type1Async();
                for (int i = 0; i < 3; i++)
                {
                    Type2 result2 = await Type2Async();
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return "Success";
        
        }


        private static async Task<Type1> Type1Async()
        {
            // 异步执行一些操作最后返回一个Type1 对象
            await Task.Delay(2000);
            return new Type1();
        }

        private static async Task<Type2> Type2Async()
        {
            // 异步执行一些操作最后返回一个Type2 对象
            await Task.Delay(2000);
            return await Task.FromResult(new Type2());
        }



    }

   

    internal class Type1 { }
    internal class Type2 { }

}

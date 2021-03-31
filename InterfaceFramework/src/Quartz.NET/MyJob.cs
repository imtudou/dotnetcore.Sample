using System;
using System.IO;
using System.Threading.Tasks;
using System.Text;
using System.Collections.Generic;

namespace Quartz.NET
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



    public class ccccc
    {


        static void Main(string[] args)
        {
            string str = "Hello,C#!!!";
            //调用。
            string result = StringToHex16String(str);
            Console.WriteLine(string.Format("将普通字符串:{0}转换成16进制字符串是:{1}", str, result));
            Console.ReadKey();
        }
        /// <summary>
        /// 此方法用于将普通字符串转换成16进制的字符串。
        /// </summary>
        /// <param name="_str">要转换的字符串。</param>
        /// <returns></returns>
        public static string StringToHex16String(string _str)
        {
            //将字符串转换成字节数组。
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(_str);
            //定义一个string类型的变量，用于存储转换后的值。
            string result = string.Empty;
            for (int i = 0; i < buffer.Length; i++)
            {
                //将每一个字节数组转换成16进制的字符串，以空格相隔开。
                result += Convert.ToString(buffer[i], 16) + " ";
            }
            return result;
        }
    }
}
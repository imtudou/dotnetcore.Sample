using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ConsoleApp
{
    class Program
    {
        public static void Main(string[] args)
        {
            /*
             yuanyi：2019年8月3日16:30:12
             此示例是为我做接口平台的例子：
             前端界面通过配置入参对应到系统中的表中最终存储到数据库中,即动态配置接口,供第三方调用
             */
            //D:\Y\Repository\repos\Personal.Sample\反射\ConsoleApp\bin\Debug\netcoreapp2.1\
            var path = AppDomain.CurrentDomain.BaseDirectory;
            string cc =RunMethod();


            //1.读取appsettings.json文件
            string paths = Directory.GetCurrentDirectory().Replace("bin\\Debug", "appsettings.json");
            string contents = File.ReadAllText(paths);
            dynamic dy = JsonConvert.DeserializeObject<dynamic>(contents);
            string Date = dy.Date;

            //2.将当前值写入appsettings.json
            string EDate = DateTime.Now.ToString("yyyy/MM/dd");
            dy.Date = EDate;
            contents = string.Empty;
            contents = JsonConvert.SerializeObject(dy);
            File.WriteAllText(path, contents);



            Console.WriteLine(cc);
            Console.ReadKey();
        }



        public static string RunMethod()
        {
            string Result = string.Empty;
            string path = Directory.GetCurrentDirectory();//获取应用程序的当期工作目录
            string namespaceName = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Namespace;//获取命名空间
            path = path.Substring(0, path.LastIndexOf(namespaceName) + namespaceName.Length) + "\\assembly";//获取需要加载的.cs 文件
            var files = Directory.GetFiles(path,"*.cs");//获取文件夹下所有的文件
            string rootdir = AppContext.BaseDirectory;

            foreach (var item in files) 
            {
                string fileName = Path.GetFileNameWithoutExtension(item);
                Type t = Type.GetType("ConsoleApp.assembly." + fileName);//反射获取类型
                object obj = Activator.CreateInstance(t);//实例化对象
                MethodInfo methodInfo = t.GetMethod("GetOrderInfo");//获取方法
                Result = methodInfo.Invoke(obj, null).ToString();//调用方法(无参)  

                MethodInfo methodInfos = t.GetMethod("GetOrderInfoByID");//获取方法
                Result += methodInfos.Invoke(obj,new object[] {"1"}).ToString();//调用方法(有参数) 

            }

            return Result;
        }

    }
}

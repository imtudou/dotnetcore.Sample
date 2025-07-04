using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

using System;
using System.Collections.Generic;
using System.IO;

namespace _4.ConfigurationDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
             1.定义本地数据源
             */
            #region 常规读取
            // 
            IConfigurationBuilder builder = new ConfigurationBuilder();

            // 定义本地数据源
            builder.AddInMemoryCollection(new Dictionary<string, string>()
            {
                { "id","1"},
                { "name","张三"},
                { "userInfo:age","18" },
                { "userInfo:email","zhangsan@163.com"},
                { "userinfo:address:province","湖北"},
                { "userinfo:address:city","武汉"},
            });

            IConfigurationRoot configurationRoot = builder.Build();

            string id = configurationRoot["id"];
            string name = configurationRoot["name"];

            IConfigurationSection section = configurationRoot.GetSection("userInfo");
            string age = section["age"];
            string email = section["email"];

            IConfigurationSection section1 = section.GetSection("address");
            string city = section1["city"];
            #endregion

            #region 命令替换
            /*
             Microsoft.Extensions.Configuration.CommandLine
             */
            var mapper = new Dictionary<string, string> { { "--k1", "CommandLineKey1" } };
            object p = builder.AddCommandLine(args, mapper);
            #endregion           


            /*
                2.json文件读取
                添加组件包：Microsoft.Extensions.Configuration.Json
             */


            IConfigurationBuilder builderJson = new ConfigurationBuilder();
            string path = Directory.GetCurrentDirectory();
            builderJson
                .SetBasePath(path)
                .AddJsonFile("appsettings.json");

            IConfigurationRoot configurationRootJson = builderJson.Build();
            string id1 = configurationRootJson["id"];
            string name1 = configurationRootJson["name"];

            IConfigurationSection sectionJson = configurationRootJson.GetSection("userInfo");
            string age1 = sectionJson["age"];
            string email1 = sectionJson["email"];

            IConfigurationSection sectionJson1 = sectionJson.GetSection("address");
            string city1 = sectionJson1["city"];

            #region 使用强类型对象配置数据
            /*
                在以下代码中，不会读取在应用启动后对 JSON 配置文件所做的更改。 
                若要读取在应用启动后的更改，请使用 IOptionsSnapshot。
             */



            #region 本地数据源
            /*
                 * 方式一：
                 */

            UserInfoCofig userInfoCofig = new UserInfoCofig();
            configurationRoot.Bind(userInfoCofig);
            id = userInfoCofig.id.ToString();//最终ID取值还是从定义本地数据源中取值的

            /*
             方式二：使用 ConfigurationBinder.Get<T>绑定返回指定类型
             */
            var userinfo = configurationRoot.GetSection("userInfo").Get<UserInfo>();
            #endregion


            #region Json文件
            /*
                 * 方式一：
                 */

            UserInfoCofig userInfoCofigJson = new UserInfoCofig();
            configurationRootJson.Bind(userInfoCofigJson);
            id = userInfoCofigJson.id.ToString();

            /*
             方式二：使用 ConfigurationBinder.Get<T>绑定返回指定类型
             */
            var userinfo1 = configurationRootJson.GetSection("userInfo").Get<UserInfo>();
            #endregion



            #endregion



            Console.WriteLine($"id:{id}");
            Console.WriteLine($"name:{name}");
            Console.WriteLine($"age:{age}");
            Console.WriteLine($"email:{email}");
            Console.WriteLine($"city:{city}");

            Console.WriteLine($"CommandLineKey1:{configurationRoot["CommandLineKey1"]}");
            Console.WriteLine($"CommandLineKey2:{configurationRoot["CommandLineKey2"]}");
            Console.WriteLine($"CommandLineKey3:{configurationRoot["CommandLineKey3"]}");

            Console.ReadKey();
        }
    }
}

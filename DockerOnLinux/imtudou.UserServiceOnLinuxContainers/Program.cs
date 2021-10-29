using AgileConfig.Client;

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace imtudou.UserServiceOnLinuxContainers
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((context, config) =>
                {
                    //根据环境变量读取appsettings.{env}.json配置信息

                    var jsonfile = $"appsettings.{context.HostingEnvironment.EnvironmentName}.json";
                    var jsonfilepath = Path.Combine(context.HostingEnvironment.ContentRootPath, jsonfile);
                    jsonfile = File.Exists(jsonfilepath) ? jsonfile : $"appsettings.json";
                    var configClient = new ConfigClient(jsonfile);
                    config.AddAgileConfig(configClient, arg => Console.WriteLine($"config changed , action:{arg.Action} key:{arg.Key}"));
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}

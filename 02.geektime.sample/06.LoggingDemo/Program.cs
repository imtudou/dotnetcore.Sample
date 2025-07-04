using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using System;
using System.IO;

namespace _06.LoggingDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            #region 1.构造配置对象
            // 1.构造配置对象
            IConfigurationBuilder builder = new ConfigurationBuilder();
            builder.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");
            IConfigurationRoot configuration = builder.Build();
            #endregion


            #region 2.构造容器对象并将所需要的服务注册到容器中
            // 2.构造容器对象
            IServiceCollection services = new ServiceCollection();

            //用工厂模式将配置对象注册到容器管理 容器帮我们管理对象的生命周期
            services.AddSingleton<IConfiguration>(s => configuration);

            // 3. 添加日志配置并注册到容器中
            services.AddLogging(s =>
            {
                s.AddConfiguration(configuration.GetSection("Logging"));
                s.AddConsole();// 输出到 Console
            });

            services.AddTransient<UserInfoService>();
            #endregion


            #region 3.获取容器中的对象并输出
            // 3.获取容器中的对象并输出
            IServiceProvider serviceProvider = services.BuildServiceProvider();

            // 获取 UserInfoService
            var userinfo = serviceProvider.GetService<UserInfoService>();
            var user = userinfo.GetUserInfo();

            var logging = serviceProvider.GetService<ILoggerFactory>();
            ILogger consolelog = logging.CreateLogger("consolelog");

            consolelog.LogTrace(nameof(LogLevel.Trace));
            consolelog.LogDebug(nameof(LogLevel.Debug));
            consolelog.LogInformation(nameof(LogLevel.Information));
            consolelog.LogWarning(nameof(LogLevel.Warning));
            consolelog.LogError(new Exception(nameof(LogLevel.Error)), nameof(LogLevel.Error));
            consolelog.LogCritical(new Exception(nameof(LogLevel.Critical)), nameof(LogLevel.Critical));

            #endregion
            Console.ReadKey();
        }
    }
}

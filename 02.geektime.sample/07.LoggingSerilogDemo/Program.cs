using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;
using System.IO;

namespace _07.LoggingSerilogDemo
{
    public class Program
    {
        public static IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .Build();

        public static void Main(string[] args)
        {
            // สนำร Serilog
            Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(configuration)
                .MinimumLevel.Debug()
                .Enrich.FromLogContext()
                .WriteTo.Console(new RenderedCompactJsonFormatter())
                .WriteTo.File(formatter: new CompactJsonFormatter(), "logs\\test.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            Log.Information("Starting web host");
            CreateHostBuilder(args).Build().Run();
            Log.Information("Ending web host");
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
            .UseSerilog(dispose: true);
    }
}

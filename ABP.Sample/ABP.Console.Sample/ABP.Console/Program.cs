using System;

using Microsoft.Extensions.DependencyInjection;
using Volo.Abp;

namespace ABP.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var application = AbpApplicationFactory.Create<ConsoleModule>())
            {
                application.Initialize();

                var helloService = application.ServiceProvider.GetService<HelloAbpService>();
                helloService.SayHello();

                application.Shutdown();
            }

            System.Console.WriteLine();
        }
    }
}

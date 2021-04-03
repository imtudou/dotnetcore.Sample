using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

using Volo.Abp.Modularity;

namespace ABP.Console
{
    public class ConsoleModule:AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddTransient<HelloAbpService>();
            base.ConfigureServices(context);
        }
    }
}

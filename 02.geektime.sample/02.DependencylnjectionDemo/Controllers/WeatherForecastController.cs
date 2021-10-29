using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

using _02.DependencylnjectionDemo.Service;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace _02.DependencylnjectionDemo.Controllers
{
    [ApiController]
    [Route("[action]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet]
        public bool GetService([FromServices] IMySingletonService singletonService,
            [FromServices] IMySingletonService singletonService1,
            [FromServices] IMyScopedService scopedService,
            [FromServices] IMyScopedService scopedService1,
            [FromServices] IMyTransientService transientService,
            [FromServices] IMyTransientService transientService1
            )
        {
            Console.WriteLine("请求开始GetService();");

            Console.WriteLine($"{nameof(IMySingletonService)}.singletonService:{singletonService.GetHashCode()}");
            Console.WriteLine($"{nameof(IMySingletonService)}.singletonService1:{singletonService1.GetHashCode()}");


            Console.WriteLine($"{nameof(IMyScopedService)}.scopedService:{scopedService.GetHashCode()}");
            Console.WriteLine($"{nameof(IMyScopedService)}.scopedService1:{scopedService1.GetHashCode()}");


            Console.WriteLine($"{nameof(IMyTransientService)}.transientService:{transientService.GetHashCode()}");
            Console.WriteLine($"{nameof(IMyTransientService)}.transientService1:{transientService1.GetHashCode()}");

            Console.WriteLine("请求结束");

            return true;
        }




        [HttpGet]
        public void GetService2([FromServices] IMySingletonService singletonService,
            [FromServices] IMyScopedService scopedService,
            [FromServices] IMyTransientService transientService
            )
        {

            Console.WriteLine("请求开始GetService2()");

            Console.WriteLine($"{nameof(IMySingletonService)}.singletonService:{singletonService.GetHashCode()}");
            Console.WriteLine($"{nameof(IMyScopedService)}.scopedService:{scopedService.GetHashCode()}");
            Console.WriteLine($"{nameof(IMyTransientService)}.transientService:{transientService.GetHashCode()}");

            //GetTransientService();

            Console.WriteLine("请求结束");

        }
        [HttpGet]
        public bool GetServiceList([FromServices] IEnumerable<IOrderService> services)
        {
            foreach (var item in services)
            {
                Console.WriteLine($"获取到服务的实例：{item.ToString()}{ item.GetHashCode()}");
            }

            return true;
        }

        [HttpGet]
        public void GetTransientService()
        {
            IServiceCollection service = new ServiceCollection();
            service.AddTransient<IMyTransientService>();
            IServiceProvider provider = service.BuildServiceProvider();
            var transientService = provider.GetService<IMyTransientService>();
            Console.WriteLine($"{nameof(IMyTransientService)}.transientService:{transientService.GetHashCode()}");

            IList<int> ids = new List<int>() { };
            if (ids?.Count >0)
            {

            }
            if ((ids?.Count ?? 0 )> 0)
            {

            }
        }
    }

}

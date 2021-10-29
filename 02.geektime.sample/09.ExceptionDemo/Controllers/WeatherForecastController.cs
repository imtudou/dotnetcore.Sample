using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using _09.ExceptionDemo.Exceptions;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace _09.ExceptionDemo.Controllers
{
    [ApiController]
    [Route("[controller]")]
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
            //int a = 1, b = 0;
            //int i = a / b;
            var rng = new Random();           
            var result = Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
            //throw new MyException("hahahha错误");
            return result;
        }
    }
}

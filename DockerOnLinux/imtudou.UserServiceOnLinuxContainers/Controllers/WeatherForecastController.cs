using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace imtudou.UserServiceOnLinuxContainers.Controllers
{
    [ApiController]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        public readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public WeatherForecastController(ILogger<WeatherForecastController> logger
            , IConfiguration configuration
            , HttpClient httpClient)
        {
            _logger = logger;
            _configuration = configuration;
            _httpClient = httpClient;
        }

        [HttpGet,Route("api/get")]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                TenantID = _configuration.GetSection("TenantID").Value ,
                ConnStr = _configuration.GetSection("conn").Value,
                Summary = _configuration.GetSection("111").Value,
            })
            .ToArray();
        }

        [HttpGet, Route("api/GetUser")]
        public string GetUser()
        {
            var result = new { Description = "用户信息", Name = "张小三", Age = 18, Email = "zxs@163.com" };
            return JsonConvert.SerializeObject(result);
        }

        [HttpGet, Route("api/GetUserList")]
        public async Task<object> GetUserList()
        {
            try
            {
                var userUrl = $"{_configuration.GetSection("productserviceurl").Value}/api/get";
                var result = _httpClient.GetStringAsync(userUrl).Result;
                var resultobj = new { product = Summaries, user = result };
                return resultobj;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
    }
}


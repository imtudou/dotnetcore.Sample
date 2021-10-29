using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using _05.Configuration.OptionDemo.Service;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace _05.Configuration.OptionDemo.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IUserInfoService _userInfoService;
       
        public WeatherForecastController(ILogger<WeatherForecastController> logger,
            IUserInfoService userInfoService)
        {
            _logger = logger;
            _userInfoService = userInfoService;
        }

        [HttpGet]
        [Obsolete]
        public string Get()
        {
            _logger.LogInformation("IOptions<UserInfoOptions>");
            return _userInfoService.GetUserInfoByIOptions();
        }

        [HttpGet]
        public string GetS()
        {
            _logger.LogInformation("UserInfoOptionsSnapshot");
            return _userInfoService.GetUserInfoByIOptionsSnapshot();
        }

        [HttpGet]
        public string GetM()
        {
            _logger.LogInformation("UserInfoOptionsMonitor");
            return _userInfoService.GetUserInfoByIOptionsMonitor();
        }
    }
}

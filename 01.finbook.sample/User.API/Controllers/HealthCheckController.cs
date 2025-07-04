using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

namespace User.API.Controllers
{
    [ApiController]
    [Route("api/HealthCheck")]
    public class HealthCheckController : ControllerBase
    {
        [HttpGet]
        [Route("")]
        public IActionResult Get()
        {
            return Ok("userapi start success!");
        }
    }
}

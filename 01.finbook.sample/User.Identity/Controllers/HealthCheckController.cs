using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace User.Identity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthCheckController : ControllerBase
    {

        [HttpGet]
        [Route("")]
        public IActionResult Get()
        {
            return Ok("User.Identity start success!");
        }
    }
}

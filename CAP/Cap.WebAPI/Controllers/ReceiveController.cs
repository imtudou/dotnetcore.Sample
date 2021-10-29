using DotNetCore.CAP;

using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cap.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ReceiveController : ControllerBase
    {
        [CapSubscribe("test.show.time1")]
        public IActionResult ReceiveMessage1(DateTime time)
        {
            return Ok(time);
        }

        [CapSubscribe("test.show.time2")]
        public IActionResult ReceiveMessage2(DateTime time)
        {
            return Ok(new ArgumentException("DateTime error!"));
        }

    }
}

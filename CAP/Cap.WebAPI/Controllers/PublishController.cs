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
    public class PublishController: ControllerBase
    {
        private readonly ICapPublisher capPublisher;

        public PublishController(ICapPublisher cap)
        {
            this.capPublisher = cap;
        }

        [HttpGet]
        public async Task<IActionResult> SendMessage()
        {
            await this.capPublisher.PublishAsync("test.show.time", DateTime.Now);
            await this.capPublisher.PublishAsync("test.show.time", DateTime.Now);
            return Ok();
        }

    }
}

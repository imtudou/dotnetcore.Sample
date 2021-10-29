using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using DnsClient;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

using User.Identity.Entity.Dtos;

namespace User.Identity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {

        private readonly IDnsQuery _dnsQuery;
        private readonly IOptions<ServiceDisvoveryOptions> _options;

        public TestController(IDnsQuery dnsQuery, IOptions<ServiceDisvoveryOptions> options)
        {
            _dnsQuery = dnsQuery ?? throw new ArgumentNullException(nameof(dnsQuery));
            _options = options ?? throw new ArgumentNullException(nameof(options));

        }


        [HttpGet("")]
        [HttpHead("")]
        public async Task<IActionResult> DoSomething()
        {
            var result = await _dnsQuery.ResolveServiceAsync("service.consul", _options.Value.ServiceName);
            var addressList = result.First().AddressList;
            var host = addressList.Any() ? addressList.First().ToString() : result.First().HostName;
            var port = result.First().Port;
            var url = $"http://{host}:{port}";
            return Ok(result);
        }

    }
}

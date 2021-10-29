using Microsoft.Extensions.Logging;

using Polly;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using System.Threading.Tasks;

namespace User.Identity.Infrastructure
{
    public class ResilienceHttpClient
    {
        private readonly ILogger<ResilienceHttpClient> _logger;

        public ResilienceHttpClient(ILogger<ResilienceHttpClient> logger)
        {
            _logger = logger;
        }


    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace User.API.Entity.Dtos
{
    public class ServiceDiscoveryOptions
    {
        public string ServiceName { get; set; }
        public ConsulOptions Consul { get; set; }
    }
}

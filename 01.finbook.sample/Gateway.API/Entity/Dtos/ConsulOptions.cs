using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gateway.API.Entity.Dtos
{
    public class ConsulOptions
    {
        public string HttpEndPoint { get; set; }
        public DnsEndPoint DnsEndPoint { get; set; }
    }
}

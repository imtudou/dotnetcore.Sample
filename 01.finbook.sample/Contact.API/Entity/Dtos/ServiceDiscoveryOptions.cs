﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Contact.API.Entity.Dtos
{
    public class ServiceDiscoveryOptions
    {
        public string ServiceName { get; set; }
        public ConsulOptions Consul { get; set; }
    }
}

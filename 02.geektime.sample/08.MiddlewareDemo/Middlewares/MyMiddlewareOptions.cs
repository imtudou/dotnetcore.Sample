using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _08.MiddlewareDemo.Middlewares
{
    public class MyMiddlewareOptions
    {
        public string SystemKey { get; set; }

        public void SetSystemKey(string systemkey)
        {
            SystemKey = systemkey;
        }

    }
}

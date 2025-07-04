
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace _02.DependencylnjectionDemo.Service
{
    public class MyScopedService:IMyScopedService
    {
        public string GetService()
        {
            var fullName = Assembly.GetAssembly(this.GetType()).GetName().FullName;
            return fullName;
        }
    }
}

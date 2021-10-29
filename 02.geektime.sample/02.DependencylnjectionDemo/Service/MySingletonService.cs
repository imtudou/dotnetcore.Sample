using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace _02.DependencylnjectionDemo.Service
{
    public class MySingletonService: IMySingletonService
    {
        public string GetService()
        {
            var fullName = "MySingletonService";
            System.Console.WriteLine(fullName);
            return fullName;
        }
    }
}

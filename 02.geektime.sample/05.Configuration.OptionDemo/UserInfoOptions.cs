using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _05.Configuration.OptionDemo
{
    public class UserInfoOptions
    {

        public int id { get; set; }
        public string name { get; set; }

        public UserInfo userInfo { get; set; }
        
    }
    public class UserInfo
    {
        public int age { get; set; }
        public string email { get; set; }
        public Address address { get; set; }

    }
    public class Address
    {
        public string province { get; set; }
        public string city { get; set; }
    }
}

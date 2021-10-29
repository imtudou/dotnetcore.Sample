using System;
using System.Collections.Generic;
using System.Text;

namespace _4.ConfigurationDemo
{
    public class UserInfoCofig
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

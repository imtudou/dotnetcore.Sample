using System;

namespace RedisTestDto
{
    public class UserInfo_StringSetDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public Address Address { get; set; }
    }

    public class Address
    {
        public string Province { get; set; }
        public string City { get; set; }
        public string Town { get; set; }
        public string PostalCode { get; set; }

    }
}

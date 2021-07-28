using System;
using System.Runtime.Serialization;

namespace _01.Shared.Grpc
{
    public class CreateOrderOutput
    {
        [DataMember(Order = 1)]
        public int OrderId { get; set; }

        [DataMember(Order = 2)]
        public string OrderName { get; set; }
    }
}

using System;
using System.Runtime.Serialization;

namespace _01.Shared.Grpc
{
    public class CreateOrderInput
    {
        [DataMember(Order = 1)]
        public int BuyerId { get; set; }

        [DataMember(Order = 2)]
        public string BuyerName { get; set; }

        [DataMember(Order = 3)]
        public int OrderId { get; set; }

        [DataMember(Order = 4)]
        public string OrderName { get; set; }
    }
}

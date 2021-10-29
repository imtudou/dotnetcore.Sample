namespace GrpcService1.Shared
{
    using System.Runtime.Serialization;
    [DataContract]
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

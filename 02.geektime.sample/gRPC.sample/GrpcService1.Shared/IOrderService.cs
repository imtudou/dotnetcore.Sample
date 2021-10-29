namespace GrpcService1.Shared
{
    using ProtoBuf.Grpc;
    using System.ServiceModel;
    using System.Threading.Tasks;

    [ServiceContract]
    public interface IOrderService
    {
        [OperationContract]
        Task<CreateOrderOutput> CreateOrderAsync(CreateOrderInput request, CallContext context = default);
    }
}

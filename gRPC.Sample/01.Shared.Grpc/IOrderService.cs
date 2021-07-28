using ProtoBuf.Grpc;

using System;
using System.ServiceModel;
using System.Threading.Tasks;

namespace _01.Shared.Grpc
{
    [ServiceContract]
    public interface IOrderService
    {
        [OperationContract]
        Task<CreateOrderOutput> CreateOrderAsync(CreateOrderInput input, CallContext context = default);
    }
}

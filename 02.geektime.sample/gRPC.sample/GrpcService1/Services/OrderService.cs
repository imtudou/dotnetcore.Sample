namespace GrpcService1.Services
{
    using ProtoBuf.Grpc;
    using GrpcService1.Shared;
    using System.Threading.Tasks;

    public class OrderService : IOrderService
    {
        public async Task<CreateOrderOutput> CreateOrderAsync(CreateOrderInput request, CallContext context = default)
        {
            return await Task.FromResult(new CreateOrderOutput
            {
                BuyerId = request.BuyerId,
                BuyerName = request.BuyerName,
                OrderId = request.OrderId,
                OrderName = request.OrderName,
            });
        }
    }
}

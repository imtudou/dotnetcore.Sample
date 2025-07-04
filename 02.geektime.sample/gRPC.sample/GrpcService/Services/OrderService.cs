using Grpc.Core;
using GrpcService;
using System.Threading.Tasks;

namespace GrpcService.Services
{
    public class OrderService:OrderGrpc.OrderGrpcBase
    {
        public override Task<CreateOrderOutPut> CreateOrder(CreateOrderInput request, ServerCallContext context)
        {
            return Task.FromResult(new CreateOrderOutPut
            {
                BuyerId = request.BuyerId,
                BuyerName = request.BuyerName,
                OrderId = request.OrderId,
                OrderName = request.OrderName,
            });
        }
    }
}

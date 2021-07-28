using _01.Shared.Grpc;

using ProtoBuf.Grpc;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _01.ProductService.Grpc.Service
{
    public class OrderService : IOrderService
    {
        public async Task<CreateOrderOutput> CreateOrderAsync(CreateOrderInput input, CallContext context = default)
        {
            return await Task.FromResult(new CreateOrderOutput
            {
                OrderId = input.OrderId,
                OrderName = input.OrderName,
            });
        }
    }
}

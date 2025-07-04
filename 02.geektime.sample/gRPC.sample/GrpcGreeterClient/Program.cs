

namespace GrpcGreeterClient
{
    using Grpc.Net.Client;
    using GrpcOrderClient;
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;

    class Program
    {
        static async Task Main(string[] args)
        {
            // The port number(5001) must match the port of the gRPC server.

            #region .NET Core 客户端必须在服务器地址中使用 https 才能使用安全连接进行调用：
            //var channel = GrpcChannel.ForAddress("https://localhost:5001");
            #endregion;


            #region 使用不受信任/无效证书调用 gRPC 服务
            //https://docs.microsoft.com/zh-cn/aspnet/core/grpc/troubleshoot?view=aspnetcore-3.0#call-insecure-grpc-services-with-net-core-client
            var httpHandler = new HttpClientHandler();
            httpHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

            var channel = GrpcChannel.ForAddress("https://localhost:5001",
                new GrpcChannelOptions { HttpHandler = httpHandler }); 
            #endregion


            var client = new OrderGrpc.OrderGrpcClient(channel);
            var result = await client.CreateOrderAsync(new CreateOrderInput
            {
                BuyerId = 1,
                BuyerName = "张小三",
                OrderId = 1,
                OrderName = "方便面",
            });
            Console.WriteLine("Order: " + result);
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}



namespace GrpcClient1
{
    using Grpc.Net.Client;
    using GrpcService1.Shared;
    using ProtoBuf.Grpc.Client;

    using System;
    using System.Net.Http;
    using System.Text.Json;
    using System.Text.Unicode;
    using System.Threading.Tasks;
    class Program
    {
        static async Task Main(string[] args)
        {
            //  使用不受信任/无效证书调用 gRPC 服务
            var httpHandler = new HttpClientHandler();
            httpHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

            using var channel = GrpcChannel.ForAddress("https://localhost:5001", new GrpcChannelOptions { HttpHandler = httpHandler });
            var client = channel.CreateGrpcService<IOrderService>();

            var result = await client.CreateOrderAsync(new CreateOrderInput
            {
                BuyerId = 1,
                BuyerName = "张小三",
                OrderId = 1,
                OrderName = "方便面",
            });
            var options = new JsonSerializerOptions();
            options.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(UnicodeRanges.All);
            Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(result, options));
            Console.ReadKey();
             
        }
    }
}

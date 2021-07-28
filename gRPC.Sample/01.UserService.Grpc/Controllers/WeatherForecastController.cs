using _01.Shared.Grpc;

using Grpc.Net.Client;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using ProtoBuf.Grpc.Client;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace _01.UserService.Grpc.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IOrderService _orderService;
  

        public WeatherForecastController(ILogger<WeatherForecastController> logger
             )
        {
            _logger = logger;
            
        }

        [HttpGet]
        public async Task<CreateOrderOutput> GetOrder()
        {


            //return await _orderService.CreateOrderAsync(new CreateOrderInput
            //{
            //    BuyerId = 1,
            //    BuyerName = "张小三",
            //    OrderId = 1,
            //    OrderName = "方便面",
            //});
            //return await Task.FromResult(new CreateOrderOutput { });


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

            return result;


            //var options = new JsonSerializerOptions();
            //options.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(UnicodeRanges.All);
            //Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(result, options));
            //Console.ReadKey();

        }
    }
}

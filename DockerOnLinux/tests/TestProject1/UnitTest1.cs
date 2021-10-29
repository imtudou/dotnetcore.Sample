using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using imtudou.UserServiceOnLinuxContainers.Controllers;

namespace TestProject1
{
    [TestClass]
    public class UnitTest1
    {
        public readonly IConfiguration _configuration;
        public UnitTest1(IConfiguration configuration)
        {
            this._configuration = configuration;
        }


        [TestMethod]
        public void TestMethod1()
        {
            var logger = new LoggerFactory().CreateLogger<WeatherForecastController>();
            WeatherForecastController controller = new WeatherForecastController(logger, _configuration);
            var result = controller.Get();
            Console.WriteLine(JsonSerializer.Serialize(result));
            Assert.IsNotNull(result);
        }
    }
}

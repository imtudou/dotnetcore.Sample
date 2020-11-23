using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using zipkin4net.Transport.Http;

namespace UserService.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IWebHostEnvironment _env;
        public UserController(ILogger<UserController> logger, IWebHostEnvironment env)
        {
            _logger = logger;
            _env = env;
        }

        [HttpGet]
        public string values()
        {
            return "UserService start by " + DateTime.Now.ToString();
        }

        [HttpGet]
        [Route("getuser")]
        public UserViewModel GetUserInfo()
        {
            Thread.Sleep(5000);
            HttpClient client = new HttpClient(new TracingHandler(_env.ApplicationName));
            var product =  client.GetAsync("http://localhost:5001/api/product").Result.Content.ReadAsStringAsync().Result;
            client.Dispose();
            return new UserViewModel {
                UserId = Guid.NewGuid().ToString(),
                UserName = "hahha",
                UserPassword = "hahah".ToMd5(),
                Product = product            
            };
        }



    }

    public class UserViewModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string UserPassword { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }

        public string Product { get; set; }
    }

 

    public static class ExtensionClass
    {
        public static string ToMd5(this string str)
        { 
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] bytes = Encoding.Default.GetBytes(str);//将要加密的字符串转换为字节数组
            byte[] encryptdata = md5.ComputeHash(bytes);//将字符串加密后也转换为字符数组
            return Convert.ToBase64String(encryptdata);//将加密后的字节数组转换为加密字符串

        }
    }
}

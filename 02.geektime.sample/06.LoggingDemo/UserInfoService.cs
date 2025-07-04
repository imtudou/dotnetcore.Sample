
using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _06.LoggingDemo
{
    public class UserInfoService
    {
        private readonly ILogger<UserInfoService> _logger;

        public UserInfoService(ILogger<UserInfoService> logger)
        {
            _logger = logger;
        }

        public string GetUserInfo()
        {
            /*
                注意：此处两种日志输出会存在新性能差异
                第一种用模板的方式拼接字符串：是在执行_logger.LogInformation()要输出的时候才开始拼接字符串
                第二种拼接字符串：是先拼接好再输出
             */
            _logger.LogInformation("HELLOxxxx:{0}",DateTime.Now);
            _logger.LogInformation($"HELLO WORLD!:{DateTime.Now.ToString()}");
            return string.Empty;
        }
    }
}

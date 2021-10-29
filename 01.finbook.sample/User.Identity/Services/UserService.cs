using DnsClient;

using IdentityModel.Client;

using IdentityServer4.Extensions;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Polly;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

using User.Identity.Entity.Dtos;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;

namespace User.Identity.Services
{
    public class UserService : IUserService
    {
        private readonly string _userServiceUrl; 
        private readonly HttpClient _httpClient;
        private readonly ILogger<UserService> _logger;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(HttpClient httpClient,
            IOptions<ServiceDisvoveryOptions> options,
            IDnsQuery dnsQuery,
            ILogger<UserService> logger,
            IConfiguration configuration,
            IHttpContextAccessor  httpContextAccessor
            ) 
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
            _configuration = configuration;



            var address = dnsQuery.ResolveService("service.consul", options.Value.ServiceName);
            if (!string.IsNullOrEmpty(httpContextAccessor.HttpContext.Request?.Path.Value)
                && httpContextAccessor.HttpContext.Request.Path.Value.ToLower().Contains("/connect/token"))
            {
                address = dnsQuery.ResolveService("service.consul", _configuration.GetValue<string>("PostUserapiServiceName"));
            }
            
            var addressList = address.First().AddressList;
            var host = addressList.Any() ? addressList.First().ToString() : address.First().HostName;
            var port = address.First().Port;
            _userServiceUrl = $"http://{host}:{port}/";


        }



        public async Task<UserInfoDto> CheckOrCreateAsync(string phone)
        {

            try
            {
                /**
                    1.如果参数是 ?phone="15071651111" 的形式发起的post请求 则webapi中 Controller 的方法需要加上 [FromQuery] 
                    例：
                        string url = _userServiceUrl + "api/User/CheckOrCreate?phone=" + phone;
                        var response = await _httpClient.PostAsync(url, null);

                    webapi:
                        public async Task<IActionResult> CheckOrCreate([FromQuery]string phone)
                        {
                            //dosomething...
                        }


                    2.如果参数是通过HttpContent发送的单个字符串
                    例：
                        var data = new FormUrlEncodedContent(new Dictionary<string, string> {{"phone",phone }});
                        var response = await _httpClient.PostAsync(url, data);

                    webapi:
                        public async Task<IActionResult> CheckOrCreate([FromForm]string phone)
                        {
                            //dosomething...
                        }


                    3.如果参数是通过HttpContent发送的JSON
                    例：
                        string url = _userServiceUrl + "api/User/CheckOrCreate";
                        UserInfoDto dto = new UserInfoDto { Name = "Yuan", Phone = phone };
                        var data = new StringContent(JsonConvert.SerializeObject(dto),Encoding.UTF8,"application/json");

                    webapi:
                        public async Task<IActionResult> CheckOrCreate([FromBody]CheckOrCreateAppUserViewModel model)
                        {
                            //dosomething...
                        }


                 **/

                string url = _userServiceUrl + "api/User/CheckOrCreate";
                //UserInfoDto dto = new UserInfoDto { Name = "Yuan", Phone = phone };
                //var data = new StringContent(JsonConvert.SerializeObject(dto),Encoding.UTF8,"application/json");

                var data = new FormUrlEncodedContent(new Dictionary<string, string> { { "phone", phone } });
                //var response = await _httpClient.PostAsync(url, data);

                int retryCount = _configuration.GetValue<int>("PollySeetings:retryCount");
                int exceptionsAllowedBeforeBreaking = _configuration.GetValue<int>("PollySeetings:exceptionsAllowedBeforeBreaking");
                var response = Policy
                    // 1.指定要处理什么异常
                    .Handle<HttpRequestException>()
                    // 2.或者指定需要处理什么样的错误返回
                    //.OrResult<HttpResponseMessage>(r => r.StatusCode != HttpStatusCode.OK)
                    // 3.指定重试次数和重试策略
                    .WaitAndRetryAsync(retryCount,
                        retryCountTimes => TimeSpan.FromSeconds(Math.Pow(2, retryCountTimes)),
                        (ex, count, context) =>
                    {
                        _logger.LogInformation("执行失败! 重试次数: {0}", count);
                        _logger.LogInformation("异常来自: {0}", ex);

                    })
                    // 3.执行具体任务
                    .ExecuteAsync(() =>
                    {
                        return  _httpClient.PostAsync(url, data);
                    }).Result;

                Policy.Handle<HttpRequestException>()
                    .CircuitBreakerAsync(
                    // 熔断前允许出现几次错误
                    exceptionsAllowedBeforeBreaking,
                    TimeSpan.FromSeconds(30),
                    (ex, duration) =>
                    {
                        _logger.LogInformation("执行失败! 熔断器打开: {0},总毫秒数:{1}", DateTime.Now, duration.TotalMilliseconds);
                    }, () =>
                    {
                        _logger.LogInformation("执行失败! 熔断器关闭: {0}", DateTime.Now);
                    });


                if (response.IsSuccessStatusCode)
                {
                    var result  = await response.Content.ReadAsStringAsync();
                    var userinfo = JsonConvert.DeserializeObject<UserInfoDto>(result);
                    _logger.LogInformation("[User.Identity].[UserServices.cs].[CheckOrCreateAsync()]---result:" + result);
                    return userinfo;
                }

            }
            catch (Exception ex)
            {

                _logger.LogError("重试异常：", ex.Message);
            }

            return null;
        }
    }
}

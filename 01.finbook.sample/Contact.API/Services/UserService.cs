using Contact.API.Entity.Dtos;

using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Polly;
 


namespace Contact.API.Services
{
    public class UserService : IUserService
    {

        private readonly string _userApiUrl;
        private readonly ILogger<UserService> _logger;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;


        public UserService(ILogger<UserService> logger,HttpClient httpClient,IConfiguration configuration)
        {
            _configuration = configuration;
            _userApiUrl = _configuration.GetValue<string>("UserServiceUrl");
            _logger = logger;
            _httpClient = httpClient;

        }
        public async Task<UserInfoDto> GetUserInfoAsync(int userid)
        {
            //dosomething
            //此处从服务内部发起网络请求 调用User.API.UserController.GetUserInfo()的方法
            //
            var data = new FormUrlEncodedContent(new Dictionary<string, string> { { "userId",userid.ToString()} });
            //var response = await _httpClient.PostAsync(_userApiUrl, data);

            var response =
                //1.指处理什么类型的异常
                Policy.Handle<HttpRequestException>()
                //2.指定需要处理什么样的错误返回
                .OrResult<HttpResponseMessage>(s => s.StatusCode != System.Net.HttpStatusCode.BadRequest)
                //3.指定重试次数和策略
                .WaitAndRetryAsync(3, s => TimeSpan.FromSeconds(Math.Pow(2, s)),
                 (ex, count, context) =>
                 {
                     _logger.LogInformation("执行失败! 重试次数: {0}", count);
                     _logger.LogInformation("异常来自: {0}", ex);
                 })
                .ExecuteAsync(() =>
                {
                    return _httpClient.PostAsync(_userApiUrl, data);
                }).Result;


            if (response.IsSuccessStatusCode)
            {   
                return JsonConvert.DeserializeObject<UserInfoDto>(await response.Content.ReadAsStringAsync());
            }
            return null;
        }
    }
}

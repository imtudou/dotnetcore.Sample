using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _05.Configuration.OptionDemo.Service
{
    public static class UserInfoServiceExtensions
    {
        public static IServiceCollection AddUserInfoService(this IServiceCollection services, IConfiguration Configuration)
        {
            #region 从配置读取数据后需要对其操作
            //services.Configure<UserInfoOptions>(Configuration.GetSection(nameof(UserInfoOptions)));
            //services.PostConfigure<UserInfoOptions>(options =>
            //{
            //    options.name = "【DATA..CCX】" + options.name;
            //});
            #endregion

            #region 添加验证属性
            // 如果条件不成立则不返回
            //services.AddOptions<UserInfoOptions>()
            //       .Configure(options =>
            //       {
            //           Configuration.Bind(options);

            //       }).Validate(options => options.name!="张三");

            #endregion

            #region 添加自定义验证类
            services.AddOptions<UserInfoOptions>()
                .Configure(options =>
                {
                    Configuration.Bind(options);
                })
                .Services.AddSingleton<IValidateOptions<UserInfoOptions>, UserInfoServiceValidateOptions>();

            #endregion

            //services.AddScoped<IUserInfoService, UserInfoService>();
            services.AddSingleton<IUserInfoService, UserInfoService>();// 使用单例模式注册服务 IOptionsSnapshot 无法读取。应改为IOptionsMonitor
            return services;
        }
    }
}

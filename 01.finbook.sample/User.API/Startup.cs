using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using User.API.Data;
using User.API.Filters;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Hosting.Server.Features;
using System.Linq;
using User.API.Entity.Dtos;
using Microsoft.Extensions.Options;
using Consul;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;

namespace User.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            #region DB
            var connstr = Configuration.GetSection("ConnectionString:MySql").Value;
            services.AddDbContext<UserContext>(options =>
            {
                options.UseMySQL(connstr, m => m.MigrationsAssembly("User.API"));
            }); 
            #endregion
            // 配置JSON https://docs.microsoft.com/zh-cn/aspnet/core/web-api/advanced/formatting?view=aspnetcore-3.1#add-newtonsoftjson-based-json-format-support
            services.AddControllers().AddNewtonsoftJson();

            #region Authentication身份验证
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = "http://localhost:5002";//IdentityServer 地址
                    options.Audience = "user_api";
                    options.RequireHttpsMetadata = false;
                });

            services.AddAuthorization(options =>
            {
                //基于策略授权
                options.AddPolicy("User_API_Policy", policy =>
                {
                    //客户端Scope中包含user_api.scope才能访问 
                    //添加IdentityServer4.AccessTokenValidation包
                    policy.RequireScope("user_api.scope");
                });
            });

            #endregion

            #region 配置Consul
            services.Configure<ServiceDiscoveryOptions>(Configuration.GetSection(nameof(ServiceDiscoveryOptions)));
            //单例注册ConsulClient
            services.AddSingleton<IConsulClient>(s => new ConsulClient(config =>
            {
                var serviceConfiguration = s.GetRequiredService<IOptions<ServiceDiscoveryOptions>>().Value;
                if (!string.IsNullOrEmpty(serviceConfiguration.Consul.HttpEndPoint))
                {
                    config.Address = new Uri(serviceConfiguration.Consul.HttpEndPoint);
                }
            }));
            #endregion

            #region Filter
            services.AddControllers(options =>
            {
                options.Filters.Add(typeof(GlobalExceptionFilter));

            }); 
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, 
            IWebHostEnvironment env,
            ILoggerFactory loggerFactory,
            IHostApplicationLifetime hostApplicationLifetime,
            IOptions<ServiceDiscoveryOptions> serviceDisvoveryOptions,
            IConsulClient consul )            
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //启动时注册服务
            hostApplicationLifetime.ApplicationStarted.Register(()=> {
                RegisterService(app, serviceDisvoveryOptions,consul);
            });

            //停止时卸载服务
            hostApplicationLifetime.ApplicationStopped.Register(() =>
            {
                DeRegisterService(app, serviceDisvoveryOptions, consul);
            });


            app.UseHttpsRedirection();

            app.UseRouting();

            //身份验证
            app.UseAuthentication();

            //授权
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        
        private void RegisterService(IApplicationBuilder app, 
            IOptions<ServiceDiscoveryOptions> serviceDisvoveryOptions,
            IConsulClient consul

            )
        {
            //从当前启动的url中拿到url
            var features = app.Properties["server.Features"] as FeatureCollection;
            var address = features.Get<IServerAddressesFeature>()
                .Addresses
                .Select(s => new Uri(s));

            foreach (var item in address)
            {
                //serviceid必须是唯一的，以便以后再次找到服务的特定实例，以便取消注册。这里使用主机和端口以及实际的服务名
                var serviceid = $"{serviceDisvoveryOptions.Value.ServiceName}_{item.Host}:{item.Port}";
                var httpcheck = new AgentServiceCheck()
                {
                    DeregisterCriticalServiceAfter = TimeSpan.FromMinutes(1),
                    Interval = TimeSpan.FromSeconds(30),
                    HTTP = new Uri(item, "api/HealthCheck").OriginalString
                };

                var registration = new AgentServiceRegistration
                {
                    Checks = new[] { httpcheck },
                    Address = item.Host,
                    ID = serviceid,
                    Name = serviceDisvoveryOptions.Value.ServiceName,
                    Port = item.Port

                };

                consul.Agent.ServiceRegister(registration).GetAwaiter().GetResult();
            }
        
            
        
        }

        private void DeRegisterService(IApplicationBuilder app,
            IOptions<ServiceDiscoveryOptions> serviceDisvoveryOptions,
            IConsulClient consul)
        {
            //从当前启动的url中拿到url
            var features = app.Properties["service:Features"] as FeatureCollection;
            var address = features.Get<IServerAddressesFeature>()
                .Addresses
                .Select(s => new Uri(s));

            foreach (var item in address)
            {
                var serviceid = $"{serviceDisvoveryOptions.Value.ServiceName}_{item.Host}:{item.Port}";
                consul.Agent.ServiceDeregister(serviceid).GetAwaiter().GetResult();
            }         
        }
    }
}

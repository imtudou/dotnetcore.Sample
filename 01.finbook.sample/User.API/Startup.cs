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
            // ����JSON https://docs.microsoft.com/zh-cn/aspnet/core/web-api/advanced/formatting?view=aspnetcore-3.1#add-newtonsoftjson-based-json-format-support
            services.AddControllers().AddNewtonsoftJson();

            #region Authentication�����֤
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = "http://localhost:5002";//IdentityServer ��ַ
                    options.Audience = "user_api";
                    options.RequireHttpsMetadata = false;
                });

            services.AddAuthorization(options =>
            {
                //���ڲ�����Ȩ
                options.AddPolicy("User_API_Policy", policy =>
                {
                    //�ͻ���Scope�а���user_api.scope���ܷ��� 
                    //���IdentityServer4.AccessTokenValidation��
                    policy.RequireScope("user_api.scope");
                });
            });

            #endregion

            #region ����Consul
            services.Configure<ServiceDiscoveryOptions>(Configuration.GetSection(nameof(ServiceDiscoveryOptions)));
            //����ע��ConsulClient
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

            //����ʱע�����
            hostApplicationLifetime.ApplicationStarted.Register(()=> {
                RegisterService(app, serviceDisvoveryOptions,consul);
            });

            //ֹͣʱж�ط���
            hostApplicationLifetime.ApplicationStopped.Register(() =>
            {
                DeRegisterService(app, serviceDisvoveryOptions, consul);
            });


            app.UseHttpsRedirection();

            app.UseRouting();

            //�����֤
            app.UseAuthentication();

            //��Ȩ
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
            //�ӵ�ǰ������url���õ�url
            var features = app.Properties["server.Features"] as FeatureCollection;
            var address = features.Get<IServerAddressesFeature>()
                .Addresses
                .Select(s => new Uri(s));

            foreach (var item in address)
            {
                //serviceid������Ψһ�ģ��Ա��Ժ��ٴ��ҵ�������ض�ʵ�����Ա�ȡ��ע�ᡣ����ʹ�������Ͷ˿��Լ�ʵ�ʵķ�����
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
            //�ӵ�ǰ������url���õ�url
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

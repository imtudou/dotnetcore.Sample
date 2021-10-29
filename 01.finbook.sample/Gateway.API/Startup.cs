using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Consul;

using Gateway.API.Entity.Dtos;

using IdentityServer4.AccessTokenValidation;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Ocelot.DependencyInjection;
using Ocelot.Middleware;

namespace Gateway.API
{
    public class Startup
    {

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; set; }


        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            #region ������֤
            var authenticationProviderKey = "finbook";
            services.AddAuthentication()
                .AddIdentityServerAuthentication(authenticationProviderKey, options =>
                {
                    options.Authority = "http://localhost:5002";//������Ȩ��֤�ĵ�ַ IdentityServer �ĵ�ַ
                    options.ApiName = "gateway_api";//��Դ���ƣ�����֤������ע�����Դ�б������е�apiResourceһ��
                    options.SupportedTokens = SupportedTokens.Both;
                    options.ApiSecret = "secret";
                    options.RequireHttpsMetadata = false;//�Ƿ�����https
                });
            #endregion



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

            services.AddOcelot();//ע��Ocelot����


            services.AddControllers();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,
            ILoggerFactory loggerFactory,
            IHostApplicationLifetime hostApplicationLifetime,
            IOptions<ServiceDiscoveryOptions> serviceDisvoveryOptions,
            IConsulClient consul)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //����ʱע�����
            hostApplicationLifetime.ApplicationStarted.Register(() => {
                RegisterService(app, serviceDisvoveryOptions, consul);
            });

            //ֹͣʱж�ط���
            hostApplicationLifetime.ApplicationStopped.Register(() =>
            {
                DeRegisterService(app, serviceDisvoveryOptions, consul);
            });

            

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.UseOcelot().Wait();


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

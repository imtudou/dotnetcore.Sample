using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

using Consul;

using DnsClient;

using IdentityServer4.Services;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using User.Identity.Authentication;
using User.Identity.Entity.Dtos;
using User.Identity.Services;

namespace User.Identity
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
            #region ����Identity Server
                var builder = services.AddIdentityServer()
            //����֤��
            .AddDeveloperSigningCredential()
            //���������Դ
            .AddInMemoryIdentityResources(IdentityServerConfig.GetIdentityResources())
            //����API��Դ
            .AddInMemoryApiResources(IdentityServerConfig.GetApiResources())
            //������ʵķ�Χ
            .AddInMemoryApiScopes(IdentityServerConfig.GetApiScopes())
            //Ԥ����Client
            .AddInMemoryClients(IdentityServerConfig.GetClients())
            //IdentityServer�Զ�����֤
            .AddExtensionGrantValidator<SmsAuthCodeValidator>();
            #endregion



            //�������ļ��л�ȡServiceDiscovery
            services.Configure<ServiceDisvoveryOptions>(Configuration.GetSection("ServiceDisvoveryOptions"));
          
            #region DI
            services.AddScoped<IAuthCodeService, AuthCodeService>()
                    .AddScoped<IUserService, UserService>()
                    .AddSingleton<HttpClient>();
            

            //ע��dnslookup�ͻ���
            services.AddSingleton<IDnsQuery>(p =>
            {
                var serviceConfig = p.GetRequiredService<IOptions<ServiceDisvoveryOptions>>().Value;//�������ļ��л�ȡconsul���������Ϣ
                var client = new LookupClient(IPAddress.Parse("127.0.0.1"), 8600);
                if (serviceConfig.Consul.DnsEndPoint != null)
                {
                    client = new LookupClient(serviceConfig.Consul.DnsEndPoint.ToIPEndPoint());
                }
                return client;

            });

            services.Configure<ServiceDisvoveryOptions>(Configuration.GetSection(nameof(ServiceDisvoveryOptions)));
            //����ע��ConsulClient
            services.AddSingleton<IConsulClient>(s => new ConsulClient(config => {
                var serviceConfiguration = s.GetRequiredService<IOptions<ServiceDisvoveryOptions>>().Value;
                if (!string.IsNullOrEmpty(serviceConfiguration.Consul.HttpEndPoint))
                {
                    config.Address = new Uri(serviceConfiguration.Consul.HttpEndPoint);
                }
            }));

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddTransient<IProfileService, ProfileService>();
            #endregion



            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,
            IHostApplicationLifetime hostApplicationLifetime,
            IOptions<ServiceDisvoveryOptions> serviceDisvoveryOptions,
            IConsulClient consul)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }


            #region Consul
            //����ʱע�����
            hostApplicationLifetime.ApplicationStarted.Register(() =>
            {
                RegisterService(app, serviceDisvoveryOptions, consul);
            });

            //ֹͣʱж�ط���
            hostApplicationLifetime.ApplicationStopped.Register(() =>
            {
                DeRegisterService(app, serviceDisvoveryOptions, consul);
            });
            #endregion

            #region Zipkin
            
            #endregion


            app.UseHttpsRedirection();

            app.UseRouting();

            //ʹ��IdentityServer
            app.UseIdentityServer();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }


        private void RegisterService(IApplicationBuilder app,
            IOptions<ServiceDisvoveryOptions> serviceDisvoveryOptions,
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
            IOptions<ServiceDisvoveryOptions> serviceDisvoveryOptions,
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

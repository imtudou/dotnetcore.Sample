using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Contact.API.Data;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MongoDB;
using Contact.API.Entity.Dtos;
using Contact.API.Services;
using Consul;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using System.Net.Http;
using System.Runtime.InteropServices.ComTypes;

namespace Contact.API
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


            //nameof�������� https://www.cnblogs.com/yuwen/p/4173370.html
            //����Mongo
            services.Configure<ContactDataBaseSettings>(Configuration.GetSection(nameof(ContactDataBaseSettings)));


            #region IdentityServer
            //��֤
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = "http://localhost:5002";//IdentityServer ��ַ
                    options.Audience = "contact_api";
                    options.RequireHttpsMetadata = false;
                });


            services.AddAuthorization(options =>
            {
                //���ڲ�����Ȩ
                options.AddPolicy("Contact_API_Policy", policy =>
                {
                    //�ͻ���Scope�а���contact_api.scope���ܷ���
                    //���IdentityServer4.AccessTokenValidation��
                    policy.RequireScope("contact_api.scope");
                });
            });


            #endregion


            #region DI
            services.AddSingleton<ContactDataBaseSettings>();
            services.AddSingleton<ContactContext>();
            services.AddScoped<IContactApplyRequestRepository, MongoContactApplyRequestRepository>()
                .AddScoped<IContactRepository, MongoContactRepository>()
                .AddScoped<IUserService, UserService>();
            services.AddSingleton<HttpClient>();

            services.Configure<ServiceDiscoveryOptions>(Configuration.GetSection(nameof(ServiceDiscoveryOptions)));
            services.AddSingleton<IConsulClient>(s => new ConsulClient(config =>
            {
                var serviceConfiguration = s.GetRequiredService<IOptions<ServiceDiscoveryOptions>>().Value;
                if (!string.IsNullOrEmpty(serviceConfiguration.Consul.HttpEndPoint))
                {
                    config.Address = new Uri(serviceConfiguration.Consul.HttpEndPoint);
                }
            })); 
            #endregion

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

            //���÷���ע�ᣬ������
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


            //IQueryable
            //Enumerable

                
        }
    }
}

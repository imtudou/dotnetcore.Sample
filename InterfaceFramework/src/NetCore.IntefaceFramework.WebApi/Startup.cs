using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace NetCore.IntefaceFramework.WebApi
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            //configuration = new ConfigurationBuilder()
            //    .SetBasePath(Directory.GetCurrentDirectory())
            //    .AddJsonFile("appsettings.json", true, true)
            //    .Build();
            Configuration = configuration;
        }


        /// <summary>
        /// 注册服务
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            #region 配置跨域
            services.AddCors(options =>
            {
                //默认策略
                options.AddDefaultPolicy(builder =>
                {
                    //AllowAnyOrigin – 允许来自任何方案使用所有来源的 CORS 请求 (http或https)
                    builder.SetIsOriginAllowedToAllowWildcardSubdomains()
                        .WithOrigins("*") ////允许http://localhost:8089的主机访问
                        .AllowAnyMethod() //允许任何 HTTP 方法
                        .AllowAnyHeader() //允许所有作者的请求标头调用AllowAnyHeader:
                        .AllowCredentials() //服务器必须允许凭据。 若要允许跨域凭据，调用AllowCredentials:
                        .AllowAnyOrigin(); //允许任何来源的主机访问

                });

                //自定义策略
                options.AddPolicy("AllowAllOrigin", builder =>
                {
                    //AllowAnyOrigin – 允许来自任何方案使用所有来源的 CORS 请求 (http或https)
                    builder.SetIsOriginAllowedToAllowWildcardSubdomains()
                        .WithOrigins("*") ////允许http://localhost:8089的主机访问
                        .AllowAnyMethod() //允许任何 HTTP 方法
                        .AllowAnyHeader() //允许所有作者的请求标头调用AllowAnyHeader:
                        .AllowCredentials() //服务器必须允许凭据。 若要允许跨域凭据，调用AllowCredentials:
                        .AllowAnyOrigin(); //允许任何来源的主机访问
                });
            });
            #endregion

        }





        /// <summary>
        /// 启用配置的服务
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {

            #region 启用注册的自定义跨域服务
            //此处必须放在 UseMvc 之前，且策略名称必须是已经定义的。https://www.cnblogs.com/stulzq/p/9392150.html
            app.UseCors("AllowAllOrigin");
            #endregion

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();//启用静态文件访问wwwroot
            app.UseMvc();

            #region 启用路由
            //app.UseMvc(routes =>
            //{

            //    routes.MapRoute(
            //        name: "default",
            //        template: "{controller=Home}/{action=Index}/{id?}"
            //    );

            //    routes.MapRoute(
            //        name: "areaname",
            //        template: "{Admin:exists}/{controller=Home}/{action=Index}/{id?}"
            //    );

            //    routes.MapAreaRoute(
            //        name: "Admin",
            //        areaName: "Admin",
            //        template: "Admin/{controller=Home}/{action=Index}"
            //    );
            //}); 
            #endregion




        }
    }
}
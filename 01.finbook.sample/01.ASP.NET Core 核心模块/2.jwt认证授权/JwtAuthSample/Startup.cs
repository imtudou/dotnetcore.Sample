using System.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using JwtAuthSample.Models;
using Microsoft.IdentityModel.Tokens;   
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;




namespace JwtAuthSample
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
            //将appsettings.json中的JwtSettings部分文件读取到JwtSettings中，这是给其他地方用的
            services.Configure<JwtSeetings>(Configuration.GetSection("JwtSeetings"));

             //由于初始化的时候我们就需要用，所以使用Bind的方式读取配置
            //将配置绑定到JwtSettings实例中
            var jwtSettings = new JwtSeetings();
            Configuration.Bind("JwtSeetings",jwtSettings);

            //注入：JWT认证配置
            services.AddAuthentication(options=>{
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(o=>{

//                 //主要是jwt配置  token参数设置
//                 o.TokenValidationParameters = new TokenValidationParameters{
// 　　　　　　　　　　　　//Token颁发机构
//                     ValidIssuer =jwtSettings.Issuer,
// 　　　　　　　　　　　　//颁发给谁
//                     ValidAudience =jwtSettings.Audience,
//                     //这里的key要进行加密，需要引用 Microsoft.IdentityModel.Tokens
//                     IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey))
// 　　　　　　　　　　　　//ValidateIssuerSigningKey=true,
// 　　　　　　　　　　　　////是否验证Token有效期，使用当前时间与Token的Claims中的NotBefore和Expires对比
// 　　　　　　　　　　　　//ValidateLifetime=true,
// 　　　　　　　　　　　　////允许的服务器时间偏移量
// 　　　　　　　　　　　　//ClockSkew=TimeSpan.Zero

//                 };


                //自定义token取值
                o.SecurityTokenValidators.Clear();
                //修改token来源:默认是从请求头 context.Request.Headers["Authorization"] 中取
                //               现在可直接从 context.Request.Headers["token"] 取

                o.SecurityTokenValidators.Add(new CustomerTokenValidation());

                o.Events = new JwtBearerEvents(){
                    OnMessageReceived = context =>{
                        var token = context.Request.Headers["token"];
                        context.Token = token.FirstOrDefault();
                        return Task.CompletedTask;
                    }
                };
            });

            services.AddAuthorization(options =>{
                options.AddPolicy("SuperAdminOnly",policy=>policy.RequireClaim("SuperAdminOnly"));
            });





            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }


            //使用认证:注意添加的位置
            app.UseAuthentication();
            
            app.UseHttpsRedirection();
            app.UseRouting();
                       
            app.UseAuthorization();//使用授权

            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using FluentValidation.AspNetCore;
using FluentValidation.Sample.Validator;
using FluentValidation.Sample.ViewModel;
using System.Net;
using FluentValidation.Sample.Filter;

namespace FluentValidation.Sample
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
            services.AddTransient<IValidator<Person>, PersonValidator>();
            services.AddControllers()
                .AddFluentValidation();

            //全局注册过滤器
            services.AddControllersWithViews(options =>
            {
                options.Filters.Add(typeof(GlobalExceptionFilter));
            });

            //重写ModelState 的默认行为
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = (context) =>
                {
                    var errorMsg = context.ModelState
                    .Values
                    .SelectMany(s => s.Errors.Select(x => x.ErrorMessage))
                    .ToList();

                    var result = new
                    {
                        code = HttpStatusCode.BadRequest,
                        Msg = "Validation errors",
                        Errors = errorMsg
                    };
                    return new BadRequestObjectResult(result);
                };
            });
            

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

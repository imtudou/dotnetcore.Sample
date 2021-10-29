using FluentValidation.AspNetCore;
using FluentValidation.Sample.Filter;
using FluentValidation.Sample.Validator;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using System.Linq;
using System.Net;

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
            // example 1
            //services.AddTransient<IValidator<Person>, PersonValidator>();

            // examole 2
            services.AddControllers().AddFluentValidation(s => s.RegisterValidatorsFromAssemblyContaining<IBaseValidator>());

            services.AddControllers()
                .AddFluentValidation();

            //ȫ��ע�������
            services.AddControllersWithViews(options =>
            {
                options.Filters.Add(typeof(GlobalExceptionFilter));
            });

            //��дModelState ��Ĭ����Ϊ // �˴��ڹ�������У��Model.State Ҳ����
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

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using _09.ExceptionDemo.Exceptions;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace _09.ExceptionDemo
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
            services.AddMvc(options =>{
                options.Filters.Add<MyExceptionFilterAttribute>();
            }).AddJsonOptions(jsonOptions => { 
                jsonOptions.JsonSerializerOptions.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
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

            //app.UseExceptionHandler("/error");
            app.UseExceptionHandler(exception =>
            {
                exception.Run(async context =>
                {
                    var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                    var exFeature = exceptionHandlerPathFeature?.Error as IMyException;
                    if (exFeature == null)
                    {
                        var logger = context.RequestServices.GetService<ILogger<MyExceptionFilterAttribute>>();
                        logger.LogError(exceptionHandlerPathFeature.Error, exceptionHandlerPathFeature.Error.Message);
                        exFeature = MyException.Unknown;
                        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    }
                    else
                    {
                        context.Response.StatusCode = StatusCodes.Status200OK;
                    }
                    var jsonOptions = context.RequestServices.GetService<IOptions<JsonOptions>>();
                    context.Response.ContentType = "application/json; charset=utf-8";
                    await context.Response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(exFeature, jsonOptions.Value.JsonSerializerOptions));
                });
            });


            app.UseDirectoryBrowser();// ÆôÓÃµ±Ç°Â·¾¶ÏÂµÄÄ¿Â¼ä¯ÀÀ¹¦ÄÜ
            //UseStaticFiles
            app.UseStaticFiles();// é»˜è®¤è®¿é—®wwwrootæ–‡ä»¶å¤¹ä¸‹çš„é™æ€æ–‡ä»?

            // é…ç½®è®¿é—®æŒ‡å®šæ–‡ä»¶å¤¹ä¸‹çš„é™æ€æ–‡ä»?
            app.UseStaticFiles(new StaticFileOptions
            {
                RequestPath = "/file",
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "file"))
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

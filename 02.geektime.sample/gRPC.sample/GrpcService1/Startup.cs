namespace GrpcService1
{
    using GrpcService1.Services;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    using ProtoBuf.Grpc.Server;

    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCodeFirstGrpc();//注册启用了代码优先的服务。
            services.AddLogging(options => 
            {
                options.AddConsole();
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

            var log = app.ApplicationServices.GetService<ILoggerFactory>()?.CreateLogger(env.ApplicationName);
            log.LogInformation("Hello world");
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<OrderService>();//添加代码优先的服务终结点。
                //endpoints.MapGet("/", async context =>
                // {
                //    await context.Response.WriteAsync("Hello world");
                // });
            });
        }
    }
}

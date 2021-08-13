using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using zipkin4net;
using zipkin4net.Middleware;
using zipkin4net.Tracers.Zipkin;
using zipkin4net.Transport.Http;

namespace Sancksone.MiddlewareExtensions
{
    public static class ZipkinMidderwaExtension
    {
        public static void AddZipKin(this IServiceCollection service)
        { 
        
        }


        /// <summary>
        /// UseZipkin
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        /// <param name="loggerFactory"></param>
        /// <param name="lifetime"></param>
        public static void UseZipkin(this IApplicationBuilder app, IHostEnvironment env, ILoggerFactory loggerFactory, IHostApplicationLifetime lifetime)
        {
            lifetime.ApplicationStarted.Register(() => {
                TraceManager.SamplingRate = 1.0f;//记录数据密度，1.0代表全部记录
                var logger = new TracingLogger(loggerFactory, "zipkin4net");//内存数据
                var httpSender = new HttpZipkinSender("http://127.0.0.1:9411", "application/json");//zipkin服务器的地址及端口

                var trace = new ZipkinTracer(httpSender, new JSONSpanSerializer(), new Statistics());//注册zipkin
                var consoleTracer = new zipkin4net.Tracers.ConsoleTracer();//控制台输出

                TraceManager.RegisterTracer(trace);//注册
                TraceManager.RegisterTracer(consoleTracer);//控制台输出日志
                TraceManager.Start(logger);//放到内存中的数据
            });

            lifetime.ApplicationStopped.Register(() => TraceManager.Stop());
            app.UseTracing(env.ApplicationName);//此处名字可自定义，一般为服务的名字

        }
    }
}

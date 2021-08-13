using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLR.Sample1
{
    public class Sample6
    {

        // I/O限制的异步操作
        // Windows 如何执行I/O操作
        // C#异步函数
        // 编译器如何将异步函数转换成状态机
        // 异步函数扩展性
        // 异步函数和事件处理程序
        // FCL的异步函数
        // 异步函数和异常处理
        // 异步函数的其他功能
        // 应用程序及其线程处理模型
        // 取消 I/O操作
        // 有的 I/O 操作必须同步进行
        // I/O 请求优先级




        private static async Task<string> IssueClientRequestAsync(string serviceName, string message)
        {
            using (var pipe = new NamedPipeClientStream(serviceName, "pipeName", PipeDirection.InOut, PipeOptions.Asynchronous | PipeOptions.WriteThrough))
            {
                pipe.Connect(); // 必须在设置ReadMode 之前连接
                pipe.ReadMode = PipeTransmissionMode.Message;

                var request = Encoding.UTF8.GetBytes(message);
                await pipe.WriteAsync(request, 0, request.Length);

                // 异步读取
                var response = new byte[1000];
                var byteRead = await pipe.ReadAsync(response, 0, response.Length);

                return Encoding.UTF8.GetString(response, 0, byteRead);
            } 
        }
    }
}

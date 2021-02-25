using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetCore.IntefaceFramework.WebApi.Filter;
using Framework.Log;
using Framework.Common.ApiResponse;

namespace NetCore.IntefaceFramework.WebApi.Controllers
{
    [Route("api/[controller]/{actionName}")]
    [ApiController]
    public class OpenApiController : ControllerBase
    {

        [HttpGet,HttpPost]
        [OpenApiFilter]
        public string Open(string actionName)
        {

            var ret = new ApiResponse(); 
            string guid = Guid.NewGuid().ToString();
            //NLogHelper.Info(guid, "------开始------", "OpenApiController");
            try
            {
                string retstr = RunMethod(actionName);

            }
            catch (Exception ex)
            {
                NLogHelper.Info(guid, "异常："+ex.Message, "OpenApiController");
                ret.Head = new Header(false, 500, ex.Message);
            }
            //NLogHelper.Info(guid, "------结束------", "OpenApiController");


            return ret.ToString();
        }
 

        public static string RunMethod(string actionName)
        {
            string Result = string.Empty;
            string path = Directory.GetCurrentDirectory() + "\\Assembly";//获取应用程序的当期工作目录
            string namespaceName = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Namespace;//获取命名空间
            //path = path.Substring(0, path.LastIndexOf(namespaceName) + namespaceName.Length) + "\\assembly";//获取需要加载的.cs 文件
            var files = Directory.GetFiles(path, "*.cs");//获取文件夹下所有的文件

            foreach (var item in files)
            {
                string fileName = Path.GetFileNameWithoutExtension(item);
                if (fileName == actionName)
                {
                    Type t = Type.GetType("NetCore.IntefaceFramework.WebApi.assembly." + fileName);//反射获取类型(命名空间)
                    object obj = Activator.CreateInstance(t);//实例化对象
                    //MethodInfo methodInfo = t.GetMethod("GetOrderInfo");//获取方法
                    //Result += methodInfo.Invoke(obj, null).ToString();//调用方法(无参)  

                    MethodInfo methodInfos = t.GetMethod("SaveData");//获取方法
                    Result += methodInfos.Invoke(obj, new object[] { null }).ToString();//调用方法(有参数) 
                }


            }

            return Result;
        }
    }
} 
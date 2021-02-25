using Framework.Common.ApiResponse;
using Framework.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CSharp;
using Microsoft.Extensions.Configuration;
using NetCore.IntefaceFramework.WebApi.CommonService;
using NetCore.IntefaceFramework.WebApi.CommonService.Helper;
using NetCore.IntefaceFramework.WebApi.CommonService.Model;
using Newtonsoft.Json;
using System.CodeDom.Compiler;
using System.Data.SqlClient;
using System.Reflection;

namespace NetCore.IntefaceFramework.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]    
    [ApiController]
    public class GenerateApiController : ControllerBase
    {
        public IConfiguration Configuration { get; }
        public System.Runtime.Loader.AssemblyLoadContext _assemblyLoadContext { get; set; }

        public GenerateApiController(IConfiguration configuration, System.Runtime.Loader.AssemblyLoadContext AssemblyLoadContext)
        {
            Configuration = configuration;
            _assemblyLoadContext = AssemblyLoadContext;
        }

        [HttpPost]
        public ApiResponse SaveRequestDataPost(RequestModel request)
        {
            var ret = new ApiResponse();
            string connstr = Configuration.GetSection("ConnectionString:DefaultConnStr:ConnStr").Value;
            var dbHelper = new DbHelper(SqlClientFactory.Instance, connstr);

            try
            {
                string path = string.Empty;
                #region 1.生成前端配置的json文件
                path = CommonHelper.JsonDataFolder + request.ApiName + ".json";
                string jsonPath = CommonHelper.SaveData(path, JsonConvert.SerializeObject(request)).Result.ToString();
                #endregion


                #region 2.读取前端配置的json文件并生成cs类
                string jsonContents = CommonHelper.ReadAllText(jsonPath);
                var requestModel = JsonConvert.DeserializeObject<RequestModel>(jsonContents);

                T4Template.Mangers.T4Template template = new T4Template.Mangers.T4Template
                {
                    MetaData = requestModel,
                    Model = ""
                };
                string contents = template.TransformText();
                path = string.Empty;
                path = CommonHelper.AssemblyFolder + requestModel.ApiName + ".cs";
                string assemblyPath = CommonHelper.SaveData(path, contents).Result;
                #endregion


                #region 3.加载程序集
                string loadpath = System.AppDomain.CurrentDomain.BaseDirectory + "\\NetCore.IntefaceFramework.WebApi.dll";
                _assemblyLoadContext.LoadFromAssemblyPath(loadpath);
            
                #endregion


                #region 3.动态编译
                //// 1.CSharpCodePrivoder
                //CSharpCodeProvider objCSharpCodePrivoder = new CSharpCodeProvider();

                //// 2.ICodeComplier
                //ICodeCompiler objICodeCompiler = objCSharpCodePrivoder.CreateCompiler();

                //// 3.CompilerParameters
                //CompilerParameters objCompilerParameters = new CompilerParameters();
                //objCompilerParameters.ReferencedAssemblies.Add("System.dll");
                //objCompilerParameters.GenerateExecutable = false;
                //objCompilerParameters.GenerateInMemory = true;

                //// 4.CompilerResults
                //CompilerResults cr = objICodeCompiler.CompileAssemblyFromSource(objCompilerParameters, contents); 
                //if (cr.Errors.HasErrors)
                //{
                //    foreach (CompilerError err in cr.Errors)
                //    {

                //    }
                //}
                //else
                //{
                //    // 通过反射，调用HelloWorld的实例
                //    Assembly objAssembly = cr.CompiledAssembly;
                //    object objHelloWorld = objAssembly.CreateInstance("DynamicCodeGenerate.HelloWorld");
                //    MethodInfo objMI = objHelloWorld.GetType().GetMethod("OutPut");
                //    objMI.Invoke(objHelloWorld, null);

                //}
                #endregion





            }
            catch (System.Exception ex) 
            {

                throw ex;
            }
            







            return ret;
        }
    }
}
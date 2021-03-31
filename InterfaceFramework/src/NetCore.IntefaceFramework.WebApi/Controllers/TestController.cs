using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetCore.IntefaceFramework.WebApi.CommonService;
using NetCore.IntefaceFramework.WebApi.Filter;

namespace NetCore.IntefaceFramework.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        public List<Dictionary<string, string>> list = new List<Dictionary<string, string>>()
            {
                new Dictionary<string, string>(){
                    {"id",Guid.NewGuid().ToString() },
                    {"name","奔驰" },
                    {"createtime",DateTime.Now.ToString() }
                },
                new Dictionary<string, string>(){
                    {"id",Guid.NewGuid().ToString() },
                    {"name","宝马" },
                    {"createtime",DateTime.Now.ToString() }
                },new Dictionary<string, string>(){
                    {"id",Guid.NewGuid().ToString() },
                    {"name","长安奔奔" },
                    {"createtime",DateTime.Now.ToString() }
                },
            };




        [HttpPost]
        public string GetData(dynamic dynamic)
        {

            string username = string.Empty, passwd = string.Empty;
            JsEncryptHelper jsHelper = new JsEncryptHelper();

            username = jsHelper.Decrypt(dynamic.username.ToString());
            passwd = jsHelper.Decrypt(dynamic.passwd.ToString());

            Dictionary<string, string> dic = new Dictionary<string, string>() { };
            dic.Add("username", username);
            dic.Add("passwd", passwd);

            return "成功";
        }


        [HttpGet]
        public string GetData()
        {
      
            var ret = Newtonsoft.Json.JsonConvert.SerializeObject(list);
            return ret;

        }

        [HttpPost]
        public string PostData(dynamic dy)
        {
            if (!string.IsNullOrEmpty(dy.id.ToString()))
            {
                list.Add(new Dictionary<string, string> {
                    { "id", dy["id"].ToString()}
                });

            }

            var ret = Newtonsoft.Json.JsonConvert.SerializeObject(list);
            return ret;

        }


        [HttpPost]
        public string OptionsData(dynamic dy)
        {
            switch (dy["options"].ToString())
            {
                case "add":
                    list.Add(new Dictionary<string, string> {
                     {"id",Guid.NewGuid().ToString() },
                    {"name",dy.name.ToString() },
                    {"createtime",dy.name.ToString() }
                });

                    break;
                case "update":


                    break;
                case "delete":

                    break;
            }

         

            var ret = Newtonsoft.Json.JsonConvert.SerializeObject(list);
            return ret;

        }

    }




    
}
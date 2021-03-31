using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using NetCore.IntefaceFramework.WebApi.Filter;

namespace Net.InterfaceFramework.WebApi.Controllers
{

    public class OpenApiController : Controller
    {

        [System.Web.Http.HttpGet, System.Web.Http.HttpPost]
        [OpenApiAttribute]
        public string Open(string actionName)
        {
            return "Request Success!";

        }





        public string Index()
        {

            return "Request Success 123456!";
        }
             

    }
}

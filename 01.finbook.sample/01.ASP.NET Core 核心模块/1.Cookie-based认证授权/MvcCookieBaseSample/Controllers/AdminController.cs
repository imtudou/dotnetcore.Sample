using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MvcCookieBaseSample.Models;


using Microsoft.AspNetCore.Authorization;

namespace MvcCookieBaseSample.Controllers
{

    
    [Authorize]
    public class AdminController : Controller
    {
        
        public IActionResult Index()
        {
            return View();
        }
 

        
    }
}

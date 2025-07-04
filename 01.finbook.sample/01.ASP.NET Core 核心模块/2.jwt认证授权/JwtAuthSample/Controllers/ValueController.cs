using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;

namespace JwtAuthSample.Controllers
{

    [Authorize(Policy = "SuperAdminOnly")]
    [ApiController]
    [Route("api/[controller]/[action]")]
    
    public class ValueController:ControllerBase
    {

        [HttpGet]
        public string Get()
        {

            return "values";
        }
        
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation.Sample.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FluentValidation.Sample.Controllers
{
    [FluentValidation.Sample.Filter.ActionFilter]
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        [HttpPost]
        public IActionResult Post([FromBody] Person person)
        {
            return Ok(person);
        }
    }

}

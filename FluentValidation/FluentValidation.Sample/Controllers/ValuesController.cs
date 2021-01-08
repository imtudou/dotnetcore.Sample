using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation.Sample.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text.Json;

namespace FluentValidation.Sample.Controllers
{
    [FluentValidation.Sample.Filter.ActionFilter]
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        public Func<UserInfoEntity,string> GenterStr = s => JsonSerializer.Serialize(s);

        [HttpPost]
        public IActionResult Post([FromBody] Person person)
        {
            var userEntity = new UserInfoEntity();
            if (person != null)
            {

            }
            return Ok(person);
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok();
        }
    }

    public class UserInfoEntity 
    {
        public string UserId { get; set; } 
        public string UserName { get; set; } 
        public string UserEmail { get; set; } 
        public string UserTitle { get; set; } 
        public int UserAge  { get; set; }
        public int UserGender  { get; set; }
        public string Address { get; set; }
    }

    public class Address
    {
        public string Country { get; set; }

        public string Province { get; set; }
        public string City { get; set; }
    }

}

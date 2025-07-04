using System.Net;
using System.Dynamic;
using System.Security.Claims;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using JwtAuthSample.LoginViewModels;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Options;
using System.Text;
using JwtAuthSample.Models;
using Microsoft.AspNetCore.Authorization;


namespace JwtAuthSample.Controllers
{


    [ApiController]
    [Route("api/[controller]/{action}")]
    public class AuthorizeController:ControllerBase
    {

        private JwtSeetings  _jwtSeetings;
        public AuthorizeController(IOptions<JwtSeetings>  JwtSeetings){
            _jwtSeetings = JwtSeetings.Value;
        }


        [HttpGet]
        [HttpPost]
        public IActionResult Token(LoginViewModel models)
        {

            if(ModelState.IsValid)
            {

                if(!(models.UserName == "test" && models.UserPwd == "123"))
                   return BadRequest();

                var claims = new Claim[]{
                    new Claim(ClaimTypes.Role,"user"),
                    new Claim(ClaimTypes.Name,"test"),
                    new Claim("SuperAdminOnly","true")
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSeetings.SecretKey));
                var creds = new SigningCredentials(key,SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(_jwtSeetings.Issuer
                    ,_jwtSeetings.Audience
                    ,claims
                    ,DateTime.Now
                    ,DateTime.Now.AddMinutes(30)
                    ,creds
                );
                return Ok(new {token = new JwtSecurityTokenHandler().WriteToken(token)});



                
                
                

            }

            return BadRequest();


        }




    }


}
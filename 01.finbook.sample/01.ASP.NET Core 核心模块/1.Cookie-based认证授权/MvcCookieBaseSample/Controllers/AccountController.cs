using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MvcCookieBaseSample.Models;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace MvcCookieBaseSample.Controllers
{
    public class AccountController:Controller
    {
        public IActionResult LoginIn()
        {
            var cliams = new List<Claim>(){
                new Claim(ClaimTypes.Name,"zhangsan"),
                new Claim(ClaimTypes.Role,"admin")
            };

            var cliamIdentity = new ClaimsIdentity(cliams,CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,new ClaimsPrincipal(cliamIdentity));
            return Ok();
        }


        public IActionResult LoginOut()
        {   
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok();

        }
    }
}
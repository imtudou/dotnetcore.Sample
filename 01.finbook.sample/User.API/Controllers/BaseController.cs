using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using User.API.Entity.Dtos;

namespace User.API.Controllers
{
    public class BaseController : Controller
    {
        protected UserIdentity UserIdentity 
        {
            get
            {
                if (User.Claims.ToList().Count > 0)
                {
                    var cc = User.Claims;
                    var identity = new UserIdentity
                    {
                        UserId = Convert.ToInt32(User.Claims.FirstOrDefault(s => s.Type.Equals(nameof(UserIdentity.UserId))).Value),
                        Name = User.Claims.FirstOrDefault(s => s.Type.Equals(nameof(UserIdentity.Name))).Value,
                        Company = User.Claims.FirstOrDefault(s => s.Type.Equals(nameof(UserIdentity.Company))).Value,
                        Avatar = User.Claims.FirstOrDefault(s => s.Type.Equals(nameof(UserIdentity.Avatar))).Value,
                        Title = User.Claims.FirstOrDefault(s => s.Type.Equals(nameof(UserIdentity.Title))).Value,
                        Phone = User.Claims.FirstOrDefault(s => s.Type.Equals(nameof(UserIdentity.Phone))).Value
                    };
                    return identity;
                }
                else
                {
                    return new UserIdentity();
                }

            }


        }


    }
}

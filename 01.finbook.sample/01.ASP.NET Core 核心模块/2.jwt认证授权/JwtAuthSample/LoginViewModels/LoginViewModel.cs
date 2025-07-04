using System.Reflection.Metadata;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;



namespace JwtAuthSample.LoginViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "UserName 必填！")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "UserPwd必填！")]
        public string UserPwd { get; set; }
    }
}
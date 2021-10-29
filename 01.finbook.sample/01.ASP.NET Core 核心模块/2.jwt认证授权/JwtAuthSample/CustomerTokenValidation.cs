using System.Runtime.InteropServices;
using System.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace JwtAuthSample
{
    public class CustomerTokenValidation : ISecurityTokenValidator
    {
        public bool CanValidateToken => true;

        public int MaximumTokenSizeInBytes { get; set; }

        public bool CanReadToken(string securityToken)
        {
            return true;
        }

        public ClaimsPrincipal ValidateToken(string securityToken, TokenValidationParameters validationParameters, out SecurityToken validatedToken)
        {
            validatedToken = null;
            if(securityToken != "abcdefg")
                return null;

            var identity = new ClaimsIdentity(JwtBearerDefaults.AuthenticationScheme);
            identity.AddClaim(new Claim("name","test"));
            identity.AddClaim(new Claim("SuperAdminOnly","true"));
            identity.AddClaim(new Claim(ClaimsIdentity.DefaultRoleClaimType,"user"));
            var claimsPrincipal = new ClaimsPrincipal(identity);
            return claimsPrincipal; 
        }
    }


}



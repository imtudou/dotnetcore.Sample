using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Validation;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

using User.Identity.Services;

namespace User.Identity.Authentication
{
    public class SmsAuthCodeValidator : IExtensionGrantValidator
    {
        public string GrantType => "sms_auth_code";

        public readonly IAuthCodeService _authCodeService;
        public readonly IUserService _userService;

        public SmsAuthCodeValidator(IAuthCodeService authCodeService, IUserService userService) {
            _authCodeService = authCodeService;
            _userService = userService;
        }

        public async Task ValidateAsync(ExtensionGrantValidationContext context)
        {
            var phone = context.Request.Raw["phone"];
            var authcode = context.Request.Raw["auth_code"];
            var errorValidateResult = new GrantValidationResult(TokenRequestErrors.InvalidGrant);

            if (string.IsNullOrEmpty(phone) || string.IsNullOrEmpty(authcode))
            {
                context.Result = errorValidateResult;
                return;
            }

            //校验验证码
            if (!_authCodeService.Validate(phone,authcode))
            {
                context.Result = errorValidateResult;
                return;
            }

            //校验手机号是否存在，    
            var userInfoDto = await _userService.CheckOrCreateAsync(phone);
            if (userInfoDto == null)
            {
                context.Result = errorValidateResult;
                return;
            }

            var cliams = new List<Claim> {
                   new Claim("UserId",$"{userInfoDto?.Id}"),
                   new Claim(nameof(userInfoDto.Name),$"{userInfoDto?.Name}"),
                   new Claim(nameof(userInfoDto.Company),$"{userInfoDto?.Company}"),
                   new Claim(nameof(userInfoDto.Title),$"{userInfoDto?.Title}"),
                   new Claim(nameof(userInfoDto.Phone),$"{userInfoDto?.Phone}"),
                   new Claim(nameof(userInfoDto.Avatar),$"{userInfoDto?.Avatar}"),
            };

            context.Result =  new GrantValidationResult(userInfoDto.Id.ToString(),GrantType, cliams);

        }
    }
}

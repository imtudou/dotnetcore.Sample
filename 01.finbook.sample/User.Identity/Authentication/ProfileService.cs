using IdentityServer4.Models;
using IdentityServer4.Services;

using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace User.Identity.Authentication
{
    /// <summary>
    /// 获取Claim
    /// </summary>
    public class ProfileService : IProfileService
    {

        private readonly ILogger<ProfileService> _logger;
        public ProfileService(ILogger<ProfileService> logger)
        {
            _logger = logger;
        }
        public Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var subject = context.Subject ?? throw new ArgumentNullException(nameof(context.Subject));
            var subjectId = subject.Claims.Where(s => s.Type == "sub").FirstOrDefault().Value;


            if (!int.TryParse(subjectId, out int userId))
            {
                _logger.LogError("subjectId tryparse exception!");
                throw new ArgumentException("subjectId tryparse exception!");
            }
                

            context.IssuedClaims = context.Subject.Claims.ToList();
            return Task.CompletedTask;
        }

        public Task IsActiveAsync(IsActiveContext context)
        {
            var subject = context.Subject ?? throw new ArgumentNullException(nameof(context.Subject));

            var subjectId = subject.Claims.Where(s => s.Type == "sub").FirstOrDefault().Value;

            context.IsActive = int.TryParse(subjectId, out int userId);

            return Task.CompletedTask;
        }
    }
}

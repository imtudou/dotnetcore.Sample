using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Routing;

namespace IdentityServerSample.Center
{
    public static class Config
    {


        /// <summary>
        /// 身份资源
        /// </summary>
        public static IEnumerable<IdentityResource> Identities =>
            new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };




        /// <summary>
        /// API 资源
        /// </summary>
        public static IEnumerable<ApiResource> ApiResources =>
            new List<ApiResource>
            {
                new ApiResource("api","MY API")
            };


        public static IEnumerable<Client> Clients =>
            new List<Client>
            {
                //1.客户端模式
                new Client{ 
                    ClientId = "client",
                    ClientSecrets = { new Secret("secret".Sha256())},
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes = { "api"}
                },
                //

            };
        

    }
}

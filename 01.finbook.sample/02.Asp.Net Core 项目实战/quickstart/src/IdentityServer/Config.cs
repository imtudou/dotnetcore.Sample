// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace IdentityServer
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> Ids =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };

        public static IEnumerable<ApiResource> ApiResources =>
            new List<ApiResource>
            {
                 new ApiResource("api1","MY API")
            };

        /// <summary>
        /// 
        /// </summary>
        public static IEnumerable<Client> Clients =>
            new List<Client>
            {

                new Client{ 
                
                    ClientId = "client",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets = { new Secret("secret".Sha256())},
                    AllowedScopes = { "api1"}
                },
                new Client{
                    ClientId = "mvc",
                    ClientName = "mvc client test",
                    ClientUri = "http://localhost:5000",
                    LogoUri = "https://ss2.bdstatic.com/70cFvnSh_Q1YnxGkpoWK1HF6hhy/it/u=3812495358,3529254906&fm=15&gp=0.jpg",
                    ClientSecrets = { new Secret("secret".Sha256())},
                        
                    AllowedGrantTypes = GrantTypes.Code,
                    RequireConsent = true,
                    RequirePkce = true,
                    AllowRememberConsent = true,

                     // where to redirect to after login
                    RedirectUris = { "http://localhost:5002/signin-oidc" },

                     // where to redirect to after logout
                    PostLogoutRedirectUris = { "http://localhost:5002/signout-callback-oidc" },
                    


                     //允许的作用域
                     AllowedScopes = new List<string>
                     {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "api1"
                     },

                     //启用对刷新令牌的支持
                     AllowOfflineAccess = true
                     


                }

            };
        




    }
}
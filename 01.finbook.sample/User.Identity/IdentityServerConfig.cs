using IdentityServer4;
using IdentityServer4.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace User.Identity
{

    public class ABC<T> where T : class,new()
    {
        
    }
    public  class IdentityServerConfig
    {
        
        /// <summary>
        /// Client
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Client> GetClients() 
        {
            return new List<Client>
            {
                #region MyRegion
		        //new Client{
                //    ClientId = "iphone",
                //    ClientSecrets = new List<Secret>
                //    {
                //        new Secret("secret".Sha256())
                //    },
                //    RefreshTokenExpiration = TokenExpiration.Sliding,
                //    AllowOfflineAccess = true,
                //    RequireClientSecret = false,
                //    AllowedGrantTypes = new List<string>{ "sms_auth_code"},
                //    AlwaysIncludeUserClaimsInIdToken = true,
                //    AllowedScopes = new List<string>{
                //        "finbook_api",
                //        IdentityServerConstants.StandardScopes.OpenId,
                //        IdentityServerConstants.StandardScopes.Profile,
                //        IdentityServerConstants.StandardScopes.OfflineAccess
                //    }

                //}

                //new Client{ 
                //    ClientId = "android",
                //    ClientSecrets = new List<Secret>{ new Secret("secret".Sha256())},
                //    AllowedGrantTypes = new List<string>{ "sms_auth_code" },
                //    AllowedScopes = new List<string> 
                //    {
                //        "user_api",
                //        IdentityServerConstants.StandardScopes.OpenId,
                //        IdentityServerConstants.StandardScopes.Profile,
                //        IdentityServerConstants.StandardScopes.OfflineAccess
                //    },
                //    AllowOfflineAccess = true
                //}, 
	#endregion

                //gateway_api client
                new Client{
                    ClientId = "android",
                    ClientSecrets = new List<Secret>{ new Secret("secret".Sha256())},
                    AllowedGrantTypes = new List<string>{ "sms_auth_code" },
                    AllowedScopes = new List<string>
                    {
                        "gateway_api.scope",//此处对应的是ApiScopes 中的配置
                        "user_api.scope",
                        "contact_api.scope",
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.OfflineAccess
                    },
                    AllowOfflineAccess = true
                },

                //user_api client
                new Client{
                    ClientId = "user_api.ClientId",
                    ClientSecrets = new List<Secret>{ new Secret("secret".Sha256())},
                    AllowedGrantTypes = new List<string>{ "sms_auth_code" },
                    AllowedScopes = new List<string>
                    {
                        "gateway_api.scope",//此处对应的是ApiScopes 中的配置
                        "user_api.scope",//此处对应的是ApiScopes 中的配置
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.OfflineAccess
                    },
                    AllowOfflineAccess = true
                },
                
                //contact_api client
                new Client{
                    ClientId = "contact_api.ClientId",
                    ClientSecrets = new List<Secret>{ new Secret("secret".Sha256())},
                    AllowedGrantTypes = new List<string>{ "sms_auth_code" },
                    AllowedScopes = new List<string>
                    {
                        "gateway_api.scope",//此处对应的是ApiScopes 中的配置
                        "contact_api.scope",//此处对应的是ApiScopes 中的配置
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.OfflineAccess
                    },
                    AllowOfflineAccess = true
                }


            };
        }


        /// <summary>
        /// Identity Resources
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<IdentityResource> GetIdentityResources() 
        {
            return new List<IdentityResource> {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };  
        }


        /// <summary>
        /// API Resource
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource> {

                //Gateway.API
                new ApiResource("gateway_api","gateway api")
                {
                    Scopes = { "gateway_api.scope" }
                },
                //User.API
                new ApiResource("user_api","user api")
                {
                    Scopes = { "user_api.scope" }
                },
                //Contact API
                new ApiResource("contact_api","contact api")
                {
                    Scopes = {"contact_api.scope" }
                }
            };
        
        }


        /// <summary>
        /// ApiScope
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<ApiScope> GetApiScopes() 
        {
            return new List<ApiScope> {
                 //user_api scope
                new ApiScope("gateway_api.scope","gateway_api  scope"),
                //user_api scope
                new ApiScope("user_api.scope","user_api  scope"),
                //contact_api scope
                new ApiScope("contact_api.scope","contact_api  scope")

            };
            
        }

      
        
       

       
    }

  
}

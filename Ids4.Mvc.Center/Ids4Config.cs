﻿using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Ids4.Mvc.Center
{
    public class Ids4Config
    {
        /// <summary>
        /// OAuth2.0 定义需要被授权访问的API资源
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("user-api","用户API接口"),
                new ApiResource("project-api","项目API接口"),
            };
        }

        /// <summary>
        /// 定义可以访问API资源的客户端
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "console_client",
                    ClientSecrets =
                    {
                        new Secret("console_secret".Sha256())
                    },

                    // 允许授予类型
                    // GrantType.ClientCredentials（客户端模式），没有用户参与交互，直接通过客户端Id和secret进行授权
                    AllowedGrantTypes = { GrantType.ClientCredentials, GrantType.ResourceOwnerPassword },
                    // 该客户端可以访问的资源集合
                    AllowedScopes = { "user-api" }
                },

                new Client
                {
                    ClientId = "mvc_client",
                    ClientName = "UserMvcClient",
                    AllowedGrantTypes = GrantTypes.Implicit,

                    // MVC客户端登录成功之后的跳转地址
                    RedirectUris = { "http://localhost:5003/signin-oidc" },
                    // MVC客户端登出之后的跳转地址
                    PostLogoutRedirectUris = { "http://localhost:5003/signout-callback-oidc" },
                    RequireConsent = false,
                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email
                    }
                }
            };
        }

        /// <summary>
        /// OpenID Connect 定义需要被授权的用户身份数据
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email()
            };
        }

        public static List<TestUser> GetTestUsers()
        {
            return new List<TestUser>
            {
                new TestUser
                {
                    SubjectId = "1",
                    Username = "张三",
                    Password = "123456",
                    Claims = new List<Claim>
                    {
                        new Claim(JwtClaimTypes.Email,"123241@qq.com"),
                        new Claim(JwtClaimTypes.Address,"中国深圳")
                    }
                }
            };
        }
    }
}

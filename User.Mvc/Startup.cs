﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace User.Mvc
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            JwtSecurityTokenHandler.DefaultOutboundClaimTypeMap.Clear();
            services.AddAuthentication(options =>
            {
                // 通过Cookies来认证用户
                options.DefaultScheme = "Cookies";
                // 通过oidc使用户登录
                options.DefaultChallengeScheme = "oidc";
            })
            .AddCookie("Cookies")   // 添加处理Cookies的Handler

            // 只能认证用户，不能访问UserApi
            //.AddOpenIdConnect("oidc", options =>    // 添加处理oidc的Handler
            // {
            //     // 当oidc handler完成认证过程之后，通过Cookies Handler发行Cookies
            //     options.SignInScheme = "Cookies";

            //     options.Authority = "http://localhost:5000";
            //     options.RequireHttpsMetadata = false;

            //     options.ClientId = "mvc_client";
            //     options.SaveTokens = true;
            // })

            .AddOpenIdConnect("oidc", options =>    // 添加处理oidc的Handler
            {
                // 当oidc handler完成认证过程之后，通过Cookies Handler发行Cookies
                options.SignInScheme = "Cookies";

                options.Authority = "http://localhost:5000";
                options.RequireHttpsMetadata = false;

                options.ClientId = "mvc_client2";
                // 请求AccessToken时需要使用密钥
                options.ClientSecret = "mvc_secret2";
                // 
                options.ResponseType = "code id_token";

                options.SaveTokens = true;

                //在拿到id_token之后自动向userinfo endpoint请求用户信息并放到asp.net core的User Identity下
                options.GetClaimsFromUserInfoEndpoint = true;

                options.Scope.Add("user-api");
                options.Scope.Add("offline_access");
            })
             ;


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}

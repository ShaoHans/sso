using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4;
using IdentityServer4.Test;
using Ids4.Mvc.Center.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace Ids4.Mvc.Center.Controllers
{
    public class AccountController : Controller
    {
        private readonly TestUserStore _userStore;

        public AccountController(TestUserStore userStore)
        {
            _userStore = userStore;
        }

        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            LoginViewModel loginViewModel = new LoginViewModel();
            loginViewModel.ReturnUrl = returnUrl;
            return View(loginViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if(ModelState.IsValid)
            {
                if(_userStore.ValidateCredentials(loginViewModel.UserName,loginViewModel.Password))
                {
                    TestUser user = _userStore.FindByUsername(loginViewModel.UserName);
                    AuthenticationProperties authProps = new AuthenticationProperties
                    {
                        IsPersistent = true,
                        ExpiresUtc = DateTimeOffset.UtcNow.Add(TimeSpan.FromDays(1))
                    };

                    await AuthenticationManagerExtensions.SignInAsync(HttpContext, user.SubjectId, user.Username, authProps);

                    if (Url.IsLocalUrl(loginViewModel.ReturnUrl))
                    {
                        return Redirect(loginViewModel.ReturnUrl);
                    }
                    else if (string.IsNullOrEmpty(loginViewModel.ReturnUrl))
                    {
                        return Redirect("~/");
                    }
                    else
                    {
                        throw new Exception("无效的ReturnUrl");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "无效的用户名或密码");
                }
            }
            return View(loginViewModel);
        }

        public IActionResult Logout()
        {
            return View();
        }
    }
}
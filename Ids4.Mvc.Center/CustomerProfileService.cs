using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ids4.Mvc.Center
{
    public class CustomerProfileService : IProfileService
    {
        private readonly TestUserStore _userStore;

        public CustomerProfileService(TestUserStore userStore)
        {
            _userStore = userStore;
        }

        /// <summary>
        /// 只要有关用户的身份信息单元被请求（例如在令牌创建期间或通过用户信息终点），就会调用此方法
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var user = _userStore.FindBySubjectId(context.Subject.GetSubjectId());
            if (user != null)
            {
                context.IssuedClaims.AddRange(user.Claims);
            }
            return Task.CompletedTask;
        }


        /// <summary>
        /// 验证用户是否有效 例如：token创建或者验证
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public Task IsActiveAsync(IsActiveContext context)
        {
            var user = _userStore.FindBySubjectId(context.Subject.GetSubjectId());
            context.IsActive = user?.IsActive == true;
            return Task.CompletedTask;
        }
    }
}

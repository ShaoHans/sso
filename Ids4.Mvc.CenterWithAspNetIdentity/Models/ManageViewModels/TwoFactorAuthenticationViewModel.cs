using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ids4.Mvc.CenterWithAspNetIdentity.Models.ManageViewModels
{
    public class TwoFactorAuthenticationViewModel
    {
        public bool HasAuthenticator { get; set; }

        public int RecoveryCodesLeft { get; set; }

        public bool Is2faEnabled { get; set; }
    }
}

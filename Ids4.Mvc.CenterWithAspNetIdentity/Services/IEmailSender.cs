using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ids4.Mvc.CenterWithAspNetIdentity.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}

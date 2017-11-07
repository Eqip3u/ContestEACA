using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using ContestEACA.Services;

namespace ContestEACA.Services
{
    public static class EmailSenderExtensions
    {
        public static Task SendEmailConfirmationAsync(this IEmailSender emailSender, string email, string link)
        {
            return emailSender.SendEmailAsync(email, "Confirm your email",
                $"Please confirm your account by clicking this link: <a href='{link}'>link</a>");
        }

        //TODO: —делать две третий аргумент, чтобы на почту приходили разные письма от статуса модерации 
        public static Task SendEmailModerateStatusPost(this IEmailSender emailSender, string email)
        {
            return emailSender.SendEmailAsync(email, "¬аша прошла модерацию",
                $"«айдите на сайт чтобы посмотреть результат");
        }
    }
}

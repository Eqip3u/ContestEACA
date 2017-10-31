using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;

namespace ContestEACA.Services
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {
            var client = new SmtpClient("smtp.yandex.ru");
            client.Port = 25;
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential("contesteaca", "fdrGfv23Tds12Hbvd");
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.DeliveryFormat = SmtpDeliveryFormat.SevenBit;

            var emailMessage = new MailMessage();

            emailMessage.From = new MailAddress("contesteaca@yandex.ru");
            emailMessage.To.Add(email);
            emailMessage.Subject = subject;
            emailMessage.Body = message;

            client.Send(emailMessage);

            return Task.CompletedTask;
        }
    }
}

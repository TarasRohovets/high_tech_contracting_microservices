using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DC.Business.Consumer.Email.SendGrid
{
    public class SendGridService
    {
        public SendGridService() { }

        public async Task Execute()
        {
            var apiKey = Environment.GetEnvironmentVariable("SendGrid.ApiKey");
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("", "Prince");
            var subject = "yooooooooooo wassssappppp";
            var to = new EmailAddress("", "Princess");
            var plainTextContent = "lake tomorrow?";
            var htmlContent = "<strong>lake tomorrow???</strong>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);

            // TODO add using !!!!!!!!! 
            var response = await client.SendEmailAsync(msg);
        }
    }
}

using System.Threading.Tasks;
using System.Web.Configuration;
using Newtonsoft.Json;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace ministryofjusticeDomain.Services
{
    public class SendGridService
    {
        public async Task SendEmailAsync(string receiverEmail, string receiverName, string message, string subject, string link=null)
        {
            // Gets SendGrid API key from web config file
            var apiKey = WebConfigurationManager.AppSettings["SendGridKey"];
            var templateId = WebConfigurationManager.AppSettings["TemplateId"];

            //Creates SendGrid email client
            var emailClient = new SendGridClient(apiKey);

            //Creates mail response object
            var mailResponse = new SendGridMessage();

            // Declares sender email and name
            mailResponse.SetFrom("emekaallison4@gmail.com", "Ministry of Justice");

            // Declares the reciever's email and name
            mailResponse.AddTo(receiverEmail, receiverName);

            // Sets email subject
            mailResponse.SetSubject(subject);

            //directs API to dynamic template created in SendGrid dashboard
            mailResponse.SetTemplateId(templateId);
            // Appends the dynamic email contents
            mailResponse.SetTemplateData(new EmailContent
            {
                Name = receiverName,
                Message = message,
                Link = link,
                Subject = subject,
            });

            await emailClient.SendEmailAsync(mailResponse);
        }

        private class EmailContent
        {
            [JsonProperty("name")]
            public string Name { get; set; }
            [JsonProperty("message")]
            public string Message { get; set; }
            [JsonProperty("link")]
            public string Link { get; set; }
            [JsonProperty("subject")]
            public string Subject { get; set; }
        }
    }
}

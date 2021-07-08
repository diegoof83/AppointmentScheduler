using Mailjet.Client;
using Mailjet.Client.Resources;
using Microsoft.AspNetCore.Identity.UI.Services;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;

namespace AppointmentScheduler.Utility
{
    public class EmailSender : IEmailSender
    {
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            MailjetClient client = new("d5204d287f79ea12d4d5117ba7f9484c", "0e36c04e699d1ef9b5f877bcf14ea788"); //TODO Configure this 2 keys into the appsettings

            MailjetRequest request = new MailjetRequest
            {
                Resource = Send.Resource,
            }
            .Property(Send.FromEmail, Helper.EmailFromAddress)
            .Property(Send.FromName, Helper.EmailFromName)
            .Property(Send.Subject, subject)
            //.Property(Send.TextPart, "Dear passenger, welcome to Mailjet! May the delivery force be with you!")
            .Property(Send.HtmlPart, htmlMessage)
            .Property(Send.Recipients, new JArray {
                new JObject {
                    {"Email", email}
                }
            });
            MailjetResponse response = await client.PostAsync(request);
        }
    }
}

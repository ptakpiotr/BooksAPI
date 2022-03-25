using FluentEmail.Core;
using FluentEmail.Smtp;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net.Mail;

namespace WebAPI.Data
{
    public class FluentEmailSender : IEmailSender
    {
        public FluentEmailSender()
        {
            SmtpSender sender = new SmtpSender(new SmtpClient("localhost")
            {
                EnableSsl = false,
                DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory,
                PickupDirectoryLocation = @"C:\Users\piter\Mails"
            });

            Email.DefaultSender = sender;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            await Email.From("marshall.lesch5@ethereal.email").To(email).Subject(subject).Body(htmlMessage, isHtml: true).SendAsync();
        }
    }
}

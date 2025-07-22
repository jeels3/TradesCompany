using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace TradesCompany.Infrastructure.Services
{
    public class EmailService
    {
        private readonly IConfiguration _configuration;
        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public Task SendEmailAsync(string ToEmail, string Subject, string Body, bool IsBodyHtml = false)
        {
            string? MailServer = _configuration["EmailSettings:MailServer"];
            string? FromEmail = _configuration["EmailSettings:FromEmail"];
            string? Password = _configuration["EmailSettings:Password"];
            string? SenderName = _configuration["EmailSettings:SenderName"];
            int Port = Convert.ToInt32(_configuration["EmailSettings:MailPort"]);

            var client = new SmtpClient(MailServer, Port)
            {
                Credentials = new NetworkCredential(FromEmail, Password),
                EnableSsl = true,
            };

            MailAddress fromAddress = new MailAddress(FromEmail, SenderName);
            MailMessage mailMessage = new MailMessage
            {
                From = fromAddress,
                Subject = Subject,
                Body = Body,
            };
            mailMessage.To.Add(ToEmail);
            return client.SendMailAsync(mailMessage);
        }
    }
}

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using MtdKey.OrderMaker.AppConfig;
using MtdKey.OrderMaker.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace MtdKey.OrderMaker.Services
{
    public interface IEmailSenderBlank
    {
        Task SendEmailAsync(string email, string subject, string message, bool mustconfirm = true);
        Task<bool> SendEmailBlankAsync(BlankEmail blankEmail, bool mustconfirm = true);
    }

    public class BlankEmail
    {
        public string Email { get; set; }
        public string Subject { get; set; }
        public string Header { get; set; }
        public List<string> Content { get; set; }
    }

    public class EmailSender : IEmailSenderBlank
    {
        private readonly EmailSettings _emailSettings;
        private readonly IWebHostEnvironment _hostingEnvironment;       
        private readonly ConfigHandler configHandler;
        private readonly UserHandler userHandler;

        public EmailSender(IOptions<EmailSettings> emailSettings, 
            IWebHostEnvironment hostingEnvironment, 
            ConfigHandler configHandler, UserHandler userHandler)
        {
            _emailSettings = emailSettings.Value;
            _hostingEnvironment = hostingEnvironment;
            this.configHandler = configHandler;
            this.userHandler = userHandler;
        }

        public async Task SendEmailAsync(string email, string subject, string message, bool mustconfirm = true)
        {
            await ExecuteAsync(email, subject, message, mustconfirm);            
        }

        public async Task<bool> SendEmailBlankAsync(BlankEmail blankEmail, bool mustconfirm = true)
        {
            string pathImgMenu = $"{_emailSettings.Host}/lib/mtd-ordermaker/images/logo-mtd.png";
            var imgMenu = await configHandler.GetImageFromConfig(configHandler.CodeImgMenu);
            if (imgMenu != string.Empty) { pathImgMenu = $"{_emailSettings.Host}/images/logo.png"; }

            try
            {

                string message = string.Empty;
                foreach (string p in blankEmail.Content)
                {
                    message += $"<p>{p}</p>";
                }

                string webRootPath = _hostingEnvironment.WebRootPath;
                string contentRootPath = _hostingEnvironment.ContentRootPath;
                var file = Path.Combine(contentRootPath, "wwwroot", "lib", "mtd-ordermaker", "emailform", "blank.html");
                var htmlArray = File.ReadAllText(file);
                string htmlText = htmlArray.ToString();

                //htmlText = htmlText.Replace("{logo}", pathImgMenu);
                htmlText = htmlText.Replace("{title}", _emailSettings.Title);
                htmlText = htmlText.Replace("{header}", blankEmail.Header);
                htmlText = htmlText.Replace("{content}", message);
                htmlText = htmlText.Replace("{footer}", _emailSettings.Footer);

                await ExecuteAsync(blankEmail.Email, blankEmail.Subject, htmlText, mustconfirm);
            }
            catch
            {
                return false;
            }


            return true;
        }

        private async Task ExecuteAsync(string email, string subject, string message , bool mustconfirm = true)
        {
            
            WebAppUser user = await userHandler.FindByEmailAsync(email);       
            if (user ==null || (mustconfirm && user.EmailConfirmed == false)) { return; }
                       
            try
            {
                MailAddress toAddress = new(email);
                MailAddress fromAddress = new(_emailSettings.FromAddress, _emailSettings.FromName);
                // создаем письмо: message.Destination - адрес получателя
                MailMessage mail = new(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = message,
                    IsBodyHtml = true,
                };

                using SmtpClient smtp = new(_emailSettings.SmtpServer, _emailSettings.Port)
                {
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(_emailSettings.FromAddress, _emailSettings.Password),
                    EnableSsl = true
                };
                smtp.Send(mail);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error Email sender service \n {ex.Message}");
            }
        }
    }
}

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using MtdKey.EmailBuilder;
using MtdKey.EmailBuilder.EmailBlocks;
using MtdKey.OrderMaker.AppConfig;
using MtdKey.OrderMaker.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Xml;

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
        private readonly ITemplateStorage templateStorage;

        public EmailSender(IOptions<EmailSettings> emailSettings,
            IWebHostEnvironment hostingEnvironment,
            ConfigHandler configHandler, UserHandler userHandler, ITemplateStorage templateStorage)
        {
            _emailSettings = emailSettings.Value;
            _hostingEnvironment = hostingEnvironment;
            this.configHandler = configHandler;
            this.userHandler = userHandler;
            this.templateStorage = templateStorage;
        }

        public async Task SendEmailAsync(string email, string subject, string message, bool mustconfirm = true)
        {
            await ExecuteAsync(email, subject, message, mustconfirm);
        }


        public async Task<bool> SendEmailBlankAsync(BlankEmail blankEmail, bool mustconfirm = true)
        {

            string pathImgMenu = $"{_emailSettings.Host}/lib/mtd-ordermaker/images/logo-mtd.png";
            var imgMenu = await configHandler.GetImageFromConfig(configHandler.CodeImgMenu);
            if (imgMenu != string.Empty) { pathImgMenu = $"{_emailSettings.Host}/images/logo.png";}

            EmailDesigner emailDesigner = new();
            LayoutBlock layoutBlock = new LayoutBlock(templateStorage);
            await layoutBlock.CreateAsync();

            LogoBlock logoBlock = new(templateStorage);
            await logoBlock.CreateAsync(pathImgMenu);

            TextBlock titleBlock = new(templateStorage);
            await titleBlock.CreateAsync(_emailSettings.Title);

            DividerBlock dividerBlock = new(templateStorage);
            await dividerBlock.CreateAsync();

            ContainerBlock titleContainer = new(templateStorage);
            await titleContainer.CreateAsync([titleBlock, dividerBlock]);

            HeadingBlock headingBlock = new(templateStorage);
            await headingBlock.CreateAsync(blankEmail.Header);


            ContainerBlock headerContainer = new(templateStorage);
            await headerContainer.CreateAsync([headingBlock]);

            emailDesigner.AddLayout(layoutBlock)
                .AddBlock(logoBlock)
                .AddBlock(titleContainer)
                .AddBlock(headerContainer);


            List<EmailBlock> textBlocks = [];
            foreach (string text in blankEmail.Content)
            {
                var textBlock = new TextBlock(templateStorage);
                await textBlock.CreateAsync(text);
                textBlocks.Add(textBlock);
            }


            ContainerBlock containerBlock = new(templateStorage);
            await containerBlock.CreateAsync(textBlocks);

            InfoLineBlock infoLineBlock = new(templateStorage);
            await infoLineBlock.CreateAsync("Не отвечайте на это письмо оно создано автоматически и не дойдет до адресата.");

            var footerText = new TextBlock(templateStorage);
            await footerText.CreateAsync(_emailSettings.Footer);

            var footerUnsubscube = new TextBlock(templateStorage);
            await footerUnsubscube.CreateAsync("<div style='text-align:center; font-size: small'><a href=\"%unsubscribe_url%\">Отказаться от рассылки</a></div>");


            var containerFooter = new ContainerBlock(templateStorage);
            await containerFooter.CreateAsync([footerText, footerUnsubscube]);

            var htmlText = emailDesigner
                .AddBlock(containerBlock)
                .AddBlock(infoLineBlock)
                .AddBlock(containerFooter)
                .Build();

            await ExecuteAsync(blankEmail.Email, blankEmail.Subject, htmlText, mustconfirm);


            return true;
        }

        private async Task ExecuteAsync(string email, string subject, string message, bool mustconfirm = true)
        {

            WebAppUser user = await userHandler.FindByEmailAsync(email);
            if (user == null || (mustconfirm && user.EmailConfirmed == false)) { return; }

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

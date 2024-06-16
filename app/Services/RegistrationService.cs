/* Translate */

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MtdKey.EmailBuilder.EmailBlocks;
using MtdKey.EmailBuilder;
using MtdKey.OrderMaker.AppConfig;
using MtdKey.OrderMaker.Areas.Identity.Data;
using MtdKey.OrderMaker.Core;
using MtdKey.OrderMaker.Entity;
using MtdKey.OrderMaker.Services.Tokens;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace MtdKey.OrderMaker.Services
{
    public class RegistrationService(OrderMakerContext context,
        IAppParams appParams, ITokenService tokenService, 
        UserHandler userHandler, SignInManager<WebAppUser> signInManager, IOptions<EmailSettings> emailSettings,
        ConfigHandler configHandler, ITemplateStorage templateStorage)
    {
        private readonly OrderMakerContext context = context;
        private readonly IAppParams appParams = appParams;
        private readonly ITokenService tokenService = tokenService;
        private readonly UserHandler userHandler = userHandler;
        private readonly SignInManager<WebAppUser> signInManager = signInManager;
        private readonly EmailSettings _emailSettings = emailSettings.Value;
        private readonly ConfigHandler configHandler = configHandler;
        private readonly ITemplateStorage templateStorage = templateStorage;


        public async Task<bool> PageRequestAccepting()
        {
            return await appParams.GetValueAsync(ParamId.RegisterByPage) == "true";
        }

        public async Task<bool> APIRequestAccepting()
        {
            return await appParams.GetValueAsync(ParamId.RegisterByAPI) == "true";
        }


        public async Task<string> GetAPITokenAsync()
        {
            return await appParams.GetValueAsync(ParamId.RegisterAPIKey);
        }

        public async Task<Dictionary<string, string>> GetPublicFormsAsync()
        {
            var dictionary = new Dictionary<string, string>();
            var publicPolicyId = await appParams.GetValueAsync(AppConfig.ParamId.RegisterPolicy);
            var policies = await context.MtdPolicyForms.Include(x => x.MtdFormNavigation)
                .Where(x => x.MtdPolicy == publicPolicyId).ToListAsync();
            policies.ForEach(policy =>
            {
                var properties = policy.GetType().GetProperties().Where(x => x.PropertyType == typeof(sbyte)).ToList();
                foreach (var property in properties)
                {
                    if ((sbyte)property.GetValue(policy, null) == 1)
                    {
                        dictionary.Add(policy.MtdForm, policy.MtdFormNavigation.Name);
                        break;
                    }
                }
            });

            return dictionary;
        }

        public async Task SendTokenByEmailAsync(RequestFormToken formToken, HostInfo hostInfo)
        {
            var emailAttribute = new EmailAddressAttribute();
            bool validEmail = emailAttribute.IsValid(formToken.Email);
            if (!validEmail) throw new FormatException("Incorrect email format!");

            var token = await tokenService.EncodeTokenAsync(formToken);
            var link = $"<a style='font-size: small;' href='{hostInfo.Name}/identity/requests/createform?token={token}' target='_blank'>Заполнить форму запроса.</a>";
            
            var formName = await context.MtdForm.Where(x=>x.Id == formToken.FormId).Select(x => x.Name).FirstOrDefaultAsync() ?? string.Empty;
            var ok = await SendEmailAsync(new()
            {
                Subject = "Вам предоставлен досуп к созданию документов.",
                Header = $"Создание документа: {formName}",
                Email = formToken.Email,
                Content = ["Для создания документа нажмите ссылку.", link]
            });

            if (!ok) throw new Exception("Send Email Error!");
        }

        public async Task<bool> TokenValidateAsync(string token)
        {
            var validToken = await tokenService.CheckTokenValidityAsync(token);
            return validToken;
        }

        public async Task<bool> AcceptUserByToken(string token)
        {

            var requestForm = await tokenService.DecodeTokenAsync<RequestFormToken>(token);
            var user = await userHandler.Users.FirstOrDefaultAsync(x => x.NormalizedEmail == requestForm.Email.ToUpper());

            if (user == null)
            {
                
                user = new WebAppUser { Title = requestForm.UserName, UserName = requestForm.Email, Email = requestForm.Email, EmailConfirmed = true };
                var password = userHandler.GeneratePassword();
                await userHandler.CreateAsync(user, password);
                user = await userHandler.Users.FirstOrDefaultAsync(x => x.NormalizedEmail == requestForm.Email.ToUpper());
                await userHandler.AddToRoleAsync(user, "User");
                var publicPolicyId = await appParams.GetValueAsync(ParamId.RegisterPolicy);
                await userHandler.SetPolicyForUserAsync(user, publicPolicyId);

                var groupId = await appParams.GetValueAsync(ParamId.RegisterGroup);
                if(groupId != null && groupId != string.Empty)
                {
                    Claim claimGroup = new("group", groupId);
                    await userHandler.AddClaimAsync(user, claimGroup);
                }

            }

            await signInManager.SignInAsync(user, isPersistent: true);
            await tokenService.RemoveTokenAsync(token);

            return true;
        }

        public async Task<string> GetFormIdByTokenAsync(string token)
        {
            var requestForm = await tokenService.DecodeTokenAsync<RequestFormToken>(token);
            return requestForm.FormId;
        }

        private async Task<bool> SendEmailAsync(BlankEmail blankEmail)
        {
            string pathImgMenu = $"{_emailSettings.Host}/lib/mtd-ordermaker/images/logo-mtd.png";
            var imgMenu = await configHandler.GetImageFromConfig(configHandler.CodeImgMenu);
            if (imgMenu != string.Empty) { pathImgMenu = $"{_emailSettings.Host}/images/logo.png"; }

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
            await footerText.CreateAsync("<span style='font-size: small; font-style: italic;'>Вы получили данное письмо, потому что на ваш адрес был сделан запрос о предоставлении возможности создания документов и обращения в службу технической поддержки. Если вы получили данное письмо по ошибке, то нажмите на ссылку ниже и вы больше не будете получать письма с этого адреса.</span>");

            var footerUnsubscube = new TextBlock(templateStorage);
            await footerUnsubscube.CreateAsync("<div style='text-align:center; font-size: small;'><a href=\"%unsubscribe_url%\">Отказаться от рассылки</a></div>");


            var containerFooter = new ContainerBlock(templateStorage);
            await containerFooter.CreateAsync([footerText, footerUnsubscube]);

            var htmlText = emailDesigner
                .AddBlock(containerBlock)
                .AddBlock(infoLineBlock)
                .AddBlock(containerFooter)
                .Build();

            Execute(blankEmail.Email, blankEmail.Subject, htmlText);
            return true;

        }


        private void Execute(string email, string subject, string message)
        {

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

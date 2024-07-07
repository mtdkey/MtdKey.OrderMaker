using MtdKey.EmailBuilder.EmailBlocks;
using MtdKey.EmailBuilder;
using MtdKey.OrderMaker.AppConfig;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
/* Translate */
using System.Threading.Tasks;

namespace MtdKey.OrderMaker.Services.EmailService.Templates
{
    public class StartApprovalDesigner : EmailTaskDesigner
    {
        public override int Template => EmailTaskTemplate.StartApproval;
        public override string Subject => "Событие согласования документов.";

        public Guid DocId { get; set; }
        public Guid FormId { get; set; }
        public string FormName { get; set; }
        public DateTime Created { get; set; }
        public string Number { get; set; }

        public override string GetAttributesAsJsonString()
        {
            var attributes = new Dictionary<string, string>() {
                {nameof(DocId), DocId.ToString() },
                {nameof(FormId), FormId.ToString()},
                {nameof(FormName), FormName},
                {nameof(Number), Number},
                {nameof(Created), Created.ToString() }
            };

            return JsonConvert.SerializeObject(attributes);
        }

        public override void MappingAttributes(string jsonString)
        {
            var attributes = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonString);

            HandleGuidProperty(nameof(DocId), attributes);
            HandleGuidProperty(nameof(FormId), attributes);
            FormName = attributes[nameof(FormName)];
            Number = attributes[nameof(Number)];
            HandleDateProperty(nameof(Created), attributes);
        }

        private void HandleDateProperty(string propertyName, Dictionary<string, string> attributes)
        {
            if (DateTime.TryParse(attributes[propertyName], out DateTime value))
                this.GetType().GetProperty(propertyName).SetValue(this, value, null);
        }

        private void HandleGuidProperty(string propertyName, Dictionary<string, string> attributes)
        {
            if (Guid.TryParse(attributes[propertyName], out Guid value))
                this.GetType().GetProperty(propertyName).SetValue(this, value, null);
        }


        public override async Task<string> MakeBodyMessageAsync(
            ITemplateStorage templateStorage, 
            EmailSettings emailSettings)
        {
            var pathImgMenu = $"{emailSettings.Host}/images/logo.png";

            EmailDesigner emailDesigner = new();
            LayoutBlock layoutBlock = new LayoutBlock(templateStorage);
            await layoutBlock.CreateAsync();

            LogoBlock logoBlock = new(templateStorage);
            await logoBlock.CreateAsync(pathImgMenu);

            TextBlock titleBlock = new(templateStorage);
            await titleBlock.CreateAsync(emailSettings.Title);

            DividerBlock dividerBlock = new(templateStorage);
            await dividerBlock.CreateAsync();

            ContainerBlock titleContainer = new(templateStorage);
            await titleContainer.CreateAsync([titleBlock, dividerBlock]);

            HeadingBlock headingBlock = new(templateStorage);
            await headingBlock.CreateAsync("Согласование документа");


            ContainerBlock headerContainer = new(templateStorage);
            await headerContainer.CreateAsync([headingBlock]);

            emailDesigner.AddLayout(layoutBlock)
                .AddBlock(logoBlock)
            .AddBlock(titleContainer)
                .AddBlock(headerContainer);


            List<EmailBlock> emailBlocks = [];

            var textBlock = new TextBlock(templateStorage);
            await textBlock.CreateAsync($"Документ <strong>{FormName}</strong> ожидает Вашего согласования.");            
            emailBlocks.Add(textBlock);

            var clickMessage = new TextBlock(templateStorage);
            await clickMessage.CreateAsync("Для того чтобы открыть документ перейдите по ссылке ниже.");
            emailBlocks.Add(clickMessage);

            var linkBlock = new LinkBlock(templateStorage);
            await linkBlock.CreateAsync($"{FormName} N {Number} от {Created:dd.MM.yyyy} ", $"{emailSettings.Host}/workplace/store/details?id={DocId}");
            emailBlocks.Add(linkBlock);

            ContainerBlock containerBlock = new(templateStorage);
            await containerBlock.CreateAsync(emailBlocks);

            InfoLineBlock infoLineBlock = new(templateStorage);
            await infoLineBlock.CreateAsync("Не отвечайте на это письмо оно создано автоматически и не дойдет до адресата.");

            var footerText = new TextBlock(templateStorage);
            await footerText.CreateAsync(emailSettings.Footer);

            var footerUnsubscube = new TextBlock(templateStorage);
            await footerUnsubscube.CreateAsync($"<div style='text-align:center; font-size: small'><a href=\"%unsubscribe_url%\">Отказаться от рассылки</a></div>");


            var containerFooter = new ContainerBlock(templateStorage);
            await containerFooter.CreateAsync([footerText, footerUnsubscube]);

            var htmlText = emailDesigner
                .AddBlock(containerBlock)
                .AddBlock(infoLineBlock)
                .AddBlock(containerFooter)
                .Build();

            return htmlText;
        }


    }
}

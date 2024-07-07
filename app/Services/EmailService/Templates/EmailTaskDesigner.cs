using Microsoft.Extensions.Localization;
using MtdKey.EmailBuilder;
using MtdKey.OrderMaker.AppConfig;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MtdKey.OrderMaker.Services.EmailService.Templates
{
    public abstract class EmailTaskDesigner
    {
        public abstract int Template { get; }
        public abstract string Subject { get; }

        public abstract string GetAttributesAsJsonString();
        public abstract void MappingAttributes(string jsonString);
        public abstract Task<string> MakeBodyMessageAsync(ITemplateStorage templateStorage, EmailSettings emailSettings);
    }
}

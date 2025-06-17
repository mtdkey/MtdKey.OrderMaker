using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MtdKey.EmailBuilder.EmailBlocks
{
    public class UserNameBlock(ITemplateStorage templateStorage) : EmailBlock
    {
        public async Task CreateAsync(string text)
        {
            var template = await templateStorage.GetTemplateAsync(EmailTemplate.UserName);
            template = template.Replace("{{text}}", text);
            SetContect(template);
        }
    }
}

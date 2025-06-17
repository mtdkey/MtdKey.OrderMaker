using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MtdKey.EmailBuilder.EmailBlocks
{
    public class TextBoldBlock(ITemplateStorage templateStorage) : EmailBlock
    {
        public async Task CreateAsync(string text)
        {
            var template = await templateStorage.GetTemplateAsync(EmailTemplate.TextBold);
            template = template.Replace("{{text}}", text);
            SetContect(template);
        }
    }
}

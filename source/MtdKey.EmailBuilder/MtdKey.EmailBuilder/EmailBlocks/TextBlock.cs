using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MtdKey.EmailBuilder.EmailBlocks
{
    public class TextBlock(ITemplateStorage templateStorage) : EmailBlock
    {
        public async Task CreateAsync(string text)
        {
            var template = await templateStorage.GetTemplateAsync(EmailTemplate.Text);
            template = template.Replace("{{text}}", text);
            SetContect(template);
        }
    }
}

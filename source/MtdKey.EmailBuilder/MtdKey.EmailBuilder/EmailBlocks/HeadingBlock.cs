using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MtdKey.EmailBuilder.EmailBlocks
{
    public class HeadingBlock(ITemplateStorage templateStorage) : EmailBlock
    {
        public async Task CreateAsync(string text)
        {
            var template = await templateStorage.GetTemplateAsync(EmailTemplate.Heading);
            template = template.Replace("{{text}}", text);
            SetContect(template);
        }
    }
}

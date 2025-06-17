using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MtdKey.EmailBuilder.EmailBlocks
{
    public class ButtonBlock(ITemplateStorage emailTemplates) : EmailBlock
    {
        public async Task CreateAsync(string caption, string url)
        {
            var template = await emailTemplates.GetTemplateAsync(EmailTemplate.Button);
            template = template.Replace("{{caption}}", caption);
            template = template.Replace("{{url}}", url);
            SetContect(template);
        }

    }
}

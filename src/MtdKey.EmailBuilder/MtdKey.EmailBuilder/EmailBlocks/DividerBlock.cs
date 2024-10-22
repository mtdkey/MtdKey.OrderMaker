using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MtdKey.EmailBuilder.EmailBlocks
{
    public class DividerBlock(ITemplateStorage emailTemplates) : EmailBlock
    {
        public async Task CreateAsync()
        {
            var template = await emailTemplates.GetTemplateAsync(EmailTemplate.Divider);
            SetContect(template);
        }
    }
}

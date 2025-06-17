using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MtdKey.EmailBuilder.EmailBlocks
{
    public class ColumnsBlock(ITemplateStorage emailTemplates) : EmailBlock
    {
        public async Task CreateAsync(EmailBlock block1, EmailBlock block2)
        {
            var template = await emailTemplates.GetTemplateAsync(EmailTemplate.Columns);
            template = template.Replace("{{column1}}", block1.Content);
            template = template.Replace("{{column2}}", block2.Content);
            SetContect(template);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MtdKey.EmailBuilder.EmailBlocks
{
    public class InfoLineBlock (ITemplateStorage templateStorage) : EmailBlock
    {
        public async Task CreateAsync(string message)
        {
            var template = await templateStorage.GetTemplateAsync(EmailTemplate.InfoLine);
            template = template.Replace("{{message}}", message);
            SetContect(template);
        }
    }
}

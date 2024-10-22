using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MtdKey.EmailBuilder.EmailBlocks
{
    public class LogoBlock(ITemplateStorage templateStorage) : EmailBlock
    {
        public async Task CreateAsync(string url)
        {
            var template = await templateStorage.GetTemplateAsync(EmailTemplate.Logo);
            template = template.Replace("{{url}}", url);
            SetContect(template);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace MtdKey.EmailBuilder.EmailBlocks
{
    public class ContainerBlock(ITemplateStorage templateStorage) : EmailBlock
    {
        private const string contentTag = "{{content}}";

        public async Task CreateAsync(List<EmailBlock> blocks)
        {
            var template = await templateStorage.GetTemplateAsync(EmailTemplate.Container);
            blocks.ForEach(block =>
            {
                template = template.Replace(contentTag, $"{block.Content}{contentTag}");
            });
            template = template.Replace(contentTag, "");
            SetContect(template);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MtdKey.EmailBuilder
{
    public class EmailDesigner
    {
        private const string contentTag = "{{content}}";
        private string Email = contentTag;

        public EmailDesigner AddLayout(EmailBlock block)
        {
            Email = Email.Replace(contentTag, $"{block}");
            return this;
        }

        public EmailDesigner AddBlock(EmailBlock block)
        {
            Email = Email.Replace(contentTag, $"{block}{contentTag}");
            return this;
        }

        public string Build()
        {
            Email = Email.Replace(contentTag, "");
            return Email;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MtdKey.EmailBuilder
{
    public class EmailTemplate(string name)
    {
        public string Name { get; private set; } = name;
        public override string ToString() {  return Name; }

        public static EmailTemplate Layout => new("_layout");
        public static EmailTemplate Heading => new("heading");
        public static EmailTemplate Button => new("button");
        public static EmailTemplate Columns => new("columns");
        public static EmailTemplate Divider => new("divider");
        public static EmailTemplate Image => new("image");
        public static EmailTemplate Link => new("link");
        public static EmailTemplate Text => new("text");
        public static EmailTemplate Spacer => new("spacer");
        public static EmailTemplate Container => new("container");
        public static EmailTemplate InfoLine => new("infoLine");
        public static EmailTemplate UserName => new("userName");
        public static EmailTemplate TextBold => new("textBold");
        public static EmailTemplate Logo => new("logo");
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MtdKey.EmailBuilder
{
    public abstract class EmailBlock
    {
        private string content = string.Empty;
        public string Content => content;

        public void SetContect(string content)
        {
            this.content = content;
        }


        public override string ToString()
        {
            return content;
        }
    }
}

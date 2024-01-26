using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MtdKey.OrderMaker.Models.Controls.MTDSelectList
{
    public class MTDSelectListItem
    {
        public string Id { get; set; }
        public string Value { get; set; }
        public bool Localized { get; set; }
        public string Attributes { get; set; }
        public bool Selectded { get; set; }
    }
}

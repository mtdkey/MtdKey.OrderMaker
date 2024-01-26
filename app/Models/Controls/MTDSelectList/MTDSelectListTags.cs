using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MtdKey.OrderMaker.Models.Controls.MTDSelectList
{
    public class MTDSelectListTags
    {
        public string Id { get; set; }
        public string Label { get; set; }
        public bool LabelLocalized { get; set; }
        public string Name { get; set; }
        public string ValueId { get; set; }
        public bool Disabled { get; set; }
        public MTDSelectListViews MTDSelectListView { get; set; }
        public List<MTDSelectListItem> Items { get; set; }
    }
}

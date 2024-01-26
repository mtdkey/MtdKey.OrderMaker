using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MtdKey.OrderMaker.Models.Index
{
    public class ColumnItem
    {
        public string PartId { get; set; }
        public string PartName { get; set; }
        public string FieldId { get; set; }
        public string FieldName { get; set; }
        public bool IsChecked { get; set; }
        public int Sequence { get; set; }
    }
}

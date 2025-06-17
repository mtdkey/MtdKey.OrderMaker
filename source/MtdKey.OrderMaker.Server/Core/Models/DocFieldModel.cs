

using MtdKey.OrderMaker.src.formBuilder.models;
using System.Collections.Generic;

namespace MtdKey.OrderMaker.Core
{
    public class DocFieldModel
    {
        public string Id { get; set; }
        public string FormId { get; set; }
        public string PartId { get; set; }
        public string StoreId { get; set; }
        public string Name { get; set; }
        public int Sequence { get; set; }
        public int IndexSequence { get; set; } = int.MaxValue;
        public int Type { get; set; }
        public Dictionary<DocFieldOption, bool> Options { get; set; } = [];
        public string DefaultValue { get; set; }
        public object Value { get; set; }
        /// <summary>
        /// Don't show for empty value
        /// </summary>
        public bool IsEmptyData { get; set; } = true;
        public ListItemModel[] ListItems { get; set; } = [];
    }
}

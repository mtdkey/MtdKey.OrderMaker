

using MtdKey.OrderMaker.src.formBuilder.models;

namespace MtdKey.OrderMaker.Core
{
    public class DocFieldModel : DataFieldModel
    {
        public string Id { get; set; }
        public string PartId { get; set; }
        public string Name { get; set; }
        public int Sequence { get; set; }
        public int IndexSequence { get; set; } = int.MaxValue;
        public int Type { get; set; }
        public bool Readonly { get; set; }
        public bool Required { get; set; }
        public string DefaultValue { get; set; }
        public object Value { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string FileType { get; set; } = string.Empty;
        public long Size { get; set; } = 0;
        public bool IsEmptyData { get; set; } = true;
        public ListItemModel[] ListItems { get; set; } = [];
    }
}

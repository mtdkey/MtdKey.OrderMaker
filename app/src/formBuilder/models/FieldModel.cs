using MtdKey.OrderMaker.src.formBuilder.models;

namespace MtdKey.OrderMaker
{
    public class FieldModel
    {
        public string Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int SysType { get; set; }
        public string PartId { get; set; }
        public bool Readonly { get; set; }
        public bool Required { get; set; }
        public int Sequence { get; set; }
        public bool Active { get; set; } = true;
        public string DefaultValue { get; set; } = string.Empty;
        public ListItemModel[] ListItems { get; set; } = [];
   
    }
}

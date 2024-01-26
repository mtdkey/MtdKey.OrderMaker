namespace MtdKey.OrderMaker.Core
{
    public class FilterFieldModel
    {
        public string FieldId { get; set; }
        public int Type { get; set; }
        public string Value { get; set; } = string.Empty;
        public string ValueExt { get; set; } = string.Empty;
        public int Term { get; set; }

    }
}

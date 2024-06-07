namespace MtdKey.OrderMaker.Core
{
    public static class DocPartType
    {
        public const int Lines = 4;
        public const int Columns = 5;
    }
    public class DocPartModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public byte[] Image { get; set; }
        public int Sequence { get; set; }
        public bool ShowTitle { get; set; }
        public int Type { get; set; }
    }
}

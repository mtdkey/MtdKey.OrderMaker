namespace MtdKey.OrderMaker
{
    public class PartModel
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string FormId { get; set; } = string.Empty;
        public int Sequence { get; set; }
        public int StyleType { get; set; } = 4;
        public bool Title { get; set; } = true;
        public bool Active { get; set; } = true;
        public string ImageData { get; set; } = string.Empty;
        public int ImageSize { get; set; }
        public string ImageType { get; set; } = string.Empty;
    }
}

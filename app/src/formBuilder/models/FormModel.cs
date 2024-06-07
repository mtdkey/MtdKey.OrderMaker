namespace MtdKey.OrderMaker
{
    public class FormModel
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool Active { get; set; } = true;
        public int Sequence { get; set; }
        public bool VisibleNumber { get; set; } = true;
        public bool VisibleDate { get; set; } = true;
        public string ImageBack { get; set; } = string.Empty;
        public string ImageBackType { get; set; } = string.Empty;
        public int ImageBackSize { get; set; }
        public string ImageLogo { get; set; } = string.Empty;
        public string ImageLogoType { get; set; } = string.Empty;
        public int ImageLogoSize { get; set; }

    }
}

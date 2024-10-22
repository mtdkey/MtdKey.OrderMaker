
namespace MtdKey.OrderMaker.AppConfig
{
    public class LimitSettings
    {
        public int Users { get; set; } = 50;
        public int Forms { get; set; } = 10;
        public bool ExportExcel { get; set; } = true;
        public int ExportSize { get; set; } = 1000;
    }
}

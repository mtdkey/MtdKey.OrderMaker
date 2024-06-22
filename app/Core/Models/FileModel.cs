namespace MtdKey.OrderMaker.Core
{ 
    public class FileModel
    {
        public string FileName { get; set; } = string.Empty;
        public string FileType { get; set; } = string.Empty;
        public long Size { get; set; } = 0;
        public byte[] Data { get; set; } = [];
    }
}

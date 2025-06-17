
namespace MtdKey.InboxMediator
{
    public class AttachmentModel
    {
        public string ContentId { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
        public string FileType { get; set; } = string.Empty;
        public long Size { get; set; } = 0;
        public byte[] Data { get; set; } = [];
    }

    public class EmailModel
    {
        public ReaderModel ReaderModel { get; set; } = new();
        public string EmailAddress {  get; set; } = string.Empty;
        public string EmailName { get; set; } = string.Empty;
        public string From { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public List<AttachmentModel> Attachments { get; set; } = [];

    }
}

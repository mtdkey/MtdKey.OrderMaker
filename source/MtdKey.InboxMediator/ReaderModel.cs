using MtdKey.InboxMediator.Data;

namespace MtdKey.InboxMediator
{
    public class ReaderModel
    {
        public Guid Id { get; set; }        
        public EmailMediator EmailMediator { get; set; } = new();
        public string? Filter { get; set; }
    }
}

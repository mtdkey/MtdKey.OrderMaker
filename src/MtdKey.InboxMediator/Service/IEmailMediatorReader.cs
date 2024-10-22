
namespace MtdKey.InboxMediator.Service
{
    public interface IEmailMediatorReader
    {
        Task HandleNewEmailsAsync(ReaderModel readerModel);
    }
}

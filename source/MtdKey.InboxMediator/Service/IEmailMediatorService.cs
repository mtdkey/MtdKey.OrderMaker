using MtdKey.InboxMediator.Data;

namespace MtdKey.InboxMediator.Service
{
    public interface IEmailMediatorService
    {
        Task<EmailMediator?> GetMediatorAsync(string email);
        Task<EmailMediator[]> GetAllMediatorsAsync();
        Task<EmailMediator[]> GetActiveMediatorsAsync();
        Task UpdateMediatorAsync(EmailMediator mediator);
        Task AddMediatorAsync(EmailMediator mediator);
        Task RemoveMediatorAsync(string email);
    }
}

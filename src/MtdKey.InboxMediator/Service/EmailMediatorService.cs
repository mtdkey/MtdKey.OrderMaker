using Microsoft.EntityFrameworkCore;
using MtdKey.InboxMediator.Data;

namespace MtdKey.InboxMediator.Service
{
    public class EmailMediatorService(InboxMediatorContext context) : IEmailMediatorService
    {
        private readonly InboxMediatorContext context = context;
        public async Task AddMediatorAsync(EmailMediator mediator)
        {
            await context.EmailMediators.AddAsync(mediator);
            await context.SaveChangesAsync();
        }

        public async Task<EmailMediator?> GetMediatorAsync(string email)
        {
            return await context.EmailMediators.FirstOrDefaultAsync(x => x.EmailAsKey == email);
        }

        public async Task<EmailMediator[]> GetAllMediatorsAsync()
        {
            return await context.EmailMediators.ToArrayAsync();
        }

        public async Task RemoveMediatorAsync(string email)
        {
            context.EmailMediators.Remove(new EmailMediator { EmailAsKey = email });
            await context.SaveChangesAsync();
        }

        public async Task UpdateMediatorAsync(EmailMediator mediator)
        {
            context.Update(mediator);
            await context.SaveChangesAsync();
        }

        public async Task<EmailMediator[]> GetActiveMediatorsAsync()
        {
            return await context.EmailMediators.Where(x => x.Active).ToArrayAsync();
        }
    }
}

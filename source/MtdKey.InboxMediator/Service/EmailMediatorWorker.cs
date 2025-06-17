using MailKit;
using MailKit.Net.Imap;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MimeKit;
using MtdKey.InboxMediator.Data;


namespace MtdKey.InboxMediator.Service
{
    public class EmailMediatorWorker(InboxMediatorContext context, ILogger<EmailMediatorWorker> logger) : IEmailMediatorWorker
    {
        private readonly InboxMediatorContext context = context;
        private readonly ILogger<EmailMediatorWorker> logger = logger;

        public async Task<bool> GetLoaderFlagStatusAsync(string email)
        {
           var flag = await context.LoaderFlags.FirstOrDefaultAsync(x=>x.EmailAsKey == email);
           return flag != null && flag.Complete;
        }

        public async Task LoadHeadersAsync(EmailMediator mediator)
        {
            if(!mediator.Active) return;

            context.ChangeTracker.AutoDetectChangesEnabled = false;
            
            try
            {
                using var client = new ImapClient();
                await client.ConnectAsync(mediator.Server, mediator.Port, mediator.UseSSL);
                await client.AuthenticateAsync(mediator.EmailAsKey, mediator.Password);

                // The Inbox folder is always available on all IMAP servers...
                var inbox = client.Inbox;
                await inbox.OpenAsync(FolderAccess.ReadOnly);

                var maxUID = await context.InboxHeaders.AsNoTracking().MaxAsync(x => (uint?)x.UID) ?? 0;
                var fullFilled = await GetLoaderFlagStatusAsync(mediator.EmailAsKey);
                for (int i = inbox.Count; i > 0; i--)
                {
                    var fetched = client.Inbox.Fetch(i-1, i, MessageSummaryItems.Headers | MessageSummaryItems.UniqueId);
                    var header = fetched.FirstOrDefault();
                   
                    if (header == null) { continue; }

                    if (fullFilled && header.UniqueId.Id <= maxUID) { break; }                                

                    var inboxHeader = new InboxHeader
                    {
                        EmailAsKey = mediator.EmailAsKey,
                        UID = header.UniqueId.Id,
                    };
                    
                    var exists = await context.InboxHeaders.AsNoTracking()
                        .AnyAsync(x => x.UID == inboxHeader.UID && x.EmailAsKey == mediator.EmailAsKey);

                    if (exists) continue;
                    else
                    {
                        await context.InboxHeaders.AddAsync(inboxHeader);
                        await context.SaveChangesAsync();
                    }
                }
                
                if(!fullFilled)
                    await SetLoaderFlagStatusAsync(mediator.EmailAsKey, true);  

                await client.DisconnectAsync(true);

            }
            catch (Exception ex)
            {
                logger.LogError("{message}", ex.Message);
            }


            context.ChangeTracker.AutoDetectChangesEnabled = true;

        }

        public async Task SetLoaderFlagStatusAsync(string email, bool status)
        {
            var flag = await context.LoaderFlags.FirstOrDefaultAsync(x => x.EmailAsKey == email);
            if (flag == null)
            {
                flag = new LoaderFlag
                {
                    EmailAsKey = email,
                };

                await context.LoaderFlags.AddAsync(flag);
            }

            flag.Complete = status;
            context.Update(flag);
            await context.SaveChangesAsync();

        }
    }
}

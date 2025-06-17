using MailKit.Net.Imap;
using MailKit;
using Microsoft.Extensions.Logging;
using MtdKey.InboxMediator.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MimeKit;
using System.Net.Mail;

namespace MtdKey.InboxMediator.Service
{
    public class EmailMediatorReader<THandler>(InboxMediatorContext context, 
        ILogger<EmailMediatorWorker> logger, IServiceScopeFactory serviceScopeFactory)
        : IEmailMediatorReader where THandler : EmailModelHandler, new()
    {
        private readonly InboxMediatorContext context = context;
        private readonly ILogger<EmailMediatorWorker> logger = logger;
        private readonly IServiceScopeFactory serviceProvider = serviceScopeFactory;

        public async Task HandleNewEmailsAsync(ReaderModel readerModel)
        {

            var flag = await GetReaderFlagAsync(readerModel);
            var emailModelHandler = new THandler();

            uint[] uids = await context.InboxHeaders
                .Where(x => x.EmailAsKey == readerModel.EmailMediator.EmailAsKey && x.UID > flag.MaxUID)
                .Select(x => x.UID)
                .ToArrayAsync() ?? [];

            foreach (var uid in uids)
            {
                try
                {
                    var emailModel = await GetMessageAsync(readerModel, uid);
                    if (emailModel == null) continue;

                    emailModelHandler.SetServiceScopeFactory(serviceProvider);
                    var complete = await emailModelHandler.HandleAsync(emailModel);

                    if (complete)
                    {
                        flag.MaxUID = uid;
                        context.ReaderFlags.Update(flag);
                        await context.SaveChangesAsync();
                    }
                    else break;

                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "{message}", ex.Message);
                    break;
                }
            }
        }

        private async Task<ReaderFlag> GetReaderFlagAsync(ReaderModel readerModel)
        {
            var flag = await context.ReaderFlags.FirstOrDefaultAsync(x => x.EmailAsKey == readerModel.EmailMediator.EmailAsKey);

            if (flag == null)
            {
                flag = new ReaderFlag
                {
                    EmailAsKey = readerModel.EmailMediator.EmailAsKey,
                    ReaderId = readerModel.Id
                };

                await context.ReaderFlags.AddAsync(flag);
                await context.SaveChangesAsync();
            }

            return flag;
        }

        private static async Task<EmailModel?> GetMessageAsync(ReaderModel readerModel, uint uid)
        {

            var mediator = readerModel.EmailMediator;

            using var client = new ImapClient();
            await client.ConnectAsync(mediator.Server, mediator.Port, mediator.UseSSL);
            await client.AuthenticateAsync(mediator.EmailAsKey, mediator.Password);

            var inbox = client.Inbox;
            await inbox.OpenAsync(FolderAccess.ReadOnly);
            var uniqueId = new UniqueId(uid);
            var message = await inbox.GetMessageAsync(uniqueId);

            var attachments = new List<AttachmentModel>();

            foreach (var bodyPart in message.BodyParts)
            {

                if (bodyPart.ContentDisposition?.Disposition == ContentDisposition.Inline || bodyPart.IsAttachment)
                {
                    var tempFile = Path.GetRandomFileName();

                    using var stream = File.Create(tempFile);
                    await ((MimePart)bodyPart).Content.DecodeToAsync(stream);
                    stream.Close();

                    attachments.Add(new AttachmentModel
                    {
                        ContentId = bodyPart.ContentId,
                        FileName = bodyPart.ContentDisposition?.FileName ?? $"{Guid.NewGuid()}.eml",
                        FileType = bodyPart.ContentType.MimeType,
                        Size = tempFile.Length,
                        Data = await File.ReadAllBytesAsync(tempFile)
                    });

                    File.Delete(tempFile);

                }

            }

            var emailModel = new EmailModel
            {
                ReaderModel = readerModel,
                EmailAddress = message.From.Mailboxes.FirstOrDefault()?.Address ?? string.Empty,
                EmailName = message.From.Mailboxes.FirstOrDefault()?.Name ?? string.Empty,
                From = message.From.ToString(),
                Subject = message.Subject,
                Body = message.HtmlBody ?? message.TextBody ?? string.Empty,
                Attachments = attachments,
            };

            if (readerModel.Filter == null || readerModel.Filter == string.Empty) return emailModel;

            if (message.Subject == null || !message.Subject.ToUpper().Contains(readerModel.Filter.ToUpper())) return null;

            return emailModel;
        }
    }

}

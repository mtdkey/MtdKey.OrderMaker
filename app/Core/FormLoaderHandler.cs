using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MtdKey.InboxMediator;
using MtdKey.OrderMaker.Entity;
using MtdKey.OrderMaker.Services.FileStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityDbContext = MtdKey.OrderMaker.Entity.IdentityDbContext;

namespace MtdKey.OrderMaker.Core
{
    public class FormLoaderHandler : EmailModelHandler
    {
        private IServiceScopeFactory serviceScopeFactory = null;
        public override void SetServiceScopeFactory(IServiceScopeFactory serviceScopeFactory)
        {
            this.serviceScopeFactory = serviceScopeFactory;
        }

        public override async Task<bool> HandleAsync(EmailModel emailModel)
        {
            using IServiceScope scope = serviceScopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<OrderMakerContext>();
            var storageService = scope.ServiceProvider.GetRequiredService<IFileStorageService>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<FormLoaderHandler>>();

            var identityContext = scope.ServiceProvider.GetRequiredService<IdentityDbContext>();


            var targetForms = await context.TargetForms
                .Include(x => x.TargetFields)
                .Include(x => x.TargetFilters)
                .AsSplitQuery()
                .AsNoTracking()
                .ToListAsync();

            try
            {
                await context.Database.BeginTransactionAsync();
                await HandleTargetsAsync(emailModel, identityContext, context, storageService, targetForms);
                await context.Database.CommitTransactionAsync();

            }
            catch (Exception ex)
            {
                logger.LogError("{message}", ex.Message);
                return false;
            }

            return true;
        }

        private static async Task HandleTargetsAsync(EmailModel emailModel, IdentityDbContext identityContext,
            OrderMakerContext context, IFileStorageService storageService, List<TargetForm> targetForms)
        {
            foreach (var targetForm in targetForms)
            {
                var mtdStore = new MtdStore
                {
                    Id = Guid.NewGuid().ToString(),
                    MtdFormId = targetForm.FormId.ToString(),
                    Timecr = DateTime.Now,
                };

                int sequence = await context.MtdStore
                    .Where(x => x.MtdFormId == targetForm.FormId.ToString())
                    .MaxAsync(x => (int?)x.Sequence) ?? 0;
                mtdStore.Sequence = sequence + 1;

                await context.MtdStore.AddAsync(mtdStore);

                await context.MtdStoreTexts.AddAsync(new MtdStoreText
                {
                    StoreId = mtdStore.Id,
                    FieldId = targetForm.TargetFields.Where(x => x.Target == (int)RenderTarget.From)
                        .Select(x => x.FieldId.ToString()).FirstOrDefault(),
                    Result = emailModel.From
                });

                await context.MtdStoreTexts.AddAsync(new MtdStoreText
                {
                    StoreId = mtdStore.Id,
                    FieldId = targetForm.TargetFields.Where(x => x.Target == (int)RenderTarget.Subject)
                        .Select(x => x.FieldId.ToString()).FirstOrDefault(),
                    Result = emailModel.Subject
                });

                foreach (var attachment in emailModel.Attachments)
                {
                    var guid = await storageService.PutFileAsync(attachment.FileName, attachment.Size, attachment.FileType, attachment.Data);

                    emailModel.Body = emailModel.Body.Replace($"cid:{attachment.ContentId}", $"/api/file/{guid}");

                    if (guid != null || guid != Guid.Empty)
                        await context.MtdStoreFileLinks.AddAsync(new MtdStoreFileLink()
                        {
                            FieldId = targetForm.TargetFields.Where(x => x.Target == (int)RenderTarget.Attachments)
                                   .Select(x => x.FieldId.ToString()).FirstOrDefault(),
                            StoreId = mtdStore.Id,
                            FileName = attachment.FileName,
                            FileSize = attachment.Size,
                            FileType = attachment.FileType,
                            Result = (Guid)guid,
                        });
                }

                var dataList = emailModel.Body.SplitByLength(255);
                var memos = new List<MtdStoreMemo>();
                var fieldId = targetForm.TargetFields.Where(x => x.Target == (int)RenderTarget.Body)
                    .Select(x => x.FieldId.ToString()).FirstOrDefault();


                dataList.ToList().ForEach(memo =>
                {
                    memos.Add(new()
                    {
                        FieldId = fieldId,
                        StoreId = mtdStore.Id,
                        Result = memo,
                    });
                });

                await context.MtdStoreMemos.AddRangeAsync(memos);
                await context.SaveChangesAsync();

                var user = await identityContext.Users.FirstOrDefaultAsync(x => x.NormalizedEmail == emailModel.EmailAddress.ToUpper());

                if (user != null)
                {
                    await context.MtdStoreOwner.AddAsync(new MtdStoreOwner
                    {
                        Id = mtdStore.Id,
                        UserId = user.Id,
                        UserName = user.GetFullName()
                    });

                    await context.MtdLogDocument.AddAsync(new MtdLogDocument
                    {
                        MtdStore = mtdStore.Id,
                        UserId = user.Id,
                        UserName = user.GetFullName(),
                        TimeCh = DateTime.Now
                    });

                    await context.SaveChangesAsync();
                }
            }
        }
    }
}

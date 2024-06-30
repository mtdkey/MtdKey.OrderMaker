using MailKit;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MimeKit;
using MtdKey.InboxMediator;
using MtdKey.OrderMaker.Entity;
using MtdKey.OrderMaker.Services.FileStorage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MtdKey.OrderMaker.Core
{
    public class FormLoaderHandler : EmailModelHandler
    {

        private IServiceScopeFactory serviceScopeFactory = null;
        //private readonly string FormId = "16fa1a01-aefa-4582-8e09-86486f6f9378";
        //private readonly string SenderId = "053ad788-f833-4156-b244-8ab1a7f415d3";
        //private readonly string SubjectId = "6ab15ecc-6383-4a71-a616-e7b3a36180b1";
        //private readonly string BodyId = "533395f4-8027-47b0-8c69-2c01ce3247f6";

        public override async Task<bool> HandleAsync(EmailModel emailModel)
        {
            using IServiceScope scope = serviceScopeFactory.CreateScope();

            OrderMakerContext context =
                    scope.ServiceProvider.GetRequiredService<OrderMakerContext>();

            IFileStorageService storageService =
                    scope.ServiceProvider.GetRequiredService<IFileStorageService>();

            var targetForms = await context.TargetForms
                .Include(x => x.TargetFields)
                .Include(x => x.TargetFilters)
                .AsSplitQuery()
                .AsNoTracking()
                .ToListAsync();

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

                foreach (var attachment in emailModel.Attachments)
                {
                    string tempPath = Path.GetTempPath();
                    string tempFile = Path.Combine(tempPath, Guid.NewGuid().ToString());

                    using var stream = File.Create(tempFile);
                    if (attachment is MessagePart)
                    {
                        var rfc822 = (MessagePart)attachment;

                        rfc822.Message.WriteTo(stream);
                    }
                    else
                    {
                        var part = (MimePart)attachment;
                        part.Content.DecodeTo(stream);
                    }
                    stream.Close();

                    var fileArray = await File.ReadAllBytesAsync(tempFile);

                    var fileSize = (int)fileArray.Length;
                    var fileName = attachment.ContentDisposition.FileName;
                    var fileType = attachment.ContentType.MimeType;

                    var guid = await storageService.PutFileAsync(fileName, fileSize, fileType, fileArray);


                    if (guid != null || guid != Guid.Empty)
                        await context.MtdStoreFileLinks.AddAsync(new MtdStoreFileLink()
                        {
                            FieldId = targetForm.TargetFields.Where(x => x.Target == (int)RenderTarget.Attachments)
                                   .Select(x => x.FieldId.ToString()).FirstOrDefault(),
                            StoreId = mtdStore.Id,
                            FileName = fileName,
                            FileSize = fileSize,
                            FileType = fileType,
                            Result = (Guid)guid,
                        });

                    File.Delete(tempFile);
                }

                await context.SaveChangesAsync();

            }

            return true;
        }

        public override void SetServiceScopeFactory(IServiceScopeFactory serviceScopeFactory)
        {
            this.serviceScopeFactory = serviceScopeFactory;
        }

    }
}

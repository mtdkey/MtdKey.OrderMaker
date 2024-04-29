using MtdKey.OrderMaker.Entity;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System;
using Microsoft.EntityFrameworkCore;
using MtdKey.OrderMaker.Areas.Identity.Data;

namespace MtdKey.OrderMaker.Core
{
    public partial class StoreService : IStoreService
    {
        public async Task AddStoreItemsAsync(StorePostRequest storeRequest, MtdStore store)
        {
            var currentUser = await userHandler.GetUserAsync(storeRequest.UserPrincipal);

            StoreDateHanlder(storeRequest, store);
            await LogHandlerAsync(store, currentUser);

            var parts = await GetAllowedPartsAsync(new()
            {
                FormId = storeRequest.FormId,
                StoreId = storeRequest.StoreId,
                UserPrincipal = storeRequest.UserPrincipal,
                ActionTypeRequest = storeRequest.ActionType,
            });

            var partIds = parts.Select(x => x.Id).ToList();

            var partFields = await context.MtdFormPartField
                .Where(x => partIds.Contains(x.MtdFormPartId))
                .Select(field => new
                {
                    Key = field.Id,
                    Type = field.MtdSysType,
                    Value = GetFieldValue(field, storeRequest) == null 
                        && field.ReadOnly != 1
                        && field.DefaultData != null 
                        && field.DefaultData != string.Empty ? 
                     field.DefaultData : GetFieldValue(field, storeRequest)
                })
                .ToListAsync();

            await context.Entry(store).Collection(x => x.MtdStoreTexts).LoadAsync();
            await context.Entry(store).Collection(x => x.MtdStoreDates).LoadAsync();
            await context.Entry(store).Collection(x => x.MtdStoreInts).LoadAsync();
            await context.Entry(store).Collection(x => x.MtdStoreDecimals).LoadAsync();
            await context.Entry(store).Collection(x => x.MtdStoreMemos).LoadAsync();
            await context.Entry(store).Collection(x => x.MtdStoreFiles).LoadAsync();
            await context.Entry(store).Collection(x => x.MtdStoreItems).LoadAsync();

            foreach (var partField in partFields)
            {
                switch (partField.Type)
                {
                    case FieldType.Text:
                    case FieldType.Link:
                        {
                            if (partField.Value == null)
                                break;

                            MarkDeletedStoreItems(store.MtdStoreTexts, partField.Key);

                            await context.MtdStoreTexts.AddAsync(
                            new()
                            {
                                FieldId = partField.Key,
                                StoreId = storeRequest.StoreId,
                                Result = (string)partField.Value,
                            });

                            break;
                        }
                    case FieldType.Date:
                    case FieldType.DateTime:
                        {
                            if (partField.Value == null
                                || !DateTime.TryParse((string)partField.Value, out DateTime dtResult))
                                break;

                            MarkDeletedStoreItems(store.MtdStoreDates, partField.Key);

                            await context.MtdStoreDates.AddAsync(
                                    new()
                                    {
                                        FieldId = partField.Key,
                                        StoreId = storeRequest.StoreId,
                                        Result = dtResult,
                                    });
                            break;
                        }
                    case FieldType.Int:
                        {
                            if (partField.Value == null
                                || !int.TryParse((string)partField.Value, out int val))
                                break;

                            MarkDeletedStoreItems(store.MtdStoreInts, partField.Key);

                            await context.MtdStoreInts.AddAsync(
                                    new()
                                    {
                                        FieldId = partField.Key,
                                        StoreId = storeRequest.StoreId,
                                        Result = val,
                                    });
                            break;
                        }
                    case FieldType.Checkbox:
                        {
                            bool checkBoxResult = false;

                            if (partField.Value != null)
                                checkBoxResult = (string)partField.Value == "true";

                            MarkDeletedStoreItems(store.MtdStoreInts, partField.Key);

                            await context.MtdStoreInts.AddAsync(
                                new()
                                {
                                    FieldId = partField.Key,
                                    StoreId = storeRequest.StoreId,
                                    Result = checkBoxResult ? 1 : 0,
                                });
                            break;
                        }
                    case FieldType.List:
                        {
                            if (!Guid.TryParse((string) partField.Value, out Guid guid)) break;

                            var item = await context.MtdFormPartFieldItems.FindAsync(guid);
                            if (item == null) break;
                            
                            MarkDeletedStoreItems(store.MtdStoreItems, partField.Key);

                            await context.MtdStoreItems.AddAsync(new() {
                                FieldId = partField.Key,
                                StoreId = storeRequest.StoreId,
                                ItemId = item.Id,
                                Result = item.Name
                            });

                            break;
                        }
                    case FieldType.Decimal:
                        {
                            if (partField.Value == null) break;
                            string separ = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
                            string data = (string)partField.Value;
                            data = data.Replace(".", separ);
                            if (!decimal.TryParse(data, out decimal val))
                                break;

                            MarkDeletedStoreItems(store.MtdStoreDecimals, partField.Key);

                            await context.MtdStoreDecimals.AddAsync(
                                    new()
                                    {
                                        FieldId = partField.Key,
                                        StoreId = storeRequest.StoreId,
                                        Result = val,
                                    });
                            break;
                        }
                    case FieldType.Memo:
                        {
                            if (partField.Value is not string val)
                                break;

                            MarkDeletedStoreItems(store.MtdStoreMemos, partField.Key);

                            var dataList = val.SplitByLength(255);
                            var memos = new List<MtdStoreMemo>();

                            dataList.ToList().ForEach(memo =>
                            {
                                memos.Add(new()
                                {
                                    FieldId = partField.Key,
                                    StoreId = storeRequest.StoreId,
                                    Result = memo,
                                });
                            });
                            await context.MtdStoreMemos.AddRangeAsync(memos);

                            break;
                        }
                    case FieldType.File:
                    case FieldType.Image:
                        {

                            if (storeRequest.DeleteFields.Any(x => x.Key == partField.Key && x.Value == "true"))
                            {
                                var sf = await context.MtdStoreFiles
                                    .FirstOrDefaultAsync(x => x.StoreId == storeRequest.StoreId && x.FieldId == partField.Key);
                                sf.IsDeleted = true;

                                break;
                            }

                            if (partField.Value == null)
                                break;

                            var file = storeRequest.Files.FirstOrDefault(f => f.Name == $"field-{partField.Key}");
                            if (file == null)
                                break;

                            MarkDeletedStoreItems(store.MtdStoreFiles, partField.Key);

                            byte[] streamArray = new byte[file.Length];
                            await file.OpenReadStream().ReadAsync(streamArray);
                            var storeFile = new MtdStoreFile()
                            {
                                FieldId = partField.Key,
                                StoreId = storeRequest.StoreId,
                                Result = streamArray,
                                FileName = file.FileName,
                                FileSize = streamArray.Length,
                                FileType = file.ContentType
                            };
                            await context.MtdStoreFiles.AddAsync(storeFile);
                            break;
                        }
                }
            }

        }

        private async Task LogHandlerAsync(MtdStore store, WebAppUser currentUser)
        {
            await context.MtdLogDocument.AddAsync(new()
            {
                MtdStore = store.Id,
                TimeCh = DateTime.UtcNow,
                UserId = currentUser.Id,
                UserName = currentUser.GetFullName()
            });
        }

        private static void StoreDateHanlder(StorePostRequest storeRequest, MtdStore store)
        {
            if (DateTime.TryParse(storeRequest.DateCreated, out DateTime dateTime))
                store.Timecr = dateTime;
        }

        private static object GetFieldValue(MtdFormPartField field, StorePostRequest storeRequest)
        {
            if (field.MtdSysType == FieldType.File)
                return storeRequest.Files
                       .FirstOrDefault(f => f.Name == $"field-{field.Id}");

            return storeRequest.Fields.Where(f => f.Key == field.Id)
                       .Select(x => x.Value)
                       .FirstOrDefault();
        }
    }
}

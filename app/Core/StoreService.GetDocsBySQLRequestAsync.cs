using Microsoft.EntityFrameworkCore;
using MtdKey.OrderMaker.Areas.Identity.Data;
using MtdKey.OrderMaker.Components.AttachedFiles;
using MtdKey.OrderMaker.Core.Scripts;
using MtdKey.OrderMaker.Entity;
using MtdKey.OrderMaker.src.formBuilder.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MtdKey.OrderMaker.Core
{
    public partial class StoreService : IStoreService
    {
        public async Task<RequestResult> GetDocsBySQLRequestAsync(StoreDocRequest docRequest)
        {
            var requestResult = new RequestResult();
            var appUser = await userHandler.GetUserAsync(docRequest.UserPrincipal);
            var allowedData = await SecurityHandlerAsync(docRequest);

            FilterSQLparams filterSQLparams = await GetFilterSQLParams(docRequest, appUser);
            filterSQLparams.UserInGroupIds = allowedData.UsersInGroupIds;

            if (docRequest.LimitRequest)
                filterSQLparams.PageSize = limitSettings.ExportSize;

            if (docRequest.UseFilter)
                allowedData.DocFields = allowedData.DocFields
                    .Where(x => filterSQLparams.FilterColumnIds.Contains(x.Id)).ToList();

            filterSQLparams.DocFieldModels = allowedData.DocFields;
            filterSQLparams.TypeRequest = TypeRequest.GetRowCount;
            var scriptForCounter = SqlScript.GetStoreIds(filterSQLparams);

            var rows = await context.Database.SqlQueryRaw<int>(scriptForCounter)
                .ToListAsync();

            int rowCount = rows.FirstOrDefault();
            int pageCount = 0;
            decimal count = (decimal)rowCount / filterSQLparams.PageSize;
            pageCount = Convert.ToInt32(Math.Ceiling(count));
            requestResult.PageCount = pageCount == 0 ? 1 : pageCount;

            filterSQLparams.TypeRequest = TypeRequest.GetIds;
            var scriptForIds = SqlScript.GetStoreIds(filterSQLparams);

            var storeIds = await context.Database.SqlQueryRaw<string>(scriptForIds)
                .ToListAsync();

            Dictionary<string, int> indexStore = new();
            foreach (var (item, index) in storeIds.WithIndex())
            {
                indexStore.Add(item, index);
            }

            var storeItems = await context.MtdStore
                .Include(x => x.MtdStoreTexts)
                .Include(x => x.MtdStoreInts)
                .Include(x => x.MtdStoreDates)
                .Include(x => x.MtdStoreDecimals)
                .Include(x => x.MtdStoreMemos)
                .Include(x => x.MtdStoreFiles)
                .Include(x => x.MtdStoreItems)
                .Include(x => x.MtdStoreFileLinks)
                .AsSplitQuery()
                .Where(x => storeIds.Contains(x.Id))
                .ToListAsync();

            storeItems = storeItems
                 .Join(indexStore, store => store.Id, index => index.Key, (store, index) => new { store, index = index.Value })
                 .OrderBy(x => x.index)
                 .Select(s => s.store)
                 .ToList();

            var formIds = storeItems.Select(x => x.MtdFormId).ToList();
            var forms = await context.MtdForm
                .Include(x => x.MtdFormHeader)
                .AsSplitQuery()
                .ToListAsync();

            var docList = new List<DocModel>();
            foreach (var storeItem in storeItems)
            {
                var form = forms.FirstOrDefault(x => x.Id == storeItem.MtdFormId);
                var doc = new DocModel
                {
                    Id = storeItem.Id,
                    FormId = storeItem.MtdFormId,
                    FormName = form.Name,
                    Image = form.MtdFormHeader?.Image,
                    Sequence = storeItem.Sequence,
                    Created = storeItem.Timecr,
                    Parts = allowedData.DocParts
                };

                docList.Add(doc);

                foreach (var docField in allowedData.DocFields)
                {
                    switch (docField.Type)
                    {
                        case FieldType.Text:
                        case FieldType.Link:
                            {
                                var value = storeItem.MtdStoreTexts
                                    .Where(x => x.FieldId == docField.Id)
                                    .Select(x => x.Result)
                                    .FirstOrDefault();

                                docField.IsEmptyData = value == null || value == string.Empty;
                                AddDocField(doc, docField, value);
                                break;
                            }
                        case FieldType.Memo:
                        case FieldType.HTMLEditor:
                            {
                                var value = string.Join("", storeItem.MtdStoreMemos
                                   .Where(x => x.FieldId == docField.Id)
                                   .OrderBy(x => x.Id)
                                   .Select(x => x.Result)
                                   .ToList());

                                docField.IsEmptyData = value == null || value == string.Empty;
                                AddDocField(doc, docField, value);
                                break;
                            }
                        case FieldType.Int:
                        case FieldType.Checkbox:
                            {
                                int? value = storeItem.MtdStoreInts
                                    .Where(x => x.FieldId == docField.Id)
                                    .Select(x => x.Result)
                                    .FirstOrDefault();

                                docField.IsEmptyData = value == null;
                                AddDocField(doc, docField, value);

                                break;
                            }
                        case FieldType.Decimal:
                            {
                                decimal? value = storeItem.MtdStoreDecimals
                                    .Where(x => x.FieldId == docField.Id)
                                    .Select(x => x.Result)
                                    .FirstOrDefault();

                                docField.IsEmptyData = value == null;
                                AddDocField(doc, docField, value);

                                break;
                            }
                        case FieldType.Date:
                        case FieldType.DateTime:
                            {
                                DateTime? value = storeItem.MtdStoreDates
                                    .Where(x => x.FieldId == docField.Id)
                                    .Select(x => x.Result)
                                    .FirstOrDefault();

                                docField.IsEmptyData = value == null || value == DateTime.MinValue;
                                AddDocField(doc, docField, value);

                                break;
                            }
                        case FieldType.File:
                        case FieldType.Image:
                            {
                                FileModel fileModel = null;
                                var file = storeItem.MtdStoreFiles
                                    .FirstOrDefault(x => x.FieldId == docField.Id);

                                if (file != null)
                                    fileModel = new FileModel
                                    {
                                        Data = file?.Result ?? [],
                                        Size = file.FileSize,
                                        FileName = file.FileName,
                                        FileType = file.FileType
                                    };

                                doc.Fields.Add(new DocFieldModel()
                                {
                                    Id = docField.Id,
                                    StoreId = doc.Id,
                                    FormId = doc.FormId,
                                    PartId = docField.PartId,
                                    Name = docField.Name,
                                    Sequence = docField.Sequence,
                                    IndexSequence = docField.IndexSequence,
                                    Type = docField.Type,
                                    Options = docField.Options,
                                    Value = fileModel,
                                    IsEmptyData = file == null || file.Result == null || file.Result.Length == 0
                                });

                                break;
                            }
                        case FieldType.List:
                            {
                                var value = storeItem.MtdStoreItems
                                    .Where(x => x.FieldId == docField.Id)
                                    .Select(x => x.ItemId.ToString())
                                    .FirstOrDefault();

                                docField.IsEmptyData = value == null || value == string.Empty;
                                AddDocField(doc, docField, value);
                                break;
                            }
                        case FieldType.FileStorage:
                            {
                                List<AttachedFile> attachedFiles = new List<AttachedFile>();
                                var fieldLinks = storeItem.MtdStoreFileLinks
                                   .Where(x => x.FieldId == docField.Id)
                                   .OrderBy(x => x.Id)
                                   .ToList();

                                foreach (var fieldLink in fieldLinks)
                                {
                                    attachedFiles.Add(new AttachedFile()
                                    {
                                        Id = fieldLink.Result.ToString(),
                                        Name = fieldLink.FileName,
                                        Size = fieldLink.FileSize,
                                        Mime = fieldLink.FileType,
                                        ByteArray = []
                                    });
                                }

                                docField.IsEmptyData = attachedFiles.Count == 0;
                                AddDocField(doc, docField, attachedFiles);
                                break;
                            }
                    }
                }
            }

            requestResult.Docs = docList;
            return requestResult;
        }

        private async Task<FilterSQLparams> GetFilterSQLParams(StoreDocRequest docRequest, WebAppUser appUser)
        {
            FilterSQLparams filterSQLparams = new()
            {
                FormId = docRequest.FormId
            };

            if (docRequest.StoreId != null && docRequest.StoreId != string.Empty)
            {

                filterSQLparams.StoreId = docRequest.StoreId;
                return filterSQLparams;
            }

            if (!docRequest.UseFilter) return filterSQLparams;

            MtdFilter userFilter = await GetUserFilter(docRequest.FormId, appUser);
            List<FilterFieldModel> filterFields = GetFilterFields(userFilter);

            filterSQLparams.Page = userFilter.Page;
            filterSQLparams.PageSize = userFilter.PageSize;
            filterSQLparams.DateStart = userFilter.MtdFilterDate?.DateStart ?? DateTime.MinValue;
            filterSQLparams.DateEnd = userFilter.MtdFilterDate?.DateStart ?? DateTime.MaxValue;
            filterSQLparams.SortByFieldId = userFilter.Sort ?? string.Empty;
            filterSQLparams.SearchNumber = userFilter.SearchNumber ?? string.Empty;
            filterSQLparams.SearchText = userFilter.SearchText ?? string.Empty;
            filterSQLparams.SortOrder = userFilter.SortOrder ?? "asc";
            filterSQLparams.OwnerId = userFilter.MtdFilterOwner != null ? userFilter.MtdFilterOwner.OwnerId : string.Empty;
            filterSQLparams.FilterFields = filterFields;
            filterSQLparams.FilterColumnIds = userFilter.MtdFilterColumns.Select(x => x.MtdFormPartFieldId).ToList();

            return filterSQLparams;
        }

        private static List<FilterFieldModel> GetFilterFields(MtdFilter userFilter)
        {
            List<FilterFieldModel> filterFields = new();

            foreach (var field in userFilter.MtdFilterFields)
            {

                if (field.MtdFormPartFieldNavigation == null
                    || field.MtdFormPartFieldNavigation?.MtdSysType == 0)
                    continue;

                filterFields.Add(new()
                {
                    FieldId = field.MtdFormPartFieldId,
                    Type = field.MtdFormPartFieldNavigation.MtdSysType,
                    Term = field.MtdTerm,
                    Value = field.Value,
                    ValueExt = field.ValueExtra
                });
            }

            return filterFields;
        }

        private async Task<List<DocFieldModel>> GetDocFields(string formId, WebAppUser appUser, List<string> partIds)
        {
            MtdFilter userFilter = await GetUserFilter(formId, appUser) ?? new();

            var docFields = await context.MtdFormPartField
                .Include(x => x.MtdFilterColumn)
                .Include(x => x.MtdFormPartFieldItems)
                .Where(x => partIds.Contains(x.MtdFormPartId))
                .Select(partField => new DocFieldModel
                {
                    Id = partField.Id,
                    PartId = partField.MtdFormPartId,
                    Name = partField.Name,
                    Sequence = partField.Sequence,
                    DefaultValue = partField.DefaultData,
                    Type = partField.MtdSysType,

                    Options = new Dictionary<DocFieldOption, bool> {
                        { DocFieldOption.Required, partField.Required == 1 },
                        { DocFieldOption.Readyonly, partField.ReadOnly == 1 }
                    },

                    ListItems = partField.MtdFormPartFieldItems.Where(item => item.IsDeleted == false)
                        .Select(model => new ListItemModel { Id = model.Id.ToString(), Name = model.Name })
                        .ToArray(),

                }).AsSplitQuery()
                .OrderBy(x => x.Sequence)
                .ToListAsync();

            foreach (var docField in docFields)
            {
                int? sequence = userFilter.MtdFilterColumns?
                    .Where(x => x.MtdFormPartFieldId == docField.Id).Select(x => (int?)x.Sequence)
                    .FirstOrDefault();

                docField.IndexSequence = sequence ?? int.MaxValue;
            }

            return docFields;
        }

        private async Task<MtdFilter> GetUserFilter(string formId, WebAppUser appUser)
        {
            var userFilter = await context.MtdFilter
                  .Include(x => x.MtdFilterColumns)
                  .Include(x => x.MtdFilterDate)
                  .Include(x => x.MtdFilterOwner)
                  .Include(x => x.MtdFilterFields)
                  .ThenInclude(x => x.MtdFormPartFieldNavigation)
                  .AsNoTracking()
                  .AsSplitQuery()
                  .Where(x => x.IdUser == appUser.Id && x.MtdFormId == formId)
                  .FirstOrDefaultAsync();

            return userFilter;
        }

        private static void AddDocField(DocModel doc, DocFieldModel docField, object value)
        {
            doc.Fields.Add(new()
            {
                Id = docField.Id,
                StoreId = doc.Id,
                FormId = doc.FormId,
                PartId = docField.PartId,
                Name = docField.Name,
                DefaultValue = docField.DefaultValue,
                Options = docField.Options,
                Sequence = docField.Sequence,
                IndexSequence = docField.IndexSequence,
                Type = docField.Type,
                Value = value,
                IsEmptyData = docField.IsEmptyData,
                ListItems = docField.ListItems,
            });
        }
    }
}

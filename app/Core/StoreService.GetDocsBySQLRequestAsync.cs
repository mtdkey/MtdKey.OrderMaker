using Microsoft.EntityFrameworkCore;
using MtdKey.OrderMaker.Core.Scripts;
using MtdKey.OrderMaker.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;
using MtdKey.OrderMaker.Areas.Identity.Data;
using MtdKey.OrderMaker.src.formBuilder.models;

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

            var IsReviewer = await userHandler.IsReviewerAsync(appUser, docRequest.FormId);
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
                    Parts = allowedData.DocParts,
                    EditDate = IsReviewer,                    
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
                            {
                                var value = string.Join("", storeItem.MtdStoreMemos
                                   .Where(x=> x.FieldId == docField.Id)
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
                                var file = storeItem.MtdStoreFiles
                                    .FirstOrDefault(x => x.FieldId == docField.Id);

                                doc.Fields.Add(new DocFieldModel()
                                {
                                    Id = docField.Id,
                                    StoreId = doc.Id,
                                    FormId = doc.FormId,
                                    PartId = docField.PartId,
                                    Name = docField.Name,
                                    FileName = file?.FileName,
                                    Sequence = docField.Sequence,
                                    IndexSequence = docField.IndexSequence,
                                    Type = docField.Type,
                                    Size = file?.FileSize ?? 0,
                                    FileType = file?.FileType,
                                    Required = docField.Required,
                                    Value = file?.Result,
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
            MtdFilter userFilter = await GetUserFilter(formId, appUser);

            var docFields = await context.MtdFormPartField
                .Include(x=>x.MtdFilterColumn)
                .Include(x=>x.MtdFormPartFieldItems)
                .Where(x => partIds.Contains(x.MtdFormPartId))
                .Select(x => new DocFieldModel
                {
                    Id = x.Id,
                    PartId = x.MtdFormPartId,
                    Name = x.Name,
                    Sequence = x.Sequence,                               
                    DefaultValue = x.DefaultData,
                    Readonly = x.ReadOnly == 1,
                    Type = x.MtdSysType,
                    Required = x.Required == 1,
                    ListItems = x.MtdFormPartFieldItems.Where(x=>x.IsDeleted == false)
                        .Select(x=> new ListItemModel { Id = x.Id.ToString(), Name=x.Name})
                        .ToArray(),
                }).AsSplitQuery()
                .OrderBy(x => x.Sequence)
                .ToListAsync();

            foreach(var docField in docFields)
            {
                int? sequence = userFilter.MtdFilterColumns?
                    .Where(x => x.MtdFormPartFieldId == docField.Id).Select(x=> (int?) x.Sequence)
                    .FirstOrDefault();
                
                docField.IndexSequence = sequence ?? int.MaxValue;
            }

            return docFields;
        }

        private async Task<MtdFilter> GetUserFilter(string formId, WebAppUser appUser)
        {
            return await context.MtdFilter
                .Include(x => x.MtdFilterColumns)
                .Include(x => x.MtdFilterDate)
                .Include(x => x.MtdFilterOwner)
                .Include(x => x.MtdFilterFields)
                .ThenInclude(x => x.MtdFormPartFieldNavigation)
                .AsSplitQuery()
                .Where(x => x.IdUser == appUser.Id && x.MtdFormId == formId)
                .FirstOrDefaultAsync();
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
                Readonly = docField.Readonly,
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

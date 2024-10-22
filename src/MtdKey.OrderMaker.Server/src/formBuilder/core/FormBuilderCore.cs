using Microsoft.EntityFrameworkCore;
using MtdKey.OrderMaker.Areas.Identity.Data;
using MtdKey.OrderMaker.Entity;
using MtdKey.OrderMaker.Services;
using MtdKey.OrderMaker.src.formBuilder.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;


namespace MtdKey.OrderMaker
{
    public class FormBuilderCore
    {
        private readonly DataConnector context;
        private readonly JsonSerializerOptions jsonOptions;
        private readonly UserHandler userHandler;
        private readonly WebAppUser currentUser;
        public FormBuilderCore(DataConnector context, WebAppUser currentUser, UserHandler userHandler)
        {
            this.context = context;
            this.userHandler = userHandler;
            this.currentUser = currentUser;

            jsonOptions = new()
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                WriteIndented = true
            };
        }


        public async Task CheckPolicy()
        {
            var policy = await userHandler.GetPolicyForUserAsync(currentUser);
            if (policy != null) return;

            policy = new MtdPolicy { Id = Guid.NewGuid().ToString(), Name = "Full Access", Description = "Full Access" };
            await context.MtdPolicy.AddAsync(policy);
            await context.SaveChangesAsync();

            await userHandler.SetPolicyForUserAsync(currentUser, policy.Id);

        }


        public async Task<string> GetJsonFormDataAsync(string formId)
        {
            var formModel = await GetFormModelAsync(formId);
            var partModels = await GetFormPartsAsync(formId);
            var partIds = partModels.Select(x => x.Id).ToArray();
            var fieldModels = await GetFieldsAsync(partIds);
            var formInfoModels = await context.MtdForm
                .Where(x => x.Id != formId)
                .OrderBy(x => x.Sequence)
                .Select(x => new FormInfoModel { Id = x.Id, Name = x.Name })
                .ToArrayAsync();

            FormDataModel formDataModel = new()
            {
                FormModel = formModel,
                PartModels = partModels,
                FieldModels = fieldModels,
                FormInfoModels = formInfoModels,
            };

            var result = JsonSerializer.Serialize(formDataModel);
            return result;
        }

        public async Task<string> GetJsonFormStringAsync(string formId)
        {
            var formModel = await GetFormModelAsync(formId);
            return GetJsonFormString(formModel);
        }

        public string GetJsonFormString(FormModel model)
        {
            return JsonSerializer.Serialize(model, jsonOptions);
        }

        public async Task<FormModel> GetFormModelAsync(string formId)
        {
            FormModel formModel = new()
            {
                Id = formId
            };

            var form = await context.MtdForm
                .FirstOrDefaultAsync(x => x.Id == formId);

            if (form != null)
            {
                await context.Entry(form).Reference(x => x.MtdFormHeader).LoadAsync();
                await context.Entry(form).Reference(x => x.MtdFormDesk).LoadAsync();

                formModel.Name = form.Name;
                formModel.Description = form.Description;
                formModel.Active = form.Active == 1;
                formModel.VisibleDate = form.VisibleDate == 1;
                formModel.VisibleNumber = form.VisibleNumber == 1;
                formModel.Sequence = form.Sequence;

                if (form.MtdFormDesk != null)
                {
                    formModel.ImageBack = Convert.ToBase64String(form.MtdFormDesk.Image);
                    formModel.ImageBackType = form.MtdFormDesk.ImageType;
                    formModel.ImageBackSize = form.MtdFormDesk.ImageSize;
                }


                if (form.MtdFormHeader != null)
                {
                    formModel.ImageLogo = Convert.ToBase64String(form.MtdFormHeader.Image);
                    formModel.ImageLogoType = form.MtdFormHeader.ImageType;
                    formModel.ImageLogoSize = form.MtdFormHeader.ImageSize;
                }
            }

            return formModel;
        }


        public string GetJsonPartsString(PartModel[] models)
        {
            return JsonSerializer.Serialize(models, jsonOptions);
        }

        public async Task<PartModel[]> GetFormPartsAsync(string formId)
        {
            var partList = new List<PartModel>();
            var parts = await context.MtdFormPart
                .Where(x => x.MtdFormId == formId)
                .OrderBy(x => x.Sequence)
                .ToListAsync();

            foreach (var part in parts)
            {

                await context.Entry(part)
                    .Reference(x => x.MtdFormPartHeader)
                    .LoadAsync();

                PartModel partModel = new()
                {
                    Id = part.Id,
                    Name = part.Name,
                    Description = part.Description,
                    Sequence = part.Sequence,
                    FormId = part.MtdFormId,
                    Active = part.Active == 1,
                    StyleType = part.MtdSysStyle,
                    Title = part.Title == 1,
                };

                if (part.MtdFormPartHeader != null)
                {
                    partModel.ImageData = Convert.ToBase64String(part.MtdFormPartHeader.Image);
                    partModel.ImageSize = part.MtdFormPartHeader.ImageSize;
                    partModel.ImageType = part.MtdFormPartHeader.ImageType;
                }

                partList.Add(partModel);
            }

            return partList.ToArray();

        }

        public async Task<string> GetJsonFieldsStringAsync(string[] partIds)
        {
            var fieldModels = await GetFieldsAsync(partIds);
            return GetJsonFieldsString(fieldModels);
        }

        public string GetJsonFieldsString(FieldModel[] models)
        {
            return JsonSerializer.Serialize<FieldModel[]>(models, jsonOptions);
        }

        public async Task<FieldModel[]> GetFieldsAsync(string[] partIds)
        {
            List<FieldModel> fieldList = new();

            var fields = await context.MtdFormPartField
                .Where(x => partIds.Contains(x.MtdFormPartId))
                .Include(x => x.MtdFormPartFieldItems)
                .AsSplitQuery()
                .OrderBy(x => x.Sequence)
                .ToListAsync();

            fields.ForEach(field =>
            {
                fieldList.Add(new()
                {
                    Id = field.Id,
                    Active = field.Active == 1,
                    Description = field.Description,
                    Name = field.Name,
                    PartId = field.MtdFormPartId,
                    Readonly = field.ReadOnly == 1,
                    Required = field.Required == 1,
                    Sequence = field.Sequence,
                    SysType = field.MtdSysType,
                    DefaultValue = field.DefaultData,
                    ListItems = field.MtdFormPartFieldItems
                    .Where(x => x.IsDeleted == false)
                    .Select(x => new ListItemModel
                    {
                        Id = x.Id.ToString(),
                        Name = x.Name
                    }).OrderBy(x => x.Name)
                    .ToArray(),
                });
            });

            return fieldList.ToArray();
        }


        public async Task SaveFormAsync(FormModel formModel)
        {
            var mtdCategory = await context.MtdCategoryForm.FirstOrDefaultAsync();
            var mtdForm = await context.MtdForm.FindAsync(formModel.Id) ?? new();

            mtdForm.Name = formModel.Name;
            mtdForm.Description = formModel.Description;
            mtdForm.Active = formModel.Active ? (sbyte)1 : (sbyte)0;
            mtdForm.MtdCategory = mtdCategory.Id;
            mtdForm.Sequence = formModel.Sequence;
            mtdForm.VisibleDate = formModel.VisibleDate ? (sbyte)1 : (sbyte)0;
            mtdForm.VisibleNumber = formModel.VisibleNumber ? (sbyte)1 : (sbyte)0;

            if (string.IsNullOrEmpty(mtdForm.Id))
                await context.MtdForm.AddAsync(mtdForm);

            mtdForm.Id = formModel.Id;
            await context.SaveChangesAsync();

            var mtdFormDesk = await context.MtdFormDesk.FirstOrDefaultAsync(x => x.Id == mtdForm.Id) ?? new();

            if (mtdFormDesk.Id != null && string.IsNullOrEmpty(formModel.ImageBack))
                context.MtdFormDesk.Remove(mtdFormDesk);

            mtdFormDesk.ColorFont = "black";
            mtdFormDesk.ColorBack = "gray";
            mtdFormDesk.Image = Convert.FromBase64String(formModel.ImageBack);
            mtdFormDesk.ImageSize = formModel.ImageBackSize;
            mtdFormDesk.ImageType = formModel.ImageBackType;

            if (string.IsNullOrEmpty(mtdFormDesk.Id) && formModel.ImageBackSize > 0)
            {
                mtdFormDesk.Id = mtdForm.Id;
                await context.MtdFormDesk.AddAsync(mtdFormDesk);
            }

            mtdFormDesk.Id = mtdForm.Id;
            await context.SaveChangesAsync();

            var mtdFormHeader = await context.MtdFormHeader.FirstOrDefaultAsync(x => x.Id == mtdForm.Id) ?? new();

            if (mtdFormHeader.Id != null && string.IsNullOrEmpty(formModel.ImageLogo))
                context.MtdFormHeader.Remove(mtdFormHeader);

            mtdFormHeader.Image = Convert.FromBase64String(formModel.ImageLogo);
            mtdFormHeader.ImageSize = formModel.ImageLogoSize;
            mtdFormHeader.ImageType = formModel.ImageLogoType;

            if (string.IsNullOrEmpty(mtdFormHeader.Id) && formModel.ImageLogoSize > 0)
            {
                mtdFormHeader.Id = mtdForm.Id;
                await context.MtdFormHeader.AddAsync(mtdFormHeader);
            }

            mtdFormHeader.Id = mtdForm.Id;
            await context.SaveChangesAsync();

            var policy = await userHandler.GetPolicyForUserAsync(currentUser);
            var formPolicy = await context.MtdPolicyForms
                .FirstOrDefaultAsync(x => x.MtdForm == mtdForm.Id && x.MtdPolicy == policy.Id);

            if (formPolicy != null) return;

            formPolicy = new MtdPolicyForms
            {
                ViewAll = 1,
                Create = 1,
                DeleteAll = 1,
                EditAll = 1,
                ChangeOwner = 1,
                ExportToExcel = 1,
                ChangeDate = 1,
                MtdPolicy = policy.Id,
                MtdForm = mtdForm.Id,
                Reviewer = 1
            };

            await context.MtdPolicyForms.AddAsync(formPolicy);
            await context.SaveChangesAsync();
        }

        public async Task SavePartAsync(PartModel[] partModels, string formId)
        {
            foreach (var partModel in partModels)
            {
                var part = await context.MtdFormPart.FindAsync(partModel.Id) ?? new();
                part.Name = partModel.Name;
                part.Description = partModel.Description;
                part.Sequence = partModel.Sequence;
                part.Active = partModel.Active ? (sbyte)1 : (sbyte)0;
                part.MtdSysStyle = partModel.StyleType;
                part.MtdFormId = partModel.FormId;
                part.Title = partModel.Title ? (sbyte)1 : (sbyte)0;

                if (string.IsNullOrEmpty(part.Id))
                {
                    part.Id = partModel.Id;
                    await context.MtdFormPart.AddAsync(part);
                }


                part.Id = partModel.Id;
                await context.SaveChangesAsync();

                var mtdPartHeader = await context.MtdFormPartHeader.FirstOrDefaultAsync(x => x.Id == part.Id) ?? new();

                if (mtdPartHeader.Id != null && string.IsNullOrEmpty(partModel.ImageData))
                    context.MtdFormPartHeader.Remove(mtdPartHeader);

                mtdPartHeader.Image = Convert.FromBase64String(partModel.ImageData);
                mtdPartHeader.ImageSize = partModel.ImageSize;
                mtdPartHeader.ImageType = partModel.ImageType;

                if (string.IsNullOrEmpty(mtdPartHeader.Id) && partModel.ImageSize > 0)
                {
                    mtdPartHeader.Id = part.Id;
                    await context.MtdFormPartHeader.AddAsync(mtdPartHeader);
                }

                mtdPartHeader.Id = part.Id;
                await context.SaveChangesAsync();

                var policy = await userHandler.GetPolicyForUserAsync(currentUser);
                var partPolicy = await context.MtdPolicyParts
                    .FirstOrDefaultAsync(x => x.MtdFormPart == part.Id && x.MtdPolicy == policy.Id);

                if (partPolicy != null) continue;

                partPolicy = new MtdPolicyParts
                {
                    MtdPolicy = policy.Id,
                    MtdFormPart = part.Id,
                    Create = 1,
                    Edit = 1,
                    View = 1,
                };

                await context.MtdPolicyParts.AddAsync(partPolicy);
                await context.SaveChangesAsync();

            }

            var parts = await context.MtdFormPart.Where(x => x.MtdFormId == formId).ToListAsync();
            foreach (var part in parts)
            {
                if (!partModels.Any(x => x.Id == part.Id))
                {
                    context.MtdFormPart.Remove(part);
                }
            }

            await context.SaveChangesAsync();
        }

        public async Task SaveFieldAsync(FieldModel[] fieldModels, string formId)
        {
            foreach (var fieldModel in fieldModels)
            {
                var field = await context.MtdFormPartField
                    .Include(x => x.MtdFormPartFieldItems)
                    .AsSplitQuery()
                    .FirstOrDefaultAsync(x => x.Id == fieldModel.Id) ?? new();
                field.Name = fieldModel.Name;
                field.Description = fieldModel.Description;
                field.Required = fieldModel.Required ? (sbyte)1 : (sbyte)0;
                field.ReadOnly = fieldModel.Readonly ? (sbyte)1 : (sbyte)0;
                field.Sequence = fieldModel.Sequence;
                field.Active = fieldModel.Active ? (sbyte)1 : (sbyte)0;
                field.MtdSysType = fieldModel.SysType;
                field.MtdFormPartId = fieldModel.PartId;
                field.DefaultData = fieldModel.DefaultValue;

                if (string.IsNullOrEmpty(field.Id))
                {
                    field.Id = fieldModel.Id;
                    await context.MtdFormPartField.AddAsync(field);
                }

                field.Id = fieldModel.Id;


                if (fieldModel.SysType == 11)
                {
                    foreach (var item in fieldModel.ListItems)
                    {
                        var itemExists = field.MtdFormPartFieldItems
                            .FirstOrDefault(x => x.Id.ToString().ToLower() == item.Id.ToLower());
                        if (itemExists == null)
                            await context.MtdFormPartFieldItems.AddAsync(new MtdFormPartFieldItem
                            {
                                Id = Guid.Parse(item.Id),
                                Name = item.Name,
                                FieldId = field.Id
                            });
                        else
                            itemExists.Name = item.Name;
                    }

                    foreach (var item in field.MtdFormPartFieldItems.Where(x => x.IsDeleted == false))
                    {
                        if (!fieldModel.ListItems.Any(x => x.Id.Equals(item.Id.ToString(), StringComparison.CurrentCultureIgnoreCase)))
                            item.IsDeleted = true;
                    }
                }


                await context.SaveChangesAsync();

            }

            var partIds = await context.MtdFormPart.Where(x => x.MtdFormId == formId).Select(x => x.Id).ToListAsync();
            var fields = await context.MtdFormPartField.Where(x => partIds.Contains(x.MtdFormPartId)).ToListAsync();

            foreach (var field in fields)
            {
                if (!fieldModels.Any(x => x.Id == field.Id))
                {
                    context.MtdFormPartField.Remove(field);
                }
            }

            await context.SaveChangesAsync();



        }
    }
}

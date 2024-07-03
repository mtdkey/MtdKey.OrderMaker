using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MtdKey.InboxMediator.Service;
using MtdKey.OrderMaker.Entity;
using MtdKey.OrderMaker.Models.Controls.MTDSelectList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MtdKey.OrderMaker.Areas.Config.Pages.EmailLoader
{
    public class CreateModel(OrderMakerContext context, IEmailMediatorService mediatorService) : PageModel
    {
        private readonly OrderMakerContext context = context;
        private readonly IEmailMediatorService mediatorService = mediatorService;

        public FormEditorModel FormEditorModel { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(string formId)
        {
            var exsistsIds = await context.TargetForms.Select(x => x.FormId).ToListAsync();

            var formList = await context.MtdForm
                .Where(x => !exsistsIds.Contains(x.Id))
                .AsNoTracking()
                .OrderBy(x => x.Sequence)
                .ToListAsync();

            var fieldList = await context.MtdFormPartField.Include(x => x.MtdFormPartNavigation)
                .AsNoTracking()
                .AsSplitQuery()
                .OrderBy(x => x.Sequence)
                .ToListAsync();

            if (formId != null)
            {
                formList = formList.Where(x => x.Id == formId).ToList();
                fieldList = fieldList.Where(x => x.MtdFormPartNavigation.MtdFormId == formId).ToList();
            }

            var mediators = await mediatorService.GetAllMediatorsAsync();

            formList.ForEach(form => FormEditorModel.FormItems.Add(new MTDSelectListItem { Id = form.Id, Value = form.Name, Selectded = formId == form.Id }));
            fieldList.ForEach(field => FormEditorModel.FieldItems.Add(new MTDSelectListItem { Id = field.Id, Value = field.Name }));
            mediators.ToList().ForEach(mediator => FormEditorModel.MediatorItems.Add(
                new MTDSelectListItem() { Id = mediator.EmailAsKey, Value = mediator.EmailAsKey }));

            FormEditorModel.LocationItems = [new MTDSelectListItem { Id = "subject", Value = "Subject", Localized = true, Selectded = true }];

            return Page();
        }


        public async Task<IActionResult> OnPostAsync()
        {

            try
            {
                await context.Database.BeginTransactionAsync();
                IFormCollection form = await Request.ReadFormAsync();
                var handler = new PostHandler(context, form);
                await handler.SaveHandlerAsync();
                await context.Database.CommitTransactionAsync();

            }
            catch (Exception ex)
            {
                await context.Database.RollbackTransactionAsync();
                return BadRequest(new JsonResult(ex.Message));
            }


            return new OkResult();
        }


    }
}

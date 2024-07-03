using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MtdKey.InboxMediator.Service;
using MtdKey.OrderMaker.Entity;
using MtdKey.OrderMaker.Models.Controls.MTDSelectList;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MtdKey.OrderMaker.Areas.Config.Pages.EmailLoader
{
    public class EditModel(OrderMakerContext context, IEmailMediatorService mediatorService) : PageModel
    {
        private readonly OrderMakerContext context = context;
        private readonly IEmailMediatorService mediatorService = mediatorService;

        public FormEditorModel FormEditorModel { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(string formId)
        {
            if (string.IsNullOrWhiteSpace(formId))
                return NotFound();


            var form = await context.MtdForm.Where(x => x.Id == formId).AsNoTracking().FirstOrDefaultAsync();

            var fieldList = await context.MtdFormPartField.Include(x => x.MtdFormPartNavigation)
                .Where(x => x.MtdFormPartNavigation.MtdFormId == formId)
                .AsNoTracking()
                .AsSplitQuery()
                .OrderBy(x => x.Sequence)
                .ToListAsync();

            var target = await context.TargetForms
                .Include(x => x.TargetFields)
                .Include(x => x.TargetFilters)
                .Where(x => x.FormId == formId)
                .AsSplitQuery()
                .AsNoTracking()
                .FirstOrDefaultAsync();

            FormEditorModel.FormId = formId;
            FormEditorModel.SenderId = target.TargetFields.Where(x => x.Target == (int)RenderTarget.From).Select(x=>x.FieldId).FirstOrDefault();
            FormEditorModel.SubjectId = target.TargetFields.Where(x => x.Target == (int)RenderTarget.Subject).Select(x => x.FieldId).FirstOrDefault();
            FormEditorModel.BodyId = target.TargetFields.Where(x => x.Target == (int)RenderTarget.Body).Select(x => x.FieldId).FirstOrDefault();
            FormEditorModel.AttachmentsId = target.TargetFields.Where(x => x.Target == (int)RenderTarget.Attachments).Select(x => x.FieldId).FirstOrDefault();
            FormEditorModel.EmailAsKey = target.TargetFilters.Select(x => x.EmailAsKey).FirstOrDefault();
            FormEditorModel.KeyWord = target.TargetFilters.Select(x => x.Filter).FirstOrDefault();

            var mediators = await mediatorService.GetAllMediatorsAsync();

            FormEditorModel.FormItems.Add(new MTDSelectListItem { Id = form.Id, Value = form.Name, Selectded = true });
            
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

        public async Task<IActionResult> OnPostDeleteAsync()
        {
            try
            {
                IFormCollection form = await Request.ReadFormAsync();
                var formId = form["formId"];
                context.TargetForms.Remove(new TargetForm { FormId = formId });
                await context.SaveChangesAsync();

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

using MtdKey.OrderMaker.Entity;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace MtdKey.OrderMaker.Areas.Config.Pages.EmailLoader
{
    public class PostHandler(OrderMakerContext context, IFormCollection form)
    {

        private readonly OrderMakerContext context = context;
        private readonly IFormCollection form = form;

        public async Task SaveHandlerAsync()
        {

            var formId = form["formId"];
            var sender = form["sender"];
            var subject = form["subject"];
            var body = form["body"];
            var attachments = form["attachments"];
            var emailMediator = form["mediator"];
            var keyword = form["keyword"];

            await FormTemplateHandlerAsync(formId);
            await FieldsHandlerAsync(formId, sender, RenderTarget.From);
            await FieldsHandlerAsync(formId, subject, RenderTarget.Subject);
            await FieldsHandlerAsync(formId, body, RenderTarget.Body);
            await FieldsHandlerAsync(formId, attachments, RenderTarget.Attachments);

            await FilterHandlerAsync(formId, emailMediator, keyword);
          
        }

        private async Task FieldsHandlerAsync(string formId, string fieldId, RenderTarget target)
        {
            var targetField = await context.TargetFields
                .Where(x => x.FormId == formId && x.Target == (int)target)
                .FirstOrDefaultAsync();

            if (targetField == null)
            {
                targetField = new TargetField { FormId = formId, FieldId = fieldId, Target = (int) target };
                await context.AddRangeAsync(targetField);
                await context.SaveChangesAsync();
            }
            else
            {
                targetField.FieldId = fieldId;
                await context.SaveChangesAsync();
            }
        }

        private async Task FormTemplateHandlerAsync(string formId)
        {
            var targetForm = await context.TargetForms.FindAsync(formId);
            if (targetForm == null)
            {
                await context.TargetForms.AddAsync(new TargetForm { FormId = formId });
                await context.SaveChangesAsync();
            }
        }

        private async Task FilterHandlerAsync(string formId, string emailMediator, string keyword)
        {
            var targetFilter = await context.TargetFilters
                    .Where(x => x.FormId == formId && x.EmailAsKey == emailMediator)
                    .FirstOrDefaultAsync();

            var value = keyword.Trim().Length > 0 ? keyword.Trim() : null;
            if (targetFilter == null)
            {
                await context.TargetFilters.AddAsync(new TargetFilter { FormId = formId, EmailAsKey = emailMediator, Filter = value });
                await context.SaveChangesAsync();
            }
            else
            {
                targetFilter.Filter = value;
                await context.SaveChangesAsync();
            }
        }

    }
}

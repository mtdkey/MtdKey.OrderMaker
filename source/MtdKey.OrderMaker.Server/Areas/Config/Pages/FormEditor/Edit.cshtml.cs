using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MtdKey.OrderMaker.Entity;
using MtdKey.OrderMaker.Services;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace MtdKey.OrderMaker.Areas.Config.Pages.FormEditor
{
    public class EditModel(OrderMakerContext context, UserHandler userHandler) : PageModel
    {
        private readonly OrderMakerContext context = context;
        private readonly UserHandler userHandler = userHandler;

        public MtdForm MtdForm { get; set; } = new MtdForm();
        public string JsonData { get; set; } = string.Empty;

        public async Task<IActionResult> OnGetAsync(string formId)
        {
            if (string.IsNullOrEmpty(formId)) return NotFound();
            var currentUser = await userHandler.GetUserAsync(HttpContext.User);
            MtdForm = await context.MtdForm.FindAsync(formId);
            var formBuilder = new FormBuilderCore(context, currentUser, userHandler);
            JsonData = await formBuilder.GetJsonFormDataAsync(formId);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var form = await Request.ReadFormAsync();
            var jsonData = form["jsonData"];
            FormDataModel model;

            try
            {
                model = JsonSerializer.Deserialize<FormDataModel>(jsonData);
            }
            catch (JsonException ex)
            {
                return new ObjectResult(new
                {
                    Title = "Invalid Request",
                    Detail = ex.Message,
                    Status = StatusCodes.Status400BadRequest
                })
                {
                    StatusCode = StatusCodes.Status400BadRequest
                };
            }

            var currentUser = await userHandler.GetUserAsync(HttpContext.User);
            var formBuilder = new FormBuilderCore(context, currentUser, userHandler);

            try
            {
                await formBuilder.CheckPolicy();
            }
            catch (Exception ex)
            {
                return new ObjectResult(new
                {
                    Title = "Invalid Request",
                    Detail = ex.Message,
                    Status = StatusCodes.Status403Forbidden
                })
                {
                    StatusCode = StatusCodes.Status403Forbidden
                };

            }

            var strategy = context.Database.CreateExecutionStrategy();
            return await strategy.ExecuteAsync(async () =>
            {
                await using var transaction = await context.Database.BeginTransactionAsync();
                try
                {
                    await formBuilder.SaveFormAsync(model.FormModel);
                    await formBuilder.SavePartAsync(model.PartModels, model.FormModel.Id);
                    await formBuilder.SaveFieldAsync(model.FieldModels, model.FormModel.Id);

                    await transaction.CommitAsync();
                    return new OkObjectResult(model);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return new ObjectResult(new
                    {
                        Title = "Invalid Request",
                        Detail = ex.Message,
                        Status = StatusCodes.Status500InternalServerError
                    })
                    {
                        StatusCode = StatusCodes.Status500InternalServerError
                    };
                }
            });
        }

    }
}

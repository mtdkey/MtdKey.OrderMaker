using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MtdKey.OrderMaker.Entity;
using MtdKey.OrderMaker.Services;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace MtdKey.OrderMaker.Areas.Config.Pages.FormEditor
{
    public class EditModel(DataConnector context, UserHandler userHandler) : PageModel
    {
        private readonly DataConnector context = context;
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
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }


            var currentUser = await userHandler.GetUserAsync(HttpContext.User);
            var formBuilder = new FormBuilderCore(context, currentUser, userHandler);

            try
            {
                await formBuilder.CheckPolicy();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }



            await context.Database.BeginTransactionAsync();

            try
            {
                await formBuilder.SaveFormAsync(model.FormModel);
                await formBuilder.SavePartAsync(model.PartModels, model.FormModel.Id);
                await formBuilder.SaveFieldAsync(model.FieldModels, model.FormModel.Id);

                await context.Database.CommitTransactionAsync();

            }
            catch (Exception ex)
            {
                await context.Database.RollbackTransactionAsync();
                return BadRequest(ex.Message);
            }

            return new OkResult();
        }
    }
}

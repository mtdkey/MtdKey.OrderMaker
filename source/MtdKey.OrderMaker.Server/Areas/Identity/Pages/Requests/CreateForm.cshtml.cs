using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using MtdKey.OrderMaker.Services;
using System;
using System.Threading.Tasks;

namespace MtdKey.OrderMaker.Areas.Identity.Requests
{
    public class CreateFormModel(RegistrationService registrationService, ILogger<CreateFormModel> logger) : PageModel
    {
        private readonly RegistrationService registrationService = registrationService;
        private readonly ILogger<CreateFormModel> logger = logger;

        public async Task<IActionResult> OnGetAsync(string token)
        {

            var pageRequest = await registrationService.PageRequestAccepting();
            var apiRequest = await registrationService.APIRequestAccepting();

            if (!pageRequest && !apiRequest)
                return NotFound();

            try
            {
                var validToken = await registrationService.TokenValidateAsync(token);
                if (validToken is not true) { return RedirectToPage("/Index"); }

                var formId = await registrationService.GetFormIdByTokenAsync(token);
                await registrationService.AcceptUserByToken(token);

                return LocalRedirect($"/workplace/store/create?formId={formId}");
            }
            catch (Exception ex)
            {

                logger.LogWarning("{message}", ex.Message);
                return BadRequest();
            }
       
        }
    }
}

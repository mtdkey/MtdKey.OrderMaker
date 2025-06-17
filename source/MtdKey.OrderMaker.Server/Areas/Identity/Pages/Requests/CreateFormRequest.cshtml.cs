using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Build.Framework;
using Microsoft.Extensions.Logging;
using MtdKey.OrderMaker.Core;
using MtdKey.OrderMaker.Models.Controls.MTDSelectList;
using MtdKey.OrderMaker.Services;
using MtdKey.OrderMaker.Services.Tokens;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MtdKey.OrderMaker.Areas.Identity.Pages.Requests
{
    public class CreateFormRequestModel(RegistrationService registrationService, ILogger<CreateFormRequestModel> logger) : PageModel
    {
        private readonly RegistrationService registrationService = registrationService;
        private readonly ILogger<CreateFormRequestModel> logger1 = logger;
        public List<MTDSelectListItem> Forms { get; set; } = [];
        public async Task<IActionResult> OnGetAsync()
        {
            if (await registrationService.PageRequestAccepting() is not true)
                return NotFound();

            var forms = await registrationService.GetPublicFormsAsync();
            foreach (var form in forms)
            {
                Forms.Add(new MTDSelectListItem { Id = form.Key, Value = form.Value, Selectded = Forms.Count == 0 });
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                var form = await Request.ReadFormAsync();
                var requestToken = new RequestFormToken()
                {
                    Email = form["email"],
                    UserName = form["userName"],
                    FormId = form["form"]
                };

                var hostInfo = new HostInfo(Request.Scheme, Request.Host.Value);
                await registrationService.SendTokenByEmailAsync(requestToken, hostInfo);

            } catch(Exception ex)
            {
                logger1.LogWarning("{message}",ex.Message);
                return BadRequest(ex.Message);
            }


            return new OkResult();
        }
    }
}

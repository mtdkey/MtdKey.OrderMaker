using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MtdKey.OrderMaker.Core;
using MtdKey.OrderMaker.Services;
using MtdKey.OrderMaker.Services.Tokens;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MtdKey.OrderMaker.Areas.Public.Pages
{

    public class RequestModel(RegistrationService registrationService) : PageModel
    {

        private readonly RegistrationService registrationService = registrationService;
        public Dictionary<string, string> Forms { get; set; } = [];


        public async Task<IActionResult> OnGetAsync()
        {               
            Forms = await registrationService.GetPublicFormsAsync();
            return Page();
        }




        public async Task<IActionResult> OnPostAsync()
        {
            var form = await Request.ReadFormAsync();
            var requestToken = new RequestFormToken()
            {
                Email = form["email"],
                UserName = form["name"],
                FormId = form["form"]
            };

            var hostInfo = new HostInfo(Request.Scheme, Request.Host.Value);
            await registrationService.SendTokenByEmailAsync(requestToken, hostInfo);

            return RedirectToPage("./Request");
        }
    }
}

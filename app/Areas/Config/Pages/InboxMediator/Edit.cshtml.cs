using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MtdKey.InboxMediator.Data;
using MtdKey.InboxMediator.Service;
using System.Threading.Tasks;

namespace MtdKey.OrderMaker.Areas.Config.Pages.InboxMediator
{
    public class EditModel(IEmailMediatorService emailMediator) : PageModel
    {
        private readonly IEmailMediatorService emailMediator = emailMediator;

        [BindProperty]
        public EmailMediator EmailMediator { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(string email)
        {
            EmailMediator = await emailMediator.GetMediatorAsync(email);
            if(EmailMediator == null) { return RedirectToPage("./Index"); }

            return Page();
        }


        public async Task<IActionResult> OnPostAsync()
        {
            await emailMediator.UpdateMediatorAsync(EmailMediator);
            return new OkResult();
        }
    }
}

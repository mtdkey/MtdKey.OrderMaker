using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MtdKey.InboxMediator.Data;
using MtdKey.InboxMediator.Service;
using MtdKey.OrderMaker.Entity;
using System.Threading.Tasks;

namespace MtdKey.OrderMaker.Areas.Config.Pages.InboxMediator
{
    public class CreateModel(IEmailMediatorService emailMediator) : PageModel
    {
        private readonly IEmailMediatorService emailMediator = emailMediator;

        public void OnGet()
        {
        }

        [BindProperty]
        public  EmailMediator EmailMediator { get; set; }
        public async Task<IActionResult> OnPostAsync()
        {
            await emailMediator.AddMediatorAsync(EmailMediator);
            return  new OkResult();
        }
    }
}

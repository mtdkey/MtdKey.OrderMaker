using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MtdKey.OrderMaker.AppConfig;
using MtdKey.OrderMaker.Services;
using System.Threading.Tasks;

namespace MtdKey.OrderMaker.Areas.Identity.Pages.Users.Registration
{
    public class IndexModel(IAppParams appParams) : PageModel
    {
        private readonly IAppParams _appParams = appParams;
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync() {
            await _appParams.SaveAsync(ParamId.RegisterByPage, "true");
            return RedirectToPage("./Index");
        }
    }
}

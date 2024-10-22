using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using MtdKey.OrderMaker.Areas.Identity.Data;
using MtdKey.OrderMaker.Core;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MtdKey.OrderMaker.Areas.Identity.Pages.Account
{
    public class ReLoginModel : PageModel
    {

        private UserManager<WebAppUser> userManager;
        private readonly ILogger<ReLoginModel> _logger;

        public ReLoginModel(UserManager<WebAppUser> userManager, ILogger<ReLoginModel> logger)
        {
            this.userManager = userManager;
            _logger = logger;
        }

        public async Task<IActionResult> OnGetAsync(string returnUrl)
        {
            WebAppUser user = await userManager.GetUserAsync(HttpContext.User);
            if (user == null) { return RedirectToPage("/Identity/Account/Login"); }

            await userManager.RemoveClaimAsync(user, new Claim("revoke", "false"));
            await HttpContext.RefreshLoginAsync();

            var targetUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}{returnUrl}";
            _logger.LogWarning($"Referer: {targetUrl}");
            return Redirect(targetUrl);

        }
    }
}

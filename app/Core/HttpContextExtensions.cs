using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using MtdKey.OrderMaker.Areas.Identity.Data;
using System.Threading.Tasks;

namespace MtdKey.OrderMaker.Core
{
    public static class HttpContextExtensions
    {
        public static async Task RefreshLoginAsync(this HttpContext context)
        {
            if (context.User == null)
                return;

            var userManager = context.RequestServices
                .GetRequiredService<UserManager<WebAppUser>>();
            var signInManager = context.RequestServices
                .GetRequiredService<SignInManager<WebAppUser>>();

            WebAppUser user = await userManager.GetUserAsync(context.User);

            if (signInManager.IsSignedIn(context.User))
            {
                await signInManager.RefreshSignInAsync(user);
            }
        }
    }
}

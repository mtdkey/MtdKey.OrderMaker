using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MtdKey.OrderMaker.Areas.Identity.Data;
using MtdKey.OrderMaker.Entity;
using MtdKey.OrderMaker.Services;
using MtdKey.Cipher;
using System;
using System.Linq;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace MtdKey.OrderMaker.Areas.Identity.Pages.Account
{
    public class TokenLoginModel : PageModel
    {
        private readonly IAesManager aesManager;
        private readonly OrderMakerContext context;
        private readonly UserHandler userHandler;
        private readonly SignInManager<WebAppUser> signInManager;

        public TokenLoginModel(IAesManager aesManager,
            OrderMakerContext context, UserHandler userHandler,
            SignInManager<WebAppUser> signInManager)
        {
            this.userHandler = userHandler;
            this.aesManager = aesManager;
            this.context = context;
            this.signInManager = signInManager;
        }

        [BindProperty]
        public string Token { get; set; }

        public IActionResult OnGetAsync(string token)
        {
            var tokeModel = aesManager.DecryptModel(token);
            if (tokeModel == null) { return NotFound(); }
            Token = token;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var tokeModel = aesManager.DecryptModel(Token);
            if (tokeModel == null) { return BadRequest("Wrong token!"); }

            try
            {
                var cookie = HttpContext.Request.Cookies;
                var jsonData = JsonObject.Parse(tokeModel.Data);
                var email = jsonData["email"].GetValue<string>();
                var password = userHandler.GeneratePassword();

                var databaseId = Guid.NewGuid();
                var user = new WebAppUser
                {
                    Email = email,
                    UserName = email,
                    Title = "Administrator",
                    DatabaseId = databaseId,
                    EmailConfirmed = true,
                };

                var userExists = await userHandler.FindByEmailAsync(user.Email);
                if (userExists != null)
                {
                    await signInManager.SignInAsync(userExists, isPersistent: true);
                    return new OkResult();
                }

                var result = await userHandler.CreateAsync(user, password);

                if (!result.Succeeded)
                {
                    var error = result.Errors.FirstOrDefault();
                    if (error == null) { BadRequest("Bad request!"); }

                    return BadRequest($"{error.Code} {error.Description}");
                }

                await userHandler.AddToRoleAsync(user, "Admin");
                await signInManager.PasswordSignInAsync(user, password, true, true);
                await signInManager.SignInAsync(user, isPersistent: true);

                context.SetDatabase(databaseId);
                await MigrationService.MigrationHandlerAsync(context);

                return new OkResult();

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}

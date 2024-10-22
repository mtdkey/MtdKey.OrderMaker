using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using MtdKey.OrderMaker.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace MtdKey.OrderMaker.Areas.Identity.Pages.Account.Manage
{
    public class ChangePasswordModel : PageModel
    {
        private readonly UserManager<WebAppUser> _userManager;
        private readonly SignInManager<WebAppUser> _signInManager;
        private readonly ILogger<ChangePasswordModel> _logger;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public ChangePasswordModel(
            UserManager<WebAppUser> userManager,
            SignInManager<WebAppUser> signInManager,
            ILogger<ChangePasswordModel> logger, IStringLocalizer<SharedResource> localizer)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _localizer = localizer;
        }

        [BindProperty]
        public string UserName { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        public class InputModel
        {
            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "Current password")]
            public string OldPassword { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 8)]
            [DataType(DataType.Password)]
            [Display(Name = "New password")]
            public string NewPassword { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm new password")]
            [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            UserName = user.UserName;
            var hasPassword = await _userManager.HasPasswordAsync(user);
            if (!hasPassword)
            {
                return RedirectToPage("./SetPassword");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }


            bool checkCurrent = await _userManager.CheckPasswordAsync(user, Input.OldPassword);
            if (!checkCurrent)
            {
                return BadRequest(_localizer["Invalid current password."]);
            }


            if (Input.NewPassword != Input.ConfirmPassword)
            {
                return BadRequest(_localizer["The new password and confirmation password do not match."]);
            }


            PasswordValidator<WebAppUser> passwordValidator = new();
            var checkPassword = await passwordValidator.ValidateAsync(_userManager, null, Input.NewPassword);
            if (!checkPassword.Succeeded)
            {
                return BadRequest(_localizer["The new password is not strong enough."]);
            }

            var changePasswordResult = await _userManager.ChangePasswordAsync(user, Input.OldPassword, Input.NewPassword.Trim());
            if (!changePasswordResult.Succeeded)
            {
                //foreach (var error in changePasswordResult.Errors)
                //{
                //    ModelState.AddModelError(string.Empty, error.Description);
                //}
                return BadRequest(_localizer["Error."]);
            }

            await _signInManager.RefreshSignInAsync(user);
            //_logger.LogInformation("User changed their password successfully.");

            //StatusMessage = "Your password has been changed.";

            return RedirectToPage("/Index");
        }
    }
}

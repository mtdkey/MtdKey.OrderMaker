using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;
using MtdKey.OrderMaker.Areas.Identity.Data;
using MtdKey.OrderMaker.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace MtdKey.OrderMaker.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<WebAppUser> _userManager;
        private readonly SignInManager<WebAppUser> _signInManager;
        private readonly IEmailSenderBlank _emailSender;
        private readonly IStringLocalizer<SharedResource> _localizer;


        public IndexModel(
            UserManager<WebAppUser> userManager,
            SignInManager<WebAppUser> signInManager,
            IEmailSenderBlank emailSender,
            IStringLocalizer<SharedResource> localizer)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _localizer = localizer;

        }

        [BindProperty]
        public string UserName { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            public string Password { get; set; }

            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "Full name")]
            public string UserTitle { get; set; }

            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var userName = await _userManager.GetUserNameAsync(user);
            var email = await _userManager.GetEmailAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            UserName = userName;

            Input = new InputModel
            {
                UserTitle = user.Title,
                Email = email,
                PhoneNumber = phoneNumber
            };

            //IsEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return BadRequest(_localizer[$"Unable to load user with ID '{_userManager.GetUserId(User)}'."]);
            }

            bool check = await _userManager.CheckPasswordAsync(user, Input.Password);
            if (!check)
            {
                return BadRequest(_localizer["Wrong password!"]);
            }

            user.EmailConfirmed = true;

            if (Input.UserTitle != user.Title)
            {
                user.Title = Input.UserTitle;
            }

            var email = await _userManager.GetEmailAsync(user);
            if (Input.Email != email)
            {
                var setEmailResult = await _userManager.SetEmailAsync(user, Input.Email);
                if (!setEmailResult.Succeeded)
                {
                    var userId = await _userManager.GetUserIdAsync(user);
                    throw new InvalidOperationException($"Unexpected error occurred setting email for user with ID '{userId}'.");
                }
            }


            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    var userId = await _userManager.GetUserIdAsync(user);
                    throw new InvalidOperationException($"Unexpected error occurred setting phone number for user with ID '{userId}'.");
                }
            }

            await _userManager.UpdateAsync(user);

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = _localizer["Your profile has been updated."];
            return RedirectToPage();
        }

        /*NOT USED*/
        public async Task<IActionResult> OnPostSendVerificationEmailAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var userId = await _userManager.GetUserIdAsync(user);
            var email = await _userManager.GetEmailAsync(user);
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var callbackUrl = Url.Page(
                "/Account/ConfirmEmail",
                pageHandler: null,
                values: new { userId, code },
                protocol: Request.Scheme);

            StatusMessage = _localizer["Verification email sent. Please check your email."];

            BlankEmail blankEmail = new()
            {
                Email = email,
                Subject = _localizer["Email Verification Procedure"],
                Header = _localizer["Email Verification Procedure"],
                Content = new List<string>()
                       {
                           _localizer["Confirm the ownership of the mailbox by clicking on the link below"],
                           $"<a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>{_localizer["Email Verification"]}</a>"
                       }
            };

            try
            {
                await _emailSender.SendEmailBlankAsync(blankEmail, false);

            }
            catch
            {
                StatusMessage = "Error sending message. Perhaps this address does not exist.";
            }

            return RedirectToPage("/Index");
        }
    }
}

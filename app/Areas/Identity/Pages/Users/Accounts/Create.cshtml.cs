/*
    MTD OrderMaker - http://mtdkey.com
    Copyright (c) 2019 Oleg Bruev <job4bruev@gmail.com>. All rights reserved.
*/

using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Security.Cryptography;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MtdKey.OrderMaker.Areas.Identity.Data;
using MtdKey.OrderMaker.AppConfig;
using Microsoft.Extensions.Localization;
using MtdKey.OrderMaker.Services;
using System.Collections.Generic;
using System.Linq;

namespace MtdKey.OrderMaker.Areas.Identity.Pages.Users.Accounts
{
    public partial class CreateModel : PageModel
    {
        private readonly UserHandler _userManager;
        private readonly IEmailSenderBlank _emailSender;
        private readonly ILogger<CreateModel> _logger;
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly IOptions<LimitSettings> limits;

        public CreateModel(
            UserHandler userManager,
            IEmailSenderBlank emailSender,
            ILogger<CreateModel> logger,
            IStringLocalizer<SharedResource> localizer,
            IOptions<LimitSettings> limits)
        {
            _userManager = userManager;
            _emailSender = emailSender;
            _logger = logger;
            _localizer = localizer;
            this.limits = limits;
        }

        [BindProperty]
        public string UserName { get; set; }
        [BindProperty]
        public bool IsEmailConfirmed { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {

            public string Id { get; set; }

            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "Full name")]
            public string Title { get; set; }

            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "User name")]
            public string UserName { get; set; }

            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Display(Name = "Send Email")]
            public bool SendEmail { get; set; }

            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }
        }

        public void OnGet()
        {
            Input = new InputModel { Id = Guid.NewGuid().ToString() };
        }


        public async Task<IActionResult> OnPostAsync()
        {

            int userQty = _userManager.Users.Count();
            int userLimit = limits.Value.Users;

            if (userQty >= userLimit) { return BadRequest(_localizer["Limit users!"]); }

            string pass = _userManager.GeneratePassword();
            var curentUser = await _userManager.GetUserAsync(User);

            var user = new WebAppUser
            {
                Id = Input.Id,
                Title = Input.Title,
                UserName = Input.Email,
                Email = Input.Email,
                EmailConfirmed = Input.SendEmail,
                PhoneNumber = Input.PhoneNumber,
                DatabaseId = curentUser.DatabaseId
            };

            IdentityResult result;
            result = await _userManager.CreateAsync(user, pass);


            await _userManager.AddToRoleAsync(user, "Guest");


            if (!result.Succeeded)
            {

                IdentityError text = result.Errors.FirstOrDefault() ?? new IdentityError();
                return BadRequest(_localizer[text.Description]);
            }

            return RedirectToPage("./Edit", new { id = Input.Id });

        }
    }
}
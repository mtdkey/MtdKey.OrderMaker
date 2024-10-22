/*
    MTD OrderMaker - http://mtdkey.com
    Copyright (c) 2019 Oleg Bruev <job4bruev@gmail.com>. All rights reserved.
*/

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MtdKey.OrderMaker.Areas.Identity.Data;
using MtdKey.OrderMaker.Services;
using System;
using System.Collections.Generic;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace MtdKey.OrderMaker.Controllers.Users
{
    [Authorize(Roles = "Admin")]
    public partial class UsersController : ControllerBase
    {

        [HttpPost("admin/confirm/email")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostAdminConfimEmailAsync()
        {
            string userName = Request.Form["UserName"];
            WebAppUser user = await _userManager.FindByNameAsync(userName);

            if (user == null)
            {
                return NotFound();
            }

            string userId = await _userManager.GetUserIdAsync(user);
            string email = await _userManager.GetEmailAsync(user);
            string code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            string callbackUrl = Url.Page(
                "/Account/ConfirmEmail",
                pageHandler: null,
                values: new { area = "Identity", userId, code },
                protocol: Request.Scheme);

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


            bool isOk = await _emailSender.SendEmailBlankAsync(blankEmail, false);
            if (!isOk) { return BadRequest(_localizer["Error sending email."]); }

            user.EmailConfirmed = false;
            await _userManager.UpdateAsync(user);

            return Ok();
        }

        [HttpPost("admin/confirm/password")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostAdminConfirmPasswordAsync()
        {

            string userName = Request.Form["UserName"];
            WebAppUser user = await _userManager.FindByNameAsync(userName);

            if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
            {
                return NotFound();
            }

            string email = await _userManager.GetEmailAsync(user);
            string code = await _userManager.GeneratePasswordResetTokenAsync(user);
            var callbackUrl = Url.Page(
                "/Account/ResetPassword",
                pageHandler: null,
                values: new { area = "Identity", code },
                protocol: Request.Scheme);

            BlankEmail blankEmail = new()
            {
                Email = email,
                Subject = _localizer["Password reset"],
                Header = _localizer["Password reset"],
                Content = new List<string>()
                       {
                           $"{_localizer["Your login"]}: <strong>{user.UserName}</strong>",
                           _localizer["To change your account password, follow the link below"],
                           $"<a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>{_localizer["Create account password"]}</a>"
                       }
            };

            bool isOk = await _emailSender.SendEmailBlankAsync(blankEmail);
            if (!isOk) { return BadRequest(_localizer["Error sending email."]); }

            return Ok();
        }

        [HttpPost("admin/create/password")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostAdminCreatePasswordAsync()
        {

            string userName = Request.Form["UserName"];
            WebAppUser user = await _userManager.FindByNameAsync(userName);

            if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
            {
                return NotFound();
            }

            string email = await _userManager.GetEmailAsync(user);
            string password = _userManager.GeneratePassword();

            await _userManager.RemovePasswordAsync(user);
            await _userManager.AddPasswordAsync(user, password);
            await _userManager.SetLockoutEndDateAsync(user, new DateTimeOffset(DateTime.UtcNow));


            var callbackUrl = Url.Page(
                     "/Account/Login",
                     pageHandler: null,
                     values: new { area = "Identity" },
                     protocol: Request.Scheme);

            BlankEmail blankEmail = new()
            {
                Email = email,
                Subject = _localizer["New password for access"],
                Header = _localizer["New password"],
                Content = new List<string>()
                       {
                           $"{_localizer["You have been granted access to the system and a new password has been created"]}.",
                           $"{_localizer["Your login"]}: <strong>{user.UserName}</strong>",
                           $"{_localizer["Your new password"]}: <strong>{password}</strong>",
                           $"{_localizer["Web-application address"]}: <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>{Request.Scheme}://{Request.Host}</a>",
                       }
            };

            bool isOk = await _emailSender.SendEmailBlankAsync(blankEmail);
            if (!isOk) { return BadRequest(_localizer["Error sending email."]); }

            return Ok();
        }

    }
}

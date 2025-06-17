/*
    MTD OrderMaker - http://mtdkey.com
    Copyright (c) 2019 Oleg Bruev <job4bruev@gmail.com>. All rights reserved.
*/

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using MtdKey.OrderMaker.Areas.Identity.Data;
using MtdKey.OrderMaker.Core;
using MtdKey.OrderMaker.Entity;
using MtdKey.OrderMaker.Services;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MtdKey.OrderMaker.Controllers.Users
{
    [Route("api/users")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public partial class UsersController : ControllerBase
    {
        private readonly UserHandler _userManager;
        private readonly RoleManager<WebAppRole> _roleManager;
        private readonly IEmailSenderBlank _emailSender;
        private readonly OrderMakerContext _context;
        private readonly IStringLocalizer<SharedResource> _localizer;


        public UsersController(
            UserHandler userManager,
            RoleManager<WebAppRole> roleManager,
            IEmailSenderBlank emailSender,
            OrderMakerContext context,
            IStringLocalizer<SharedResource> localizer, IdentityDbContext identity
            )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _emailSender = emailSender;
            _context = context;
            _localizer = localizer;
        }

        [HttpGet("admin/user/{userId}")]
        public async Task<IActionResult> OnPostGetUserAsync(string userId)
        {
            WebAppUser user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return BadRequest(_localizer["ERROR! User not found."]);
            }

            return new JsonResult(new { user.Id, titleName = user.Title, titleGroup = user.TitleGroup, fullName = user.GetFullName() });
        }

        [HttpPost("admin/delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostAdminDeleteAsync()
        {
            string userId = Request.Form["user-delete-id"];
            WebAppUser user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return BadRequest(_localizer["ERROR! User not found."]);
            }

            bool isApprover = await _context.MtdApprovalStage.Where(x => x.UserId == user.Id).AnyAsync();
            bool isOwner = await _context.MtdStoreOwner.Where(x => x.UserId == user.Id).AnyAsync();

            if (isApprover || isOwner)
            {
                return BadRequest(_localizer["ERROR! There are documents owned by the user. Before deleting, transfer of documents to another user."]);
            }

            IList<MtdFilter> mtdFilters = await _context.MtdFilter.Where(x => x.IdUser == user.Id).ToListAsync();
            _context.MtdFilter.RemoveRange(mtdFilters);
            await _context.SaveChangesAsync();
            await _userManager.DeleteAsync(user);

            return Ok();
        }

        [HttpPost("admin/profile")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostAdminProfileAsync()
        {

            var form = await HttpContext.Request.ReadFormAsync();

            string username = form["UserName"];

            if (username == null)
            {
                return BadRequest(_localizer["Error. User name is null."]);
            }

            var user = await _userManager.FindByNameAsync(username);

            if (user == null)
            {
                return BadRequest(_localizer["Error. User not found."]);
            }

            string email = form["Input.Email"];
            string title = form["Input.Title"];
            string titleGroup = form["Input.TitleGroup"];
            string phone = form["Input.PhoneNumber"];
            string roleId = form["Input.Role"];
            string policyId = form["Input.Policy"];

            string groupContainer = form["groupContainer"];

            WebAppRole roleUser = await _roleManager.FindByIdAsync(roleId);


            string[] formConfirm = form["Input.IsConfirm"];
            bool isConfirm = false;
            if (formConfirm.FirstOrDefault() != null)
            {
                isConfirm = bool.Parse(formConfirm.FirstOrDefault());
            }

            if (user.Email != email)
            {
                user.Email = email;
                user.EmailConfirmed = false;
            }

            if (user.Title != title)
            {
                user.Title = title;
            }

            if (user.TitleGroup != titleGroup)
            {
                user.TitleGroup = titleGroup;
            }

            if (user.PhoneNumber != phone)
            {
                user.PhoneNumber = phone;
            }

            if (isConfirm)
            {
                user.EmailConfirmed = true;
            }

            await _userManager.UpdateAsync(user);
            IList<string> roles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, roles);
            await _userManager.AddToRoleAsync(user, roleUser.Name);


            IEnumerable<Claim> claims = await _userManager.GetClaimsAsync(user);
            await _userManager.RemoveClaimsAsync(user, claims);

            Claim claim = new("policy", policyId);
            await _userManager.AddClaimAsync(user, claim);

            List<string> groupIds = groupContainer.Split("&").Where(x => x != "").ToList();
            foreach (var groupId in groupIds)
            {
                Claim claimGroup = new("group", groupId);
                await _userManager.AddClaimAsync(user, claimGroup);
            }

            await _userManager.AddClaimAsync(user, new Claim("revoke", "false"));

            return Ok();
        }

        [HttpPost("admin/transfer")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostAdminTranferAsync()
        {
            string UserOwner = Request.Form["user-owner-id"];
            string UserRecipient = Request.Form["user-recipient-id"];

            WebAppUser userOwner = await _userManager.FindByIdAsync(UserOwner);
            WebAppUser userRecipient = await _userManager.FindByIdAsync(UserRecipient);

            if (userOwner == null || userRecipient == null)
            {
                return BadRequest(_localizer["ERROR! User not found."]);
            }

            IList<MtdStoreOwner> storeOwners = await _context.MtdStoreOwner.Where(x => x.UserId == userOwner.Id).ToListAsync();
            foreach (MtdStoreOwner owner in storeOwners)
            {
                owner.UserId = userRecipient.Id;
                owner.UserName = userRecipient.Title;
            }

            IList<MtdApprovalStage> stages = await _context.MtdApprovalStage.Where(x => x.UserId == userOwner.Id).ToListAsync();
            foreach (MtdApprovalStage stage in stages)
            {
                stage.UserId = userRecipient.Id;
            }

            _context.MtdStoreOwner.UpdateRange(storeOwners);
            _context.MtdApprovalStage.UpdateRange(stages);
            await _context.SaveChangesAsync();

            return Ok();
        }

    }
}
/*
    MTD OrderMaker - http://mtdkey.com
    Copyright (c) 2019 Oleg Bruev <job4bruev@gmail.com>. All rights reserved.
*/

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MtdKey.OrderMaker.Areas.Identity.Data;
using MtdKey.OrderMaker.Entity;
using MtdKey.OrderMaker.AppConfig;
using MtdKey.OrderMaker.Services;
using MtdKey.OrderMaker.Models.Controls.MTDSelectList;
using MtdKey.OrderMaker.Core;

namespace MtdKey.OrderMaker.Areas.Identity.Pages.Users.Accounts
{

    public class GroupModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
    }

    public partial class EditModel : PageModel
    {

        private readonly UserHandler _userManager;
        private readonly RoleManager<WebAppRole> _roleManager;
        private readonly DataConnector _context;
        private readonly IOptions<ConfigSettings> options;

        public EditModel(
            IOptions<ConfigSettings> options,
            UserHandler userManager,
            RoleManager<WebAppRole> roleManager,
            DataConnector context
            )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
            this.options = options;
        }
        

        public string UserName { get; set; }
        //public string Role { get; set; }
        public IList<MtdGroup> MtdGroups { get; set; }
        public InputModel Input { get; set; }
        public List<string> GroupIds { get; set; }

        public List<GroupModel> SelectedGroups { get; set; }
        public string SelectedGroupIds { get; set; }
        public string SelectedChildIds { get; set; }

        public List<MTDSelectListItem> GroupList { get; set; }

        public class InputModel
        {
            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "Full name")]
            public string Title { get; set; }

            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }

            [Display(Name = "Confirm")]
            public bool IsConfirm { get; set; }
         
            public string Role { get; set; }
            public string Policy { get; set; }
            public string TitleGroup { get; set; }              
        }

        public List<MTDSelectListItem> Roles { get; set; }
        public List<MTDSelectListItem> Policies { get; set; }
        public List<MTDSelectListItem> Users { get; set; }
        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var currentUser = await _userManager.GetUserAsync(User);
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) { return NotFound(); }
            UserName = user.UserName;
            IList<WebAppRole> roles = await _roleManager.Roles.ToListAsync();
            var userRoles = await _userManager.GetRolesAsync(user);
            var userRoleName = userRoles.FirstOrDefault();
            var userRole =  await _roleManager.FindByNameAsync(userRoleName);         

            Input = new InputModel
            {
                Role = userRole.Id,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Title = user.Title,
                IsConfirm = user.EmailConfirmed,
                TitleGroup = user.TitleGroup
            };

            Roles = new List<MTDSelectListItem>();
            roles.OrderBy(x => x.Seq).ToList().ForEach((item) =>
            {
                Roles.Add(new MTDSelectListItem { Id = item.Id, Value = item.Title });
            });
                      

            Users = new List<MTDSelectListItem>();
            var users = await _userManager.Users
                .Where(x=>x.DatabaseId == currentUser.DatabaseId)
                .OrderBy(x => x.Title)
                .ToListAsync();

            users.ForEach((item) =>
            {
                Users.Add(new MTDSelectListItem { Id = item.Id, Value = item.GetFullName() });
            });
            //ViewData["Users"] = new SelectList(_userManager.Users.OrderBy(x => x.Title), "Id", "Title", Input.Role);

            string policyID = await _userManager.GetPolicyIdAsync(user);
            Input.Policy = policyID;
            IList<MtdPolicy> mtdPolicy = await _userManager.GetPoliciesAsync();

            Policies = new List<MTDSelectListItem>();
            mtdPolicy.OrderBy(x => x.Name).ToList().ForEach((item) =>
            {
                Policies.Add(new MTDSelectListItem { Id = item.Id, Value = item.Name });
            });

           //ViewData["Policies"] = new SelectList(mtdPolicy.OrderBy(x=>x.Name), "Id", "Name", policyID);

            MtdGroups = await _context.MtdGroup.OrderBy(x => x.Name).ToListAsync();

            IList<Claim> claims = await _userManager.GetClaimsAsync(user);
            GroupIds = new List<string>();
            GroupIds = claims.Where(x => x.Type == "group").Select(x => x.Value).ToList();

            IList<MtdGroup> groups = await _context.MtdGroup.OrderBy(x => x.Name).ToListAsync();
            GroupList = new List<MTDSelectListItem>()
            {
                new MTDSelectListItem{ Id="firstitem", Value="No group selected", Selectded=true, Localized=true}
            };

            foreach (var group in groups)
            {                
                GroupList.Add(new MTDSelectListItem { Id = group.Id, Value = group.Name });
            }

            IList<MtdGroup> selectedGroups = groups.Where(x => GroupIds.Contains(x.Id)).ToList();

            SelectedGroups = new List<GroupModel>();
            foreach (var group in selectedGroups)
            {
                GroupModel groupModel = new() { Id = group.Id, Name = group.Name, UserName = "No owner selected" };

                SelectedGroupIds += $"&{group.Id}";
                SelectedGroups.Add(groupModel);
            }


            return Page();
        }

    }
}
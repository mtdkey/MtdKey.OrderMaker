/*
    OrderMaker - http://mtdkey.com
    Copyright(c) 2019 Oleg Bruev. All rights reserved.
*/


using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MtdKey.OrderMaker.Areas.Identity.Data;
using MtdKey.OrderMaker.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MtdKey.OrderMaker.Services
{
    public enum RightsType
    {
        ViewAll, Create, Edit, Delete, ViewOwn, EditOwn, DeleteOwn,
        ViewGroup, EditGroup, DeleteGroup, SetOwn,
        Reviewer, SetDate, OwnDenyGroup,
        ExportToExcel
    };

    public partial class UserHandler : UserManager<WebAppUser>
    {
        private readonly DataConnector _context;
        private readonly SignInManager<WebAppUser> _signInManager;
        private readonly IdentityDbContext identity;


        public UserHandler(IdentityDbContext identity,
            DataConnector context,
            IUserStore<WebAppUser> store,
            IOptions<IdentityOptions> optionsAccessor,
            IPasswordHasher<WebAppUser> passwordHasher,
            IEnumerable<IUserValidator<WebAppUser>> userValidators,
            IEnumerable<IPasswordValidator<WebAppUser>> passwordValidators,
            ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors,
            IServiceProvider services, ILogger<UserManager<WebAppUser>> logger, SignInManager<WebAppUser> signInManager) :
            base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {
            _context = context;
            _signInManager = signInManager;
            this.identity = identity;
        }

        public async Task<IList<MtdPolicy>> GetPoliciesAsync()
        {

            IList<MtdPolicy> mtdPolicies = await _context.MtdPolicy.ToListAsync();

            foreach (var policy in mtdPolicies)
            {
                await _context.Entry(policy).Collection(x => x.MtdPolicyForms).LoadAsync();
                await _context.Entry(policy).Collection(x => x.MtdPolicyParts).LoadAsync();
                await _context.Entry(policy).Collection(x => x.MtdPolicyScripts).LoadAsync();
            }

            return mtdPolicies;
        }

        public async Task<string> GetPolicyIdAsync(WebAppUser user)
        {
            IList<Claim> claims = await GetClaimsAsync(user);
            string policyId = claims.Where(x => x.Type == "policy").Select(x => x.Value).FirstOrDefault();
            return policyId ?? "";
        }

        public async Task<List<string>> GetViewFormIdsAsync(WebAppUser user)
        {
            List<string> formIds = new();
            if (user == null) return formIds;

            string policyId = await GetPolicyIdAsync(user);
            IList<MtdPolicy> mtdPolicy = await GetPoliciesAsync();

            IList<MtdPolicyForms> policyForms = mtdPolicy.SelectMany(x => x.MtdPolicyForms)
                .Where(x => x.MtdPolicy == policyId && (x.ViewAll == 1 || x.ViewGroup == 1 || x.ViewOwn == 1))
                .ToList();

            formIds = policyForms.GroupBy(x => x.MtdForm).Select(x => x.Key).ToList();
            return formIds;
        }

        public async Task SetPolicyForUserAsync(WebAppUser user, string policyId)
        {
            IEnumerable<Claim> claims = await GetClaimsAsync(user);
            await RemoveClaimsAsync(user, claims.Where(x => x.Type == "policy"));

            Claim claim = new("policy", policyId);
            await AddClaimAsync(user, claim);
        }

        public async Task<MtdPolicy> GetPolicyForUserAsync(WebAppUser user)
        {
            string policyId = await GetPolicyIdAsync(user);
            if (policyId == null) return null;
            IList<MtdPolicy> mtdPolicy = await GetPoliciesAsync();
            if (mtdPolicy == null) return null;
            return mtdPolicy.Where(x => x.Id == policyId).FirstOrDefault();
        }

        public async Task<WebAppUser> GetOwnerAsync(string storeId)
        {
            WebAppUser webAppUser = null;
            MtdStoreOwner owner = await _context.MtdStoreOwner.Where(x => x.Id == storeId).FirstOrDefaultAsync();
            if (owner != null)
            {
                webAppUser = await FindByIdAsync(owner.UserId);
            }

            return webAppUser;
        }

        public async Task<bool> IsOwner(WebAppUser user, string storeId)
        {
            return await _context.MtdStoreOwner.Where(x => x.Id == storeId && x.UserId == user.Id).AnyAsync();
        }

        public async Task<List<MtdGroup>> GetAllGroupsAsync()
        {
            return await _context.MtdGroup.ToListAsync();
        }

        public async Task<bool> InGroup(WebAppUser user, string formId, string storeId)
        {
            string ownerId = await _context.MtdStoreOwner.Where(x => x.Id == storeId).Select(x => x.UserId).FirstOrDefaultAsync();
            WebAppUser userOwner = new() { Id = ownerId };
            if (user.Id == userOwner.Id) { return true; }

            bool denyGroup = await CheckUserPolicyAsync(userOwner, formId, RightsType.OwnDenyGroup);
            if (denyGroup) { return false; }

            IList<Claim> ownerClaims = await GetClaimsAsync(userOwner);
            List<string> ownerGroupdIds = ownerClaims.Where(x => x.Type == "group").Select(x => x.Value).ToList();
            IList<Claim> userClaims = await GetClaimsAsync(user);
            List<string> userGroupdIds = userClaims.Where(x => x.Type == "group").Select(x => x.Value).ToList();

            bool result = false;

            foreach (var idgroup in ownerGroupdIds)
            {
                if (userGroupdIds.Contains(idgroup))
                {
                    result = true;
                }
            }

            return result;
        }

        public async Task<bool> IsCreator(WebAppUser user, string formId)
        {
            IList<MtdPolicy> mtdPolicy = await GetPoliciesAsync();
            string policyId = await GetPolicyIdAsync(user);
            if (policyId == null) return false;
            MtdPolicyForms policyForms = mtdPolicy.SelectMany(x => x.MtdPolicyForms).Where(x => x.MtdForm == formId && x.MtdPolicy == policyId).FirstOrDefault();
            if (policyForms == null) return false;

            return policyForms.Create == 1;
        }

        public async Task<bool> IsViewer(WebAppUser user, string formId, string storeId)
        {
            if (storeId == null || formId == null) { return false; }
            if (storeId.Length > 36 || formId.Length > 36) { return false; }

            IList<MtdPolicy> mtdPolicy = await GetPoliciesAsync();
            string policyId = await GetPolicyIdAsync(user);
            if (policyId == null) { return false; }
            MtdPolicyForms policyForms = mtdPolicy.SelectMany(x => x.MtdPolicyForms).Where(x => x.MtdForm == formId && x.MtdPolicy == policyId).FirstOrDefault();
            if (policyForms == null) { return false; }
            if (policyForms.ViewAll == 1) { return true; }

            bool isOwner = await IsOwner(user, storeId);
            if (policyForms.ViewOwn == 1 && isOwner) { return true; }

            bool inGroup = await InGroup(user, formId, storeId);
            if (policyForms.ViewGroup == 1 && inGroup) { return true; }

            return false;

        }

        public async Task<bool> IsViewer(WebAppUser user, string formId)
        {
            if (formId.Length > 36) { return false; }
            IList<MtdPolicy> mtdPolicy = await GetPoliciesAsync();
            string policyId = await GetPolicyIdAsync(user);
            if (policyId == null) { return false; }
            MtdPolicyForms policyForms = mtdPolicy.SelectMany(x => x.MtdPolicyForms).Where(x => x.MtdForm == formId && x.MtdPolicy == policyId).FirstOrDefault();
            if (policyForms == null) { return false; }
            if (policyForms.ViewAll == 1 || policyForms.ViewGroup == 1 || policyForms.ViewOwn == 1) { return true; }

            return false;
        }

        public async Task<bool> IsEditor(WebAppUser user, string formId, string storeId)
        {
            IList<MtdPolicy> mtdPolicy = await GetPoliciesAsync();
            string policyId = await GetPolicyIdAsync(user);
            if (policyId == null) { return false; }
            MtdPolicyForms policyForms = mtdPolicy.SelectMany(x => x.MtdPolicyForms)
                .Where(x => x.MtdForm == formId && x.MtdPolicy == policyId).FirstOrDefault();
            if (policyForms == null) { return false; }

            if (policyForms.EditAll == 1) { return true; }

            if (storeId != null)
            {
                bool isOwner = await IsOwner(user, storeId);
                if (policyForms.EditOwn == 1 && isOwner) { return true; }
                bool inGroup = await InGroup(user, formId, storeId);
                if (policyForms.EditGroup == 1 && inGroup) { return true; }
            }

            return false;
        }

        public async Task<bool> IsEraser(WebAppUser user, string formId, string storeId)
        {
            IList<MtdPolicy> mtdPolicy = await GetPoliciesAsync();
            string policyId = await GetPolicyIdAsync(user);
            if (policyId == null) return false;
            MtdPolicyForms policyForms = mtdPolicy.SelectMany(x => x.MtdPolicyForms).Where(x => x.MtdForm == formId && x.MtdPolicy == policyId).FirstOrDefault();
            if (policyForms == null) return false;

            if (policyForms.DeleteAll == 1) { return true; }

            if (storeId != null)
            {
                bool isOwner = await IsOwner(user, storeId);
                if (policyForms.DeleteOwn == 1 && isOwner) { return true; }
                bool inGroup = await InGroup(user, formId, storeId);
                if (policyForms.DeleteGroup == 1 && inGroup) { return true; }
            }

            return false;
        }

        public async Task<bool> IsInstallerOwner(WebAppUser user, string formId)
        {
            IList<MtdPolicy> mtdPolicy = await GetPoliciesAsync();
            string policyId = await GetPolicyIdAsync(user);
            if (policyId == null) return false;
            MtdPolicyForms policyForms = mtdPolicy.SelectMany(x => x.MtdPolicyForms).Where(x => x.MtdForm == formId && x.MtdPolicy == policyId).FirstOrDefault();
            if (policyForms == null) return false;

            return policyForms.ChangeOwner == 1;
        }

        public async Task<bool> IsReviewerAsync(WebAppUser user, string formId)
        {
            IList<MtdPolicy> mtdPolicy = await GetPoliciesAsync();
            string policyId = await GetPolicyIdAsync(user);
            if (policyId == null) return false;
            MtdPolicyForms policyForms = mtdPolicy.SelectMany(x => x.MtdPolicyForms)
                .Where(x => x.MtdForm == formId && x.MtdPolicy == policyId).FirstOrDefault();
            if (policyForms == null) return false;

            return policyForms.Reviewer == 1;
        }

        public async Task<bool> IsCreatorPartAsync(WebAppUser user, string partId)
        {
            IList<MtdPolicy> mtdPolicy = await GetPoliciesAsync();
            string policyId = await GetPolicyIdAsync(user);
            if (policyId == null) return false;
            MtdPolicyParts policyParts = mtdPolicy.SelectMany(x => x.MtdPolicyParts).Where(x => x.MtdFormPart == partId && x.MtdPolicy == policyId).FirstOrDefault();
            if (policyParts == null) return false;

            return policyParts.Create == 1;
        }

        public async Task<bool> IsEditorPartAsync(WebAppUser user, string idPart)
        {
            IList<MtdPolicy> mtdPolicy = await GetPoliciesAsync();
            string policyId = await GetPolicyIdAsync(user);
            if (policyId == null) return false;
            MtdPolicyParts policyParts = mtdPolicy.SelectMany(x => x.MtdPolicyParts).Where(x => x.MtdFormPart == idPart && x.MtdPolicy == policyId).FirstOrDefault();
            if (policyParts == null) return false;

            return policyParts.Edit == 1;
        }

        public async Task<bool> IsViewerPartAsync(WebAppUser user, string idPart)
        {
            IList<MtdPolicy> mtdPolicy = await GetPoliciesAsync();
            string policyId = await GetPolicyIdAsync(user);
            if (policyId == null) return false;
            MtdPolicyParts policyParts = mtdPolicy.SelectMany(x => x.MtdPolicyParts).Where(x => x.MtdFormPart == idPart && x.MtdPolicy == policyId).FirstOrDefault();
            if (policyParts == null) return false;

            return policyParts.View == 1;
        }

        public async Task<List<MtdFormPart>> GetAllowPartsForView(WebAppUser user, string formId)
        {
            List<MtdFormPart> result = new();
            IList<MtdPolicy> mtdPolicy = await GetPoliciesAsync();
            MtdPolicy userPolicy = await GetPolicyForUserAsync(user);
            if (userPolicy == null) return result;

            IList<MtdFormPart> parts = await _context.MtdFormPart
                .Include(x => x.MtdFormPartHeader)
                .Where(x => x.MtdFormId == formId)
                .AsSplitQuery()
                .ToListAsync();
            List<string> partIds = parts.Select(x => x.Id).ToList();
            List<string> allowPartsIds = userPolicy.MtdPolicyParts
                .Where(x => partIds.Contains(x.MtdFormPart) && x.View == 1)
                .Select(x => x.MtdFormPart)
                .ToList();

            return parts.Where(x => allowPartsIds.Contains(x.Id)).OrderBy(x => x.Sequence).ToList();
        }

        public async Task<List<WebAppUser>> GetUsersInGroupsOutDenyAsync(WebAppUser webAppUser, string formId)
        {
            List<WebAppUser> result = new();
            List<WebAppUser> users = await GetUsersInGroupsAsync(webAppUser);

            foreach (WebAppUser user in users)
            {
                bool checkDeny = await CheckUserPolicyAsync(user, formId, RightsType.OwnDenyGroup);
                if (!checkDeny || webAppUser.Id == user.Id)
                {
                    result.Add(user);
                }
            }

            return result;
        }

        public async Task<IList<WebAppUser>> GetUsersInGroupAsync(string groupId = null)
        {
            if (groupId != null)
            {
                Claim claim = new("group", groupId);
                return await GetUsersForClaimAsync(claim);
            }

            IList<string> userIds = await identity.UserClaims.Where(x => x.ClaimType == "group").Select(x => x.UserId).ToListAsync();
            return await Users.Where(x => !userIds.Contains(x.Id)).ToListAsync();

        }

        public async Task<List<WebAppUser>> GetUsersInGroupsAsync(WebAppUser webAppUser)
        {

            List<WebAppUser> webAppUsers = new();
            IList<Claim> claims = await GetClaimsAsync(webAppUser);
            IList<Claim> groups = claims.Where(c => c.Type == "group").ToList();

            foreach (var claim in groups)
            {
                IList<WebAppUser> users = await GetUsersForClaimAsync(claim);
                if (users != null)
                {
                    var temp = users.Where(x => !webAppUsers.Select(w => w.Id).Contains(x.Id)).ToList();
                    if (temp != null)
                    {
                        webAppUsers.AddRange(temp);
                    }
                }
            }

            return webAppUsers;
        }

        public async Task<List<WebAppUser>> GetUsersForViewingForm(string formId, string storeId = null)
        {
            List<WebAppUser> usersAll = await this.Users.ToListAsync();
            List<WebAppUser> usersAccess = new();

            foreach (WebAppUser user in usersAll)
            {
                bool viewer = await IsViewer(user, formId, storeId);
                if (viewer)
                {
                    usersAccess.Add(user);
                }
            }

            return usersAccess;
        }

        public async Task<MtdFilter> GetFilterAsync(WebAppUser user, string formId)
        {
            MtdFilter filter = await _context.MtdFilter.AsNoTracking().FirstOrDefaultAsync(x => x.IdUser == user.Id && x.MtdFormId == formId);

            if (filter == null)
            {
                filter = new MtdFilter
                {
                    IdUser = user.Id,
                    MtdFormId = formId,
                    SearchNumber = "",
                    SearchText = "",
                    Page = 1,
                    PageSize = 10,
                    ShowDate = 1,
                    ShowNumber = 1
                };
                await _context.MtdFilter.AddAsync(filter);
                await _context.SaveChangesAsync();
            }

            return filter;
        }

        public async Task<MtdFilter> GetFilterAsync(ClaimsPrincipal principal, string formId)
        {
            WebAppUser user = await GetUserAsync(principal);
            return await GetFilterAsync(user, formId);
        }

        public async Task<bool> IsFilterAccessingAsync(ClaimsPrincipal user, int scriptId)
        {
            WebAppUser userApp = await GetUserAsync(user);
            string policyId = await GetPolicyIdAsync(userApp);
            IList<MtdPolicy> mtdPolicy = await GetPoliciesAsync();
            MtdPolicy policy = mtdPolicy.Where(x => x.Id == policyId).FirstOrDefault();
            return policy.MtdPolicyScripts.Where(x => x.MtdFilterScriptId == scriptId).Any();
        }

        public async Task<List<MtdFilterScript>> GetFilterScriptsAsync(WebAppUser user, string formId, sbyte apply = -1)
        {
            string policyId = await GetPolicyIdAsync(user);
            IList<MtdPolicy> mtdPolicy = await GetPoliciesAsync();

            MtdPolicy policy = mtdPolicy.Where(x => x.Id == policyId).FirstOrDefault();

            List<int> filterIds = policy.MtdPolicyScripts.Select(x => x.MtdFilterScriptId).ToList();

            var query = _context.MtdFilterScript.AsNoTracking().Where(x => filterIds.Contains(x.Id) && x.MtdFormId == formId);

            if (apply > 0)
            {
                var filter = await GetFilterAsync(user, formId);
                IList<int> applyIds = await _context.MtdFilterScriptApply
                    .Where(x => x.MtdFilterId == filter.Id)
                    .Select(x => x.MtdFilterScriptId).ToListAsync();
                query = query.Where(q => applyIds.Contains(q.Id));
            }

            return await query.ToListAsync();
        }

        public async Task<bool> CheckUserPolicyAsync(WebAppUser user, string formId, RightsType rightsType)
        {
            IList<MtdPolicy> mtdPolicy = await GetPoliciesAsync();
            string policyId = await GetPolicyIdAsync(user);
            if (policyId == null) return false;
            MtdPolicyForms policyForms = mtdPolicy.SelectMany(x => x.MtdPolicyForms)
                .Where(x => x.MtdForm == formId && x.MtdPolicy == policyId).FirstOrDefault();
            if (policyForms == null) return false;

            bool result = false;

            switch (rightsType)
            {
                case RightsType.Create:
                    {
                        result = policyForms.Create == 1;
                        break;
                    }

                case RightsType.Delete:
                    {
                        result = policyForms.DeleteAll == 1;
                        break;
                    }

                case RightsType.DeleteGroup:
                    {
                        result = policyForms.DeleteGroup == 1;
                        break;
                    }
                case RightsType.DeleteOwn:
                    {
                        result = policyForms.DeleteOwn == 1;
                        break;
                    }
                case RightsType.Edit:
                    {
                        result = policyForms.EditAll == 1;
                        break;
                    }
                case RightsType.EditGroup:
                    {
                        result = policyForms.EditGroup == 1;
                        break;
                    }
                case RightsType.EditOwn:
                    {
                        result = policyForms.EditOwn == 1;
                        break;
                    }
                case RightsType.Reviewer:
                    {
                        result = policyForms.Reviewer == 1;
                        break;
                    }

                case RightsType.SetOwn:
                    {
                        result = policyForms.ChangeOwner == 1;
                        break;
                    }
                case RightsType.ViewAll:
                    {
                        result = policyForms.ViewAll == 1;
                        break;
                    }
                case RightsType.ViewGroup:
                    {
                        result = policyForms.ViewGroup == 1;
                        break;
                    }
                case RightsType.ViewOwn:
                    {
                        result = policyForms.ViewOwn == 1;
                        break;
                    }
                case RightsType.SetDate:
                    {
                        result = policyForms.ChangeDate == 1;
                        break;
                    }
                case RightsType.OwnDenyGroup:
                    {
                        result = policyForms.OwnDenyGroup == 1;
                        break;
                    }
                case RightsType.ExportToExcel:
                    {
                        result = policyForms.ExportToExcel == 1;
                        break;
                    }
                default:
                    {
                        result = false;
                        break;
                    }

            }

            return result;
        }

        public override IQueryable<WebAppUser> Users
        {
            get
            {
                if (Store is not IQueryableUserStore<WebAppUser> queryableStore)
                {
                    throw new NotSupportedException("StoreNotIQueryableUserStore");
                }
                return queryableStore.Users.Where(x => x.DatabaseId == _context.DatabaseId);
            }
        }

        public override async Task<WebAppUser> GetUserAsync(ClaimsPrincipal principal)
        {
            if (principal == null)
            {
                throw new ArgumentNullException(nameof(principal));
            }
            var id = GetUserId(principal);
            if (id == null) { return null; }
            WebAppUser user = await FindByIdAsync(id);
            if (user == null)
            {
                await _signInManager.SignOutAsync();
                return null;
            }

            return user;
        }

        public string GeneratePassword()
        {
            int length = 12;
            bool nonAlphanumeric = true;
            bool digit = true;
            bool lowercase = true;
            bool uppercase = true;

            StringBuilder password = new();
            Random random = new();
            int[] exceptions = new int[] { 92, 47, 40, 41, 39, 34, 44, 46, 60, 62, 96, 32, 123, 124, 125, 38, 43 };

            while (password.Length < length)
            {
            getCode:
                int code = random.Next(33, 126);
                if (exceptions.Where(x => exceptions.Contains(code)).Any()) { goto getCode; }
                char c = (char)code;

                password.Append(c);

                if (char.IsDigit(c))
                    digit = false;
                else if (char.IsLower(c))
                    lowercase = false;
                else if (char.IsUpper(c))
                    uppercase = false;
                else if (!char.IsLetterOrDigit(c))
                    nonAlphanumeric = false;
            }

            if (nonAlphanumeric)
                password.Append((char)random.Next(33, 46));
            if (digit)
                password.Append((char)random.Next(48, 58));
            if (lowercase)
                password.Append((char)random.Next(97, 123));
            if (uppercase)
                password.Append((char)random.Next(65, 91));


            return password.ToString();
        }



    }
}

/*
    MTD OrderMaker - http://mtdkey.com
    Copyright (c) 2019 Oleg Bruev <job4bruev@gmail.com>. All rights reserved.
*/

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MtdKey.OrderMaker.Areas.Identity.Data;
using MtdKey.OrderMaker.Core;
using MtdKey.OrderMaker.Entity;
using MtdKey.OrderMaker.Models.Controls.MTDSelectList;
using MtdKey.OrderMaker.Models.Index;
using MtdKey.OrderMaker.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MtdKey.OrderMaker.Components.Index.Filter
{
    public enum ServiceFilter { DateCreated, DocumentOwner, DocumentBased }

    [ViewComponent(Name = "IndexFilterSelector")]
    public class Selector : ViewComponent
    {
        private readonly DataConnector _context;
        private readonly UserHandler _userHandler;

        public Selector(DataConnector context, UserHandler userHandler)
        {
            _context = context;
            _userHandler = userHandler;
        }

        public async Task<IViewComponentResult> InvokeAsync(string formId)
        {
            WebAppUser user = await _userHandler.GetUserAsync(HttpContext.User);

            MtdFilter filter = await _context.MtdFilter.FirstOrDefaultAsync(x => x.IdUser == user.Id && x.MtdFormId == formId);

            List<MTDSelectListItem> customItems = await GetCustomFieldsAsync(user, formId, filter);

            IList<MtdSysTerm> mtdSysTerms = await _context.MtdSysTerm.ToListAsync();
            List<MTDSelectListItem> terms = new();
            mtdSysTerms.ToList().ForEach((term =>
            {
                terms.Add(new MTDSelectListItem { Id = term.Id.ToString(), Value = $"{term.Name} ({term.Sign})", Localized = true });
            }));

            List<MTDSelectListItem> userList = await GetUserItemsAsync(user, formId);

            IList<MtdFilterScript> scripts = await _userHandler.GetFilterScriptsAsync(user, formId, 0);
            List<MTDSelectListItem> scriptItems = new();

            foreach (var script in scripts)
            {
                scriptItems.Add(new MTDSelectListItem { Id = script.Id.ToString(), Value = script.Name });
            }

            List<MTDSelectListItem> serviceItems = GetServiceItems();

            SelectorModelView selector = new()
            {
                FormId = formId,
                ScriptItems = scriptItems,
                UsersItems = userList,
                CustomItems = customItems,
                TermItems = terms,
                ServiceItems = serviceItems,
            };

            return View("Default", selector);
        }


        private async Task<List<MTDSelectListItem>> GetCustomFieldsAsync(WebAppUser user, string formId, MtdFilter mtdFilter)
        {
            List<MtdFormPart> parts = await _userHandler.GetAllowPartsForView(user, formId);
            List<string> partIds = parts.Select(x => x.Id).ToList();

            var query = _context.MtdFormPartField
                .Where(x => x.Active == 1 && partIds.Contains(x.MtdFormPartId))
                .OrderBy(x => x.MtdFormPartNavigation.Sequence).ThenBy(x => x.Sequence);

            IList<MtdFormPartField> mtdFields;
            if (mtdFilter != null)
            {
                List<string> fieldIds = await _context.MtdFilterField.Where(x => x.MtdFilter == mtdFilter.Id)
                    .Select(x => x.MtdFormPartFieldId).ToListAsync();
                mtdFields = await query.Where(x => !fieldIds.Contains(x.Id)).ToListAsync();
            }
            else
            {
                mtdFields = await query.ToListAsync();
            }

            List<MTDSelectListItem> customItems = new();
            int[] exclude = { 7, 8, 13 };
            List<MtdFormPartField> customFields = mtdFields.Where(x => !exclude.Contains(x.MtdSysType)).ToList();

            foreach (var item in customFields)
            {
                customItems.Add(new MTDSelectListItem
                {
                    Id = item.Id,
                    Value = $"{item.MtdFormPartNavigation.Name}: {item.Name}",
                    Attributes = $" data-type={@item.MtdSysType} "
                });
            }

            return customItems;
        }

        private async Task<List<MTDSelectListItem>> GetUserItemsAsync(WebAppUser user, string formId)
        {
            List<MTDSelectListItem> userList = new();

            List<WebAppUser> appUsers = new();

            bool isViewAll = await _userHandler.CheckUserPolicyAsync(user, formId, RightsType.ViewAll);
            if (isViewAll)
            {
                appUsers = await _userHandler.Users.ToListAsync();
            }
            else
            {
                appUsers = await _userHandler.GetUsersInGroupsAsync(user);
            }

            appUsers = appUsers.OrderBy(x => x.Title).ToList();

            foreach (var appUser in appUsers)
            {
                userList.Add(new MTDSelectListItem { Id = appUser.Id, Value = appUser.GetFullName() });
            }

            return userList;

        }

        private static List<MTDSelectListItem> GetServiceItems()
        {

            List<MTDSelectListItem> result = new()
            {
                        new MTDSelectListItem {
                            Id=ServiceFilter.DateCreated.ToString(),
                            Value="Date Created",
                            Localized = true
                        },
                        new MTDSelectListItem {
                            Id=ServiceFilter.DocumentOwner.ToString(),
                            Value="Document owner",
                            Localized = true
                        }

                    };

            return result;
        }
    }
}

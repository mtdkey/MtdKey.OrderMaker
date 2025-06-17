/*
    MTD OrderMaker - http://mtdkey.com
    Copyright (c) 2019 Oleg Bruev <job4bruev@gmail.com>. All rights reserved.
*/

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.Extensions.Localization;
using MtdKey.OrderMaker.Areas.Identity.Data;
using MtdKey.OrderMaker.Entity;
using MtdKey.OrderMaker.Models.Index;
using MtdKey.OrderMaker.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MtdKey.OrderMaker.Components.Index.Filter
{
    [ViewComponent(Name = "IndexFilterDisplay")]
    public class Display : ViewComponent
    {
        private readonly OrderMakerContext _context;
        private readonly UserHandler _userHandler;
        private readonly IStringLocalizer<SharedResource> localizer;

        public Display(OrderMakerContext context, UserHandler userHandler, IStringLocalizer<SharedResource> localizer)
        {
            _context = context;
            _userHandler = userHandler;
            this.localizer = localizer;
        }

        public async Task<IViewComponentResult> InvokeAsync(string formId)
        {
            List<DisplayData> displayDatas = new();
            var user = await _userHandler.GetUserAsync(HttpContext.User);

            List<MtdFormPart> parts = await _userHandler.GetAllowPartsForView(user, formId);
            List<string> partIds = parts.Select(x => x.Id).ToList();

            MtdFilter filter = await _context.MtdFilter.FirstOrDefaultAsync(x => x.IdUser == user.Id && x.MtdFormId == formId);
            if (filter != null)
            {

                List<MtdFilterField> mtdFilterFields = await _context.MtdFilterField
                    .Where(x => x.MtdFilter == filter.Id)
                    .ToListAsync();

                foreach (var field in mtdFilterFields)
                {
                    await _context.Entry(field).Reference(x => x.MtdTermNavigation).LoadAsync();
                    await _context.Entry(field).Reference(x => x.MtdFormPartFieldNavigation).LoadAsync();

                    DisplayData displayData = new()
                    {
                        Id = field.Id,
                        Header = $"{field.MtdFormPartFieldNavigation.Name} ({field.MtdTermNavigation.Sign})",
                        Value = "",
                        Type = "-field"
                    };



                    displayData.Value = field.Value;
                    if (field.MtdFormPartFieldNavigation.MtdSysType == 12)
                    {
                        displayData.Value = field.Value.Equals("1") ? localizer["ON"] : localizer["OFF"];
                    }

                    if (field.MtdFormPartFieldNavigation.MtdSysType == 5 || field.MtdFormPartFieldNavigation.MtdSysType == 6)
                    {
                        displayData.Header = field.MtdFormPartFieldNavigation.Name;
                        displayData.Value = field.Value.Replace("***", "-");
                    }

                    displayDatas.Add(displayData);
                }

                MtdFilterDate mtdFilterDate = await _context.MtdFilterDate.FindAsync(filter.Id);
                if (mtdFilterDate != null)
                {
                    DisplayData displayDate = new()
                    {
                        Id = filter.Id,
                        Header = localizer["Period"],
                        Value = $"{mtdFilterDate.DateStart.ToShortDateString()} {mtdFilterDate.DateEnd.ToShortDateString()}",
                        Type = "-date"
                    };
                    displayDatas.Add(displayDate);
                }

                MtdFilterOwner mtdFilterOwner = await _context.MtdFilterOwner.FindAsync(filter.Id);
                if (mtdFilterOwner != null)
                {
                    WebAppUser userOwner = await _userHandler.FindByIdAsync(mtdFilterOwner.OwnerId);
                    DisplayData displayDate = new()
                    {
                        Id = filter.Id,
                        Header = localizer["Owner"],
                        Value = $"{userOwner.Title}",
                        Type = "-owner"
                    };
                    displayDatas.Add(displayDate);
                }

                IList<MtdFilterScript> scripts = await _userHandler.GetFilterScriptsAsync(user, formId, 1);
                if (scripts != null && scripts.Count > 0)
                {
                    foreach (var fs in scripts)
                    {
                        DisplayData displayDate = new()
                        {
                            Id = fs.Id,
                            Header = localizer["Advanced filter"],
                            Value = fs.Name,
                            Type = "-script"
                        };
                        displayDatas.Add(displayDate);
                    }

                }
            }

            DisplayModelView displayModelView = new()
            {
                FormId = formId,
                IdFilter = filter == null ? -1 : filter.Id,
                DisplayDatas = displayDatas
            };

            return View("Default", displayModelView);
        }
    }
}

/*
    MTD OrderMaker - http://mtdkey.com
    Copyright (c) 2019 Oleg Bruev <job4bruev@gmail.com>. All rights reserved.
*/

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MtdKey.OrderMaker.Areas.Identity.Data;
using MtdKey.OrderMaker.Entity;
using MtdKey.OrderMaker.Models.Index;
using System.Threading.Tasks;

namespace MtdKey.OrderMaker.Components.Index
{
    [ViewComponent(Name = "IndexFooter")]
    public class Footer : ViewComponent
    {
        private readonly OrderMakerContext _context;
        private readonly UserManager<WebAppUser> _userManager;

        public Footer(OrderMakerContext context, UserManager<WebAppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        public async Task<IViewComponentResult> InvokeAsync(string formId, int pageCount)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            int pageSize = 10;
            int page = 1;
            MtdFilter filter = await _context.MtdFilter.FirstOrDefaultAsync(x => x.IdUser == user.Id && x.MtdFormId == formId);
            if (filter != null) { page = filter.Page; pageSize = filter.PageSize; }

            FooterModelView model = new() { FormId = formId, Page = page, PageSize = pageSize, PageCount = pageCount };

            return View("Default", model);
        }
    }
}

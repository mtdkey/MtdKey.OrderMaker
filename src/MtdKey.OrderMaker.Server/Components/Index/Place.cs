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
using System.Linq;
using System.Threading.Tasks;

namespace MtdKey.OrderMaker.Components.Index
{
    [ViewComponent(Name = "IndexPlace")]
    public class Place : ViewComponent
    {
        private readonly DataConnector _context;
        private readonly UserManager<WebAppUser> _userManager;

        public Place(DataConnector context, UserManager<WebAppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync(string formId)
        {

            var user = await _userManager.GetUserAsync(HttpContext.User);
            bool isExists = await _context.MtdFilter.Where(x => x.IdUser == user.Id).AnyAsync();

            if (!isExists)
            {
                MtdFilter mtdFilter = new()
                {
                    IdUser = user.Id,
                    MtdFormId = formId,
                    Page = 1,
                    PageSize = 10,
                    SearchNumber = "",
                    SearchText = ""
                };
                await _context.MtdFilter.AddAsync(mtdFilter);
                await _context.SaveChangesAsync();
            }

            PlaceModelView model = new() { FormId = formId };
            return View("Default", model);
        }
    }
}

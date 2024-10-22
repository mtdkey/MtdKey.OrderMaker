/*
    MTD OrderMaker - http://mtdkey.com
    Copyright (c) 2019 Oleg Bruev <job4bruev@gmail.com>. All rights reserved.
*/

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MtdKey.OrderMaker.Areas.Identity.Data;
using MtdKey.OrderMaker.Entity;
using MtdKey.OrderMaker.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MtdKey.OrderMaker.Pages
{
    public class IndexModel : PageModel
    {
        private readonly DataConnector _context;
        private readonly UserHandler _userHandler;

        public IndexModel(UserHandler userHandler, DataConnector context)
        {
            _context = context;
            _userHandler = userHandler;
        }

        public IList<MtdForm> Forms { get; set; }
        public string SearchText { get; set; }

        public async Task<IActionResult> OnGetAsync(string searchText)
        {
            WebAppUser user = await _userHandler.GetUserAsync(HttpContext.User);

            List<string> formIds = await _userHandler.GetViewFormIdsAsync(user);
            List<MtdForm> forms = await _context.MtdForm.Where(x => formIds.Contains(x.Id)).ToListAsync();

            foreach (var form in forms)
            {
                bool isExists = await _context.MtdFilter.Where(x => x.MtdFormId == form.Id && x.IdUser == user.Id).AnyAsync();
                if (!isExists)
                {
                    MtdFilter mtdFilter = new()
                    {
                        MtdFormId = form.Id,
                        IdUser = user.Id,
                        Page = 1,
                        PageSize = 10,
                        SearchText = "",
                        ShowDate = 1,
                        SearchNumber = "",
                        ShowNumber = 1
                    };

                    await _context.MtdFilter.AddAsync(mtdFilter);
                    await _context.SaveChangesAsync();
                }
            }

            IQueryable<MtdForm> query = _context.MtdForm
                    .Where(x => formIds.Contains(x.Id));

            if (searchText != null)
            {
                query = query.Where(x => x.Name.Contains(searchText));
                SearchText = searchText;
            }


            Forms = await query.ToListAsync();

            foreach (var form in Forms)
            {
                await _context.Entry(form).Reference(x => x.MtdCategoryNavigation).LoadAsync();
                await _context.Entry(form).Reference(x => x.MtdFormHeader).LoadAsync();
                await _context.Entry(form).Reference(x => x.MtdFormDesk).LoadAsync();
            }

            return Page();
        }

    }
}

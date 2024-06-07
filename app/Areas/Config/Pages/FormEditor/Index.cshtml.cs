/*
    MTD OrderMaker - http://mtdkey.com
    Copyright (c) 2019 Oleg Bruev <job4bruev@gmail.com>. All rights reserved.

*/


using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MtdKey.OrderMaker.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MtdKey.OrderMaker.Areas.Config.Pages.FormEditor
{
    public class IndexModel : PageModel
    {
        private readonly DataConnector _context;

        public IndexModel(DataConnector context)
        {
            _context = context;
        }

        public IList<MtdForm> MtdForms { get; set; }
        public string SearchText { get; set; }

        public async Task<IActionResult> OnGetAsync(string searchText)
        {
            var query = _context.MtdForm.AsQueryable();

            if (searchText != null)
            {
                string normText = searchText.ToUpper();
                query = query.Where(x => x.Name.ToUpper().Contains(normText) ||
                                        x.Description.ToUpper().Contains(normText)
                                        );
                SearchText = searchText;
            }

            MtdForms = await query.OrderBy(x => x.Sequence).ToListAsync();

            foreach (MtdForm form in MtdForms)
            {
                await _context.Entry(form)
                    .Reference(x => x.MtdFormDesk)
                    .LoadAsync();
            }

            return Page();

        }
    }
}

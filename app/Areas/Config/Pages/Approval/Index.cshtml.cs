/*
    MTD OrderMaker - http://mtdkey.com
    Copyright (c) 2019 Oleg Bruev <job4bruev@gmail.com>. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MtdKey.OrderMaker.Entity;

namespace MtdKey.OrderMaker.Areas.Config.Pages.Approval
{

    public class IndexModel : PageModel
    {
        private readonly DataConnector _context;
        
        public IndexModel(DataConnector context)
        {
            _context = context;
        }

        public IList<MtdApproval> MtdApprovals { get; set; }
        public string SearchText { get; set; }
        public async Task<IActionResult> OnGetAsync(string searchText)
        {
            var query = _context.MtdApproval.AsQueryable();

            if (searchText != null)
            {
                string normText = searchText.ToUpper();
                query = query.Where(x => x.Name.ToUpper().Contains(normText) ||
                                        x.Description.ToUpper().Contains(normText)
                                        );
                SearchText = searchText;
            }
            
            MtdApprovals = await query.ToListAsync();
            foreach(var MtdApproval in MtdApprovals)
            {
                await _context.Entry(MtdApproval).Reference(x=>x.MtdFormNavigation).LoadAsync();
                await _context.Entry(MtdApproval.MtdFormNavigation).Reference(x => x.MtdFormDesk).LoadAsync();
            }
            return Page();
        }
    }
}
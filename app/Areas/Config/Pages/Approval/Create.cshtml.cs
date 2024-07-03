/*
    MTD OrderMaker - http://mtdkey.com
    Copyright (c) 2019 Oleg Bruev <job4bruev@gmail.com>. All rights reserved.
*/

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MtdKey.OrderMaker.Entity;
using MtdKey.OrderMaker.Models.Controls.MTDSelectList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MtdKey.OrderMaker.Areas.Config.Pages.Approval
{
    public class CreateModel : PageModel
    {
        private readonly DataConnector _context;

        public CreateModel(DataConnector context)
        {
            _context = context;
        }

        [BindProperty]
        public MtdApproval MtdApproval { get; set; }
        public IList<MtdForm> MtdForms { get; set; }
        public List<MTDSelectListItem> FormItems { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            MtdApproval = new MtdApproval
            {
                Id = Guid.NewGuid().ToString()
            };

            IList<string> formsIds = await _context.MtdApproval.Select(x => x.MtdForm).ToListAsync();
            MtdForms = await _context.MtdForm.Where(x => !formsIds.Contains(x.Id)).OrderBy(x => x.Sequence).ToListAsync();
            ViewData["Forms"] = new SelectList(MtdForms.OrderBy(x => x.Sequence), "Id", "Name");

            FormItems = new List<MTDSelectListItem>();
            foreach (MtdForm item in MtdForms)
            {
                FormItems.Add(new MTDSelectListItem { Id = item.Id, Value = item.Name });
            }

            return Page();
        }

        //public async Task<IActionResult> OnPostAsync()
        //{

        //    _context.MtdApproval.Add(MtdApproval);
        //    await _context.SaveChangesAsync();

        //    return RedirectToPage("./Edit", new { id = MtdApproval.Id });
        //}
    }
}
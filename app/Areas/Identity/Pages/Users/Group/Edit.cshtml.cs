/*
    MTD OrderMaker - http://mtdkey.com
    Copyright (c) 2019 Oleg Bruev <job4bruev@gmail.com>. All rights reserved.
*/

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MtdKey.OrderMaker.Entity;
using System.Threading.Tasks;

namespace MtdKey.OrderMaker.Areas.Identity.Pages.Users.Group
{
    public class EditModel : PageModel
    {
        private readonly DataConnector _context;

        public EditModel(DataConnector context)
        {
            _context = context;
        }

        public MtdGroup MtdGroup { get; set; }
        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null) { return NotFound(); }
            MtdGroup = await _context.MtdGroup.FindAsync(id);
            if (MtdGroup == null) { return NotFound(); }

            return Page();
        }

    }
}
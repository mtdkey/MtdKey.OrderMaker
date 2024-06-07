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
    public class CreateModel : PageModel
    {
        private readonly DataConnector _context;

        public CreateModel(DataConnector context)
        {
            _context = context;
        }

        [BindProperty]
        public MtdGroup MtdGroup { get; set; }

        public void OnGet()
        {
            MtdGroup = new MtdGroup();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await _context.MtdGroup.AddAsync(MtdGroup);
            await _context.SaveChangesAsync();
            return Page();
        }

    }
}
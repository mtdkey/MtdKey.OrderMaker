/*
    MTD OrderMaker - http://mtdkey.com
    Copyright (c) 2019 Oleg Bruev <job4bruev@gmail.com>. All rights reserved.
 */

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MtdKey.OrderMaker.Entity;
using System.Threading.Tasks;

namespace MtdKey.OrderMaker.Areas.Config.Pages.Approval
{
    public class StageCreateModel : PageModel
    {
        private readonly OrderMakerContext _context;

        public StageCreateModel(OrderMakerContext context)
        {
            _context = context;
        }

        public MtdApproval MtdApproval { get; set; }

        public async Task<IActionResult> OnGetAsync(string idApproval)
        {

            MtdApproval = await _context.MtdApproval.FindAsync(idApproval);
            if (MtdApproval == null)
            {
                return NotFound();
            }

            return Page();
        }

    }
}
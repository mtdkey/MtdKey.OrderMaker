/*
    MTD OrderMaker - http://mtdkey.com
    Copyright (c) 2019 Oleg Bruev <job4bruev@gmail.com>. All rights reserved.
*/


using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MtdKey.OrderMaker.Entity;
using MtdKey.OrderMaker.Services;
using System.Threading.Tasks;

namespace MtdKey.OrderMaker.Areas.Workplace.Pages.Store
{
    public class DetailsPrintModel : PageModel
    {
        private readonly OrderMakerContext _context;
        private readonly UserHandler _userHandler;

        public DetailsPrintModel(OrderMakerContext context, UserHandler userHandler)
        {
            _context = context;
            _userHandler = userHandler;
        }

        public MtdStore MtdStore { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            MtdStore = await _context.MtdStore.FirstOrDefaultAsync(m => m.Id == id);

            if (MtdStore == null)
            {
                return NotFound();
            }

            var user = await _userHandler.GetUserAsync(HttpContext.User);
            bool isViewer = await _userHandler.IsViewer(user, MtdStore.MtdFormId, MtdStore.Id);

            if (!isViewer)
            {
                return Forbid();
            }

            return Page();
        }
    }
}
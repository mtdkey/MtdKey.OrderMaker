/*
    MTD OrderMaker - http://mtdkey.com
    Copyright (c) 2019 Oleg Bruev <job4bruev@gmail.com>. All rights reserved.
*/

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MtdKey.OrderMaker.Entity;
using MtdKey.OrderMaker.Services;
using System.Threading.Tasks;

namespace MtdKey.OrderMaker.Areas.Workplace.Pages.Store
{
    public class CreateModel : PageModel
    {
        private readonly DataConnector _context;
        private readonly UserHandler _userHandler;

        public CreateModel(DataConnector context, UserHandler userHandler)
        {
            _context = context;
            _userHandler = userHandler;
        }

        [BindProperty]
        public MtdStore MtdStore { get; set; }
        public MtdForm MtdForm { get; set; }

        public async Task<IActionResult> OnGet(string formId)
        {

            if (formId == null)
            {
                return NotFound();
            }

            var user = await _userHandler.GetUserAsync(HttpContext.User);
            bool isCreator = await _userHandler.IsCreator(user, formId);

            if (!isCreator)
            {
                return Forbid();
            }

            MtdForm = await _context.MtdForm.FindAsync(formId);
            MtdStore = new MtdStore { MtdFormId = MtdForm.Id, MtdFormNavigation = MtdForm };

            return Page();
        }

    }
}
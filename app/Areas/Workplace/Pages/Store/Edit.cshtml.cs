/*
    MTD OrderMaker - http://mtdkey.com
    Copyright (c) 2019 Oleg Bruev <job4bruev@gmail.com>. All rights reserved.
*/

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MtdKey.OrderMaker.Areas.Identity.Data;
using MtdKey.OrderMaker.Core;
using MtdKey.OrderMaker.Entity;
using MtdKey.OrderMaker.Services;

namespace MtdKey.OrderMaker.Areas.Workplace.Pages.Store
{
    public class EditModel : PageModel
    {
        private readonly DataConnector _context;
        private readonly UserHandler _userHandler;

        public EditModel(DataConnector context, UserHandler userHandler)
        {
            _context = context;
            _userHandler = userHandler;
        }


        public MtdForm MtdForm { get; set; }
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
            bool isEditor = await _userHandler.IsEditor(user,MtdStore.MtdFormId,MtdStore.Id);
            
            if (!isEditor) {
                return Forbid();
            }

            WebAppUser webUser = await _userHandler.GetUserAsync(HttpContext.User);
            ApprovalHandler approvalHandler = new(_context, MtdStore.Id);
            ApprovalStatus approvalStatus = await approvalHandler.GetStatusAsync(webUser);

            if (approvalStatus == ApprovalStatus.Rejected)
            {
                return Forbid();
            }

            MtdForm = await _context.MtdForm.FindAsync(MtdStore.MtdFormId);

            return Page();
        }

    }
}

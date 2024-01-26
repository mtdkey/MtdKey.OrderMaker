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
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MtdKey.OrderMaker.Areas.Identity.Data;
using MtdKey.OrderMaker.Entity;
using MtdKey.OrderMaker.Services;

namespace MtdKey.OrderMaker.Areas.Config.Pages.Approval
{
    public class StageCreateModel : PageModel
    {
        private readonly DataConnector _context;

        public StageCreateModel(DataConnector context)
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
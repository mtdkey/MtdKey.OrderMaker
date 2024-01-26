/*
    MTD OrderMaker - http://mtdkey.com
    Copyright (c) 2019 Oleg Bruev <job4bruev@gmail.com>. All rights reserved.
*/


using System.Collections.Generic;
using System.Composition;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.CodeAnalysis.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MtdKey.OrderMaker.AppConfig;
using MtdKey.OrderMaker.Areas.Identity.Data;
using MtdKey.OrderMaker.Entity;
using MtdKey.OrderMaker.Services;

namespace MtdKey.OrderMaker.Areas.Workplace.Pages.Store
{
    public class IndexModel : PageModel
    {
        private readonly DataConnector _context;
        private readonly UserHandler _userHandler;
        private readonly LimitSettings limit;

        public IndexModel(DataConnector context, UserHandler userHandler, IOptions<LimitSettings> limit)
        {
            _context = context;
            _userHandler = userHandler;
            this.limit = limit.Value;
        }

        public MtdForm MtdForm { get; set; }
        public bool ExportToExcel { get; set; }

        public async Task<IActionResult> OnGetAsync(string indexForm)
        {
            WebAppUser user = await _userHandler.GetUserAsync(HttpContext.User);
            bool isViewer = await _userHandler.CheckUserPolicyAsync(user, indexForm, RightsType.ViewAll);
            bool GroupRight = await _userHandler.CheckUserPolicyAsync(user, indexForm, RightsType.ViewGroup);
            bool OwnerRight = await _userHandler.CheckUserPolicyAsync(user, indexForm, RightsType.ViewOwn);            

            if (!isViewer & !OwnerRight & !GroupRight)
            {
                return Forbid(); 
            }

            MtdForm = await _context.MtdForm.FindAsync(indexForm);

            if (MtdForm == null)
            {
                return NotFound();
            }

            bool exporter = await _userHandler.CheckUserPolicyAsync(user, indexForm, RightsType.ExportToExcel);
            ExportToExcel = limit.ExportExcel && exporter;

            ViewData["FormId"] = indexForm;
            return Page();
        }

    }
}

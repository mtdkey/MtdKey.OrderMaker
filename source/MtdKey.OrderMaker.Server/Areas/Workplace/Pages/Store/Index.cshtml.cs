/*
    MTD OrderMaker - http://mtdkey.com
    Copyright (c) 2019 Oleg Bruev <job4bruev@gmail.com>. All rights reserved.
*/


using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using MtdKey.OrderMaker.AppConfig;
using MtdKey.OrderMaker.Areas.Identity.Data;
using MtdKey.OrderMaker.Entity;
using MtdKey.OrderMaker.Services;
using System.Threading.Tasks;

namespace MtdKey.OrderMaker.Areas.Workplace.Pages.Store
{
    public class IndexModel : PageModel
    {
        private readonly OrderMakerContext _context;
        private readonly UserHandler _userHandler;
        private readonly LimitSettings limit;

        public IndexModel(OrderMakerContext context, UserHandler userHandler, IOptions<LimitSettings> limit)
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

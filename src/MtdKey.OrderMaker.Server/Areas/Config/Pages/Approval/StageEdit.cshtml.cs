/*
    MTD OrderMaker - http://mtdkey.com
    Copyright (c) 2019 Oleg Bruev <job4bruev@gmail.com>. All rights reserved.
 */

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MtdKey.OrderMaker.Areas.Identity.Data;
using MtdKey.OrderMaker.Entity;
using MtdKey.OrderMaker.Models.Controls.MTDSelectList;
using MtdKey.OrderMaker.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MtdKey.OrderMaker.Areas.Config.Pages.Approval
{
    public class StageEditModel : PageModel
    {
        private readonly DataConnector _context;
        private readonly UserHandler _userHandler;

        public StageEditModel(DataConnector context, UserHandler userHandler)
        {
            _context = context;
            _userHandler = userHandler;
        }

        public MtdApproval MtdApproval { get; set; }
        public MtdApprovalStage MtdApprovalStage { get; set; }
        public IList<MtdFormPart> MtdFormParts { get; set; }
        public IList<MtdApprovalResolution> Resolutions { get; set; }
        public IList<MtdApprovalRejection> Rejections { get; set; }

        public List<MTDSelectListItem> UserItems { get; set; }
        public async Task<IActionResult> OnGetAsync(int id)
        {

            MtdApprovalStage = await _context.MtdApprovalStage.FindAsync(id);

            if (MtdApprovalStage == null)
            {
                return NotFound();
            }

            MtdApproval = await _context.MtdApproval
                .Where(x => x.Id == MtdApprovalStage.MtdApproval)
                .FirstOrDefaultAsync();

            await _context.Entry(MtdApproval).Reference(x => x.MtdFormNavigation).LoadAsync();

            MtdFormParts = await _context.MtdFormPart.Where(x => x.MtdFormId == MtdApproval.MtdForm).OrderBy(x => x.Sequence).ToListAsync();
            IList<WebAppUser> webAppUsers = await _userHandler.Users.OrderBy(x => x.Title).ToListAsync();
            UserItems = new List<MTDSelectListItem>
            {
                new MTDSelectListItem { Id = "owner", Value = "Owner", Selectded = MtdApprovalStage.UserId == "owner", Localized=true }
            };
            webAppUsers.ToList().ForEach((item) =>
            {
                UserItems.Add(new MTDSelectListItem { Id = item.Id, Value = item.Title, Selectded = MtdApprovalStage.UserId == item.Id });
            });


            ViewData["Users"] = new SelectList(webAppUsers, "Id", "Title");

            Resolutions = await _context.MtdApprovalResolution.Where(x => x.MtdApprovalStageId == id).OrderBy(x => x.Sequence).ToListAsync();
            Rejections = await _context.MtdApprovalRejection.Where(x => x.MtdApprovalStageId == id).OrderBy(x => x.Sequence).ToListAsync();

            return Page();
        }
    }
}
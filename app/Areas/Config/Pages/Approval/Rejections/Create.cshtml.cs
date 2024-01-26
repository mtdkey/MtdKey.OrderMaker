using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MtdKey.OrderMaker.Components;
using MtdKey.OrderMaker.Entity;

namespace MtdKey.OrderMaker.Areas.Config.Pages.Approval.Rejections
{
    public class CreateModel : PageModel
    {
        private readonly DataConnector _context;

        public CreateModel(DataConnector context)
        {
            _context = context;
        }

        [BindProperty]
        public MtdApprovalRejection MtdApprovalRejection { get; set; }

        [BindProperty]
        public string ID { get; set; }
        [BindProperty]
        public MtdApprovalStage MtdApprovalStage { get; set; }
        public async Task<IActionResult> OnGetAsync(int stage)
        {
            MtdApprovalStage = await _context.MtdApprovalStage.FindAsync(stage);
            if (MtdApprovalStage == null) { return NotFound(); }
            MtdApprovalRejection = new MtdApprovalRejection { Id = Guid.NewGuid().ToString() };
            return Page();
        }


        public async Task OnPostAsync()
        {
            MtdApprovalRejection.MtdApprovalStageId = MtdApprovalStage.Id;

            MTDImgSModify img = await MTDImgSelector.ImageModifyAsync("img", Request);
            MtdApprovalRejection.ImgData = img.Image;
            MtdApprovalRejection.ImgType = img.ImgType;

            _context.MtdApprovalRejection.Add(MtdApprovalRejection);
            await _context.SaveChangesAsync();

        }
    }
}
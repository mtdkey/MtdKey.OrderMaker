using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MtdKey.OrderMaker.Components;
using MtdKey.OrderMaker.Entity;
using System;
using System.Threading.Tasks;

namespace MtdKey.OrderMaker.Areas.Config.Pages.Approval.Resolutions
{
    public class CreateModel : PageModel
    {
        private readonly DataConnector _context;

        public CreateModel(DataConnector context)
        {
            _context = context;
        }

        [BindProperty]
        public MtdApprovalResolution MtdApprovalResolution { get; set; }

        [BindProperty]
        public MtdApprovalStage MtdApprovalStage { get; set; }
        public async Task<IActionResult> OnGetAsync(int stage)
        {

            MtdApprovalStage = await _context.MtdApprovalStage.FindAsync(stage);
            if (MtdApprovalStage == null) { return NotFound(); }
            MtdApprovalResolution = new MtdApprovalResolution { Id = Guid.NewGuid().ToString() };

            return Page();
        }



        public async Task OnPostAsync()
        {
            MtdApprovalResolution.MtdApprovalStageId = MtdApprovalStage.Id;

            MTDImgSModify img = await MTDImgSelector.ImageModifyAsync("img", Request);
            MtdApprovalResolution.ImgData = img.Image;
            MtdApprovalResolution.ImgType = img.ImgType;

            _context.MtdApprovalResolution.Add(MtdApprovalResolution);
            await _context.SaveChangesAsync();

        }
    }
}
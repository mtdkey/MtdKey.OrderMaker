using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MtdKey.OrderMaker.Components;
using MtdKey.OrderMaker.Entity;
using System;
using System.Threading.Tasks;

namespace MtdKey.OrderMaker.Areas.Config.Pages.Approval.Rejections
{
    public class CreateModel : PageModel
    {
        private readonly OrderMakerContext _context;

        public CreateModel(OrderMakerContext context)
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
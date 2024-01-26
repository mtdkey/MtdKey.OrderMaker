using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MtdKey.OrderMaker.Components;
using MtdKey.OrderMaker.Entity;

namespace MtdKey.OrderMaker.Areas.Config.Pages.Approval.Rejections
{
    public class EditModel : PageModel
    {
        private readonly DataConnector _context;

        public EditModel(DataConnector context)
        {
            _context = context;
        }

        [BindProperty]
        public MtdApprovalRejection MtdApprovalRejection { get; set; }

        [BindProperty]
        public MtdApprovalStage MtdApprovalStage { get; set; }
        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            MtdApprovalRejection = await _context.MtdApprovalRejection.FirstOrDefaultAsync(m => m.Id == id);

            if (MtdApprovalRejection == null)
            {
                return NotFound();
            }

            await _context.Entry(MtdApprovalRejection).Reference(x => x.MtdApprovalStage).LoadAsync();
            MtdApprovalStage = MtdApprovalRejection.MtdApprovalStage;            
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {

            _context.Attach(MtdApprovalRejection).State = EntityState.Modified;
            MTDImgSModify img = await MTDImgSelector.ImageModifyAsync("img", Request);
            MtdApprovalRejection.ImgData = img.Image;
            MtdApprovalRejection.ImgType = img.ImgType;
            _context.Entry(MtdApprovalRejection).Property(x => x.ImgType).IsModified = img.Modify;
            _context.Entry(MtdApprovalRejection).Property(x => x.ImgData).IsModified = img.Modify;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MtdApprovalRejectionExists(MtdApprovalRejection.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return new OkResult();
        }

        public async Task<IActionResult> OnPostDeleteAsync()
        {
            _context.MtdApprovalRejection.Remove(MtdApprovalRejection);
            await _context.SaveChangesAsync();
            return new OkResult();
        }

        private bool MtdApprovalRejectionExists(string id)
        {
            return _context.MtdApprovalRejection.Any(e => e.Id == id);
        }
    }
}

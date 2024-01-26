using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MtdKey.OrderMaker.Components;
using MtdKey.OrderMaker.Entity;

namespace MtdKey.OrderMaker.Areas.Config.Pages.Approval.Resolutions
{
    public class EditModel : PageModel
    {
        private readonly DataConnector _context;

        public EditModel(DataConnector context)
        {
            _context = context;
        }

        [BindProperty]
        public MtdApprovalResolution MtdApprovalResolution { get; set; }

        [BindProperty]
        public MtdApprovalStage MtdApprovalStage { get; set; }
        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            MtdApprovalResolution = await _context.MtdApprovalResolution.FirstOrDefaultAsync(m => m.Id == id);

            if (MtdApprovalResolution == null)
            {
                return NotFound();
            }

            await _context.Entry(MtdApprovalResolution).Reference(x => x.MtdApprovalStage).LoadAsync();
            MtdApprovalStage = MtdApprovalResolution.MtdApprovalStage;            
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {

            _context.Attach(MtdApprovalResolution).State = EntityState.Modified;
            MTDImgSModify img = await MTDImgSelector.ImageModifyAsync("img", Request);
            MtdApprovalResolution.ImgData = img.Image;
            MtdApprovalResolution.ImgType = img.ImgType;
            _context.Entry(MtdApprovalResolution).Property(x => x.ImgType).IsModified = img.Modify;
            _context.Entry(MtdApprovalResolution).Property(x => x.ImgData).IsModified = img.Modify;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MtdApprovalResolutionExists(MtdApprovalResolution.Id))
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
            _context.MtdApprovalResolution.Remove(MtdApprovalResolution);
            await _context.SaveChangesAsync();
            return new OkResult();
        }

        private bool MtdApprovalResolutionExists(string id)
        {
            return _context.MtdApprovalResolution.Any(e => e.Id == id);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MtdKey.OrderMaker.Entity;

namespace MtdKey.OrderMaker.Areas.Config.Pages.Approval.Resolutions
{
    public class IndexModel : PageModel
    {
        private readonly DataConnector _context;

        public IndexModel(DataConnector context)
        {
            _context = context;
        }

        public IList<MtdApprovalResolution> Resolutions { get; set; }

        public MtdApprovalStage MtdApprovalStage { get; set; }
        [BindProperty]
        public string SearchText { get; set; }
        public async Task<IActionResult> OnGetAsync(string searchText, int stage=0)
        {
            SearchText = searchText;
            if (stage == 0) { return NotFound(); }
            MtdApprovalStage = await _context.MtdApprovalStage.FindAsync(stage);
            if (MtdApprovalStage == null) { return NotFound(); }
            IQueryable<MtdApprovalResolution> query = _context.MtdApprovalResolution.Where(x => x.MtdApprovalStageId == stage);

            if (searchText != null)
            {
                string text = searchText.ToUpper();
                query = query.Where(x => x.Name.ToUpper().Contains(text));
            }

            Resolutions = await query.OrderBy(x => x.Sequence).ToListAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostSequenceAsync()
        {
            string strData = Request.Form["drgData"];
            string stage = Request.Form["stage-id"];

            int.TryParse(stage, out int stageId);

            string[] data = strData.Split("&");

            IList<MtdApprovalResolution> resolutions = await _context.MtdApprovalResolution.Where(x => x.MtdApprovalStageId == stageId).ToListAsync();

            int counter = 1;
            foreach (string id in data)
            {

                var field = resolutions.Where(x => x.Id == id).FirstOrDefault();
                if (field != null)
                {
                    field.Sequence = counter;
                    counter++;
                }

            }

            _context.MtdApprovalResolution.UpdateRange(resolutions);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index",new {stage=stageId,searchText=SearchText});
        }
    }
}

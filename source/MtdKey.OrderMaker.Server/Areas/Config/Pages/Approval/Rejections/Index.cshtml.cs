using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MtdKey.OrderMaker.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MtdKey.OrderMaker.Areas.Config.Pages.Approval.Rejections
{
    public class IndexModel : PageModel
    {
        private readonly OrderMakerContext _context;

        public IndexModel(OrderMakerContext context)
        {
            _context = context;
        }

        public IList<MtdApprovalRejection> Rejections { get; set; }

        public MtdApprovalStage MtdApprovalStage { get; set; }
        [BindProperty]
        public string SearchText { get; set; }
        public async Task<IActionResult> OnGetAsync(string searchText, int stage = 0)
        {
            SearchText = searchText;
            if (stage == 0) { return NotFound(); }
            MtdApprovalStage = await _context.MtdApprovalStage.FindAsync(stage);
            if (MtdApprovalStage == null) { return NotFound(); }
            IQueryable<MtdApprovalRejection> query = _context.MtdApprovalRejection.Where(x => x.MtdApprovalStageId == stage);

            if (searchText != null)
            {
                string text = searchText.ToUpper();
                query = query.Where(x => x.Name.ToUpper().Contains(text));
            }

            Rejections = await query.OrderBy(x => x.Sequence).ToListAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostSequenceAsync()
        {
            string strData = Request.Form["drgData"];
            string stage = Request.Form["stage-id"];

            int.TryParse(stage, out int stageId);

            string[] data = strData.Split("&");

            IList<MtdApprovalRejection> rejections = await _context.MtdApprovalRejection.Where(x => x.MtdApprovalStageId == stageId).ToListAsync();

            int counter = 1;
            foreach (string id in data)
            {

                var field = rejections.Where(x => x.Id == id).FirstOrDefault();
                if (field != null)
                {
                    field.Sequence = counter;
                    counter++;
                }

            }

            _context.MtdApprovalRejection.UpdateRange(rejections);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index", new { stage = stageId, searchText = SearchText });
        }
    }
}

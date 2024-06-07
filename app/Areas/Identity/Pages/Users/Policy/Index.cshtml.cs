using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MtdKey.OrderMaker.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MtdKey.OrderMaker.Areas.Identity.Pages.Users.Policy
{
    public class IndexModel : PageModel
    {
        private readonly DataConnector _context;

        public IndexModel(DataConnector context)
        {
            _context = context;
        }

        public IList<MtdPolicy> MtdPolicies { get; set; }
        public string SearchText { get; set; }
        public async Task<IActionResult> OnGetAsync(string searchText)
        {
            var query = _context.MtdPolicy.AsQueryable();

            if (searchText != null)
            {
                string normText = searchText.ToUpper();
                query = query.Where(x => x.Name.ToUpper().Contains(normText) ||
                                        x.Description.ToUpper().Contains(normText)
                                        );
                SearchText = searchText;
            }


            MtdPolicies = await query.OrderBy(x => x.Name).ToListAsync();
            return Page();
        }
    }
}
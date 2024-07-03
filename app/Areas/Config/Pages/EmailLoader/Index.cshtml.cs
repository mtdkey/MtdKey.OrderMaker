using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MtdKey.OrderMaker.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MtdKey.OrderMaker.Areas.Config.Pages.EmailLoader
{
    public class IndexModel(OrderMakerContext context) : PageModel
    {
        private readonly OrderMakerContext context = context;


        public IList<FormLoaderModel> FormLoaderModels { get; private set; } = [];

        public async Task<IActionResult> OnGetAsync()
        {

            var targetForms = await context.TargetForms
                .Include(x => x.TargetFields)
                .Include(x => x.TargetFilters)
                .AsNoTracking()
                .AsSplitQuery()
                .ToListAsync();

            var formIds = targetForms.Select(x => x.FormId).ToList();

            var formList = await context.MtdForm
                .Where(x => formIds.Contains(x.Id))
                .AsNoTracking()
                .ToListAsync();

            targetForms.ForEach(target =>
            {
                FormLoaderModels.Add(new FormLoaderModel
                {
                    FormId = target.FormId,
                    Name = formList.FirstOrDefault(x => x.Id == target.FormId)?.Name,
                    Email = target.TargetFilters.FirstOrDefault()?.EmailAsKey,

                });
            });


            return Page();
        }
    }
}

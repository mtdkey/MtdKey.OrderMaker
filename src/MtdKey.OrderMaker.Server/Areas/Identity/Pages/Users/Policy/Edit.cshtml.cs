/*
    MTD OrderMaker - http://mtdkey.com
    Copyright (c) 2019 Oleg Bruev <job4bruev@gmail.com>. All rights reserved.
*/

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MtdKey.OrderMaker.AppConfig;
using MtdKey.OrderMaker.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MtdKey.OrderMaker.Areas.Identity.Pages.Users.Policy
{
    public class EditModel : PageModel
    {
        private readonly DataConnector _context;
        private readonly LimitSettings limit;

        public EditModel(DataConnector context, IOptions<LimitSettings> limit)
        {
            _context = context;
            this.limit = limit.Value;
        }

        public MtdPolicy MtdPolicy { get; set; }
        public IList<MtdForm> MtdForms { get; set; }
        public IList<MtdGroup> MtdGroups { get; set; }
        public bool ExportToExcel { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null) { return NotFound(); }

            MtdPolicy = await _context.MtdPolicy
                .FirstOrDefaultAsync(x => x.Id == id);

            if (MtdPolicy == null) { return NotFound(); }
            await _context.Entry(MtdPolicy).Collection(x => x.MtdPolicyForms).LoadAsync();
            await _context.Entry(MtdPolicy).Collection(x => x.MtdPolicyParts).LoadAsync();

            MtdGroups = await _context.MtdGroup.OrderBy(x => x.Name).ToListAsync();

            MtdForms = await _context.MtdForm
                .OrderBy(x => x.Sequence).ToListAsync();

            foreach (MtdForm form in MtdForms)
            {
                await _context.Entry(form).Collection(x => x.MtdFormParts).LoadAsync();
            }

            ExportToExcel = limit.ExportExcel;
            return Page();
        }

    }
}
/*
    MTD OrderMaker - http://mtdkey.com
    Copyright (c) 2019 Oleg Bruev <job4bruev@gmail.com>. All rights reserved.
*/

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using MtdKey.OrderMaker.AppConfig;
using MtdKey.OrderMaker.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MtdKey.OrderMaker.Areas.Config.Pages.FormEditor
{
    public class CreateModel : PageModel
    {

        private readonly OrderMakerContext _context;
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly IOptions<LimitSettings> limits;

        public CreateModel(OrderMakerContext context, IStringLocalizer<SharedResource> localizer, IOptions<LimitSettings> limits)
        {
            _context = context;
            _localizer = localizer;
            this.limits = limits;
        }

        [BindProperty]
        public MtdForm MtdForm { get; set; }
        public IList<MtdForm> MtdForms { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            MtdForm = new MtdForm
            {
                Id = Guid.NewGuid().ToString(),
            };

            MtdForms = await _context.MtdForm.ToListAsync();
            ViewData["Forms"] = new SelectList(MtdForms.OrderBy(x => x.Sequence), "Id", "Name");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            int formQty = await _context.MtdForm.CountAsync();
            int formLimit = limits.Value.Forms;

            if (formQty >= formLimit) { return BadRequest(_localizer["Forms Limit!"]); }

            var group = await _context.MtdCategoryForm.FirstOrDefaultAsync();

            MtdForm.MtdCategory = group.Id;
            MtdForm.Active = 1;

            var formPart = new MtdFormPart
            {
                Id = Guid.NewGuid().ToString(),
                MtdFormId = MtdForm.Id,
                Name = _localizer["Main content"],
                Description = _localizer["Main content"],
                MtdSysStyle = 4,
                Sequence = 1,
                Title = 1
            };

            MtdForm.MtdFormParts.Add(formPart);
            await _context.MtdForm.AddAsync(MtdForm);

            await _context.SaveChangesAsync();

            return LocalRedirect($"/config/formeditor/index?formId={MtdForm.Id}");
        }
    }
}
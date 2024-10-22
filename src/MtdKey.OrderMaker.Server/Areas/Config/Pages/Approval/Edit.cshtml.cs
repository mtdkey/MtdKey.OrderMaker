/*
    MTD OrderMaker - http://mtdkey.com
    Copyright (c) 2019 Oleg Bruev <job4bruev@gmail.com>. All rights reserved.
 */

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MtdKey.OrderMaker.Components;
using MtdKey.OrderMaker.Entity;
using System.Threading.Tasks;

namespace MtdKey.OrderMaker.Areas.Config.Pages.Approval
{
    public class EditModel : PageModel
    {
        private readonly DataConnector _context;

        public EditModel(DataConnector context)
        {
            _context = context;
        }

        [BindProperty]
        public MtdApproval MtdApproval { get; set; }
        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            MtdApproval = await _context.MtdApproval.FirstOrDefaultAsync(x => x.Id == id);
            if (MtdApproval == null)
            {
                return NotFound();
            }
            await _context.Entry(MtdApproval).Reference(x => x.MtdFormNavigation).LoadAsync();

            return Page();
        }


        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(MtdApproval).State = EntityState.Modified;
            _context.Entry(MtdApproval).Property(x => x.MtdForm).IsModified = false;

            MTDImgSModify mStart = await MTDImgSelector.ImageModifyAsync("img-start", Request);
            MtdApproval.ImgStart = mStart.Image;
            MtdApproval.ImgStartType = mStart.ImgType;
            _context.Entry(MtdApproval).Property(x => x.ImgStart).IsModified = mStart.Modify;
            _context.Entry(MtdApproval).Property(x => x.ImgStartType).IsModified = mStart.Modify;

            MTDImgSModify mIteraction = await MTDImgSelector.ImageModifyAsync("img-iteraction", Request);
            MtdApproval.ImgIteraction = mIteraction.Image;
            MtdApproval.ImgIteractionType = mIteraction.ImgType;
            _context.Entry(MtdApproval).Property(x => x.ImgIteraction).IsModified = mIteraction.Modify;
            _context.Entry(MtdApproval).Property(x => x.ImgIteractionType).IsModified = mIteraction.Modify;

            MTDImgSModify mRequired = await MTDImgSelector.ImageModifyAsync("img-required", Request);
            MtdApproval.ImgRequired = mRequired.Image;
            MtdApproval.ImgRequiredType = mRequired.ImgType;
            _context.Entry(MtdApproval).Property(x => x.ImgRequired).IsModified = mRequired.Modify;
            _context.Entry(MtdApproval).Property(x => x.ImgRequiredType).IsModified = mRequired.Modify;

            MTDImgSModify mWaiting = await MTDImgSelector.ImageModifyAsync("img-waiting", Request);
            MtdApproval.ImgWaiting = mWaiting.Image;
            MtdApproval.ImgWaitingType = mWaiting.ImgType;
            _context.Entry(MtdApproval).Property(x => x.ImgWaiting).IsModified = mWaiting.Modify;
            _context.Entry(MtdApproval).Property(x => x.ImgWaitingType).IsModified = mWaiting.Modify;

            MTDImgSModify mApproved = await MTDImgSelector.ImageModifyAsync("img-approved", Request);
            MtdApproval.ImgApproved = mApproved.Image;
            MtdApproval.ImgApprovedType = mApproved.ImgType;
            _context.Entry(MtdApproval).Property(x => x.ImgApproved).IsModified = mApproved.Modify;
            _context.Entry(MtdApproval).Property(x => x.ImgApprovedType).IsModified = mApproved.Modify;

            MTDImgSModify mRejected = await MTDImgSelector.ImageModifyAsync("img-rejected", Request);
            MtdApproval.ImgRejected = mRejected.Image;
            MtdApproval.ImgRejectedType = mRejected.ImgType;
            _context.Entry(MtdApproval).Property(x => x.ImgRejected).IsModified = mRejected.Modify;
            _context.Entry(MtdApproval).Property(x => x.ImgRejectedType).IsModified = mRejected.Modify;


            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }


    }
}
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using MtdKey.OrderMaker.Entity;
using System;
using System.Linq;
using System.Threading.Tasks;
using MtdKey.OrderMaker.Services;
using MtdKey.OrderMaker.Areas.Identity.Data;

namespace MtdKey.OrderMaker.Components.Store
{
    public class StoreViewerModel
    {
        public string SoreId { get; set; }
        public string ImgSrc { get; set; }
        public string FormName { get; set; }
        public string DocName { get; set; }
        public bool IsViewer { get; set; }
        public string ViewerId { get; set; }
    }

    [ViewComponent(Name = "StoreViewer")]
    public class StoreViewer : ViewComponent
    {
        private readonly DataConnector context;
        private readonly IStringLocalizer<SharedResource> localizer;
        private readonly UserHandler userHandler;

        public StoreViewer(DataConnector context, IStringLocalizer<SharedResource> localizer, UserHandler userHandler)
        {
            this.context = context;
            this.localizer = localizer;
            this.userHandler = userHandler;
        }

        public async Task<IViewComponentResult> InvokeAsync(string storeId, string viewerId)            
        {
            if (storeId == null)
            {
                               
                StoreViewerModel nullModel = new()
                {
                    SoreId = string.Empty,
                    ImgSrc = string.Empty,
                    FormName = string.Empty,
                    DocName = string.Empty,
                    ViewerId = viewerId
                };

                return View(nullModel);
            }

            MtdStore mtdStore = await context.MtdStore.FindAsync(storeId);
            MtdForm mtdForm = await context.MtdForm.Where(x => x.Id == mtdStore.MtdFormId).FirstOrDefaultAsync();
            await context.Entry(mtdForm).Reference(x => x.MtdFormHeader).LoadAsync();

            WebAppUser user = await userHandler.GetUserAsync(HttpContext.User);
            bool isViewer = await userHandler.IsViewer(user,mtdForm.Id, storeId);
           
            string imgSrc = string.Empty;
            if (mtdForm.MtdFormHeader != null)
            {
                string base64 = Convert.ToBase64String(mtdForm.MtdFormHeader.Image);
                imgSrc = String.Format("data:{0};base64,{1}", mtdForm.MtdFormHeader.ImageType, base64);
            }


            StoreViewerModel model = new()
            {
                SoreId = storeId,
                ImgSrc = imgSrc,
                FormName = mtdForm.Name,
                DocName = $"{localizer["No."]} {mtdStore.Sequence:D9} {localizer["at"]} {mtdStore.Timecr.ToShortDateString()}",
                ViewerId = viewerId,
                IsViewer = isViewer
            };

            return View(model);
        }
    }
}

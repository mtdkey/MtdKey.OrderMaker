using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using MtdKey.OrderMaker.Models.Controls.MTDSelectList;
using System.Threading.Tasks;

namespace MtdKey.OrderMaker.Components.Controls
{

    [ViewComponent(Name = "MTDSelectList")]
    public class MTDSelectList : ViewComponent
    {
        private readonly IStringLocalizer<SharedResource> Localizer;
        public MTDSelectList(IStringLocalizer<SharedResource> localizer)
        {
            Localizer = localizer;
        }

        public async Task<IViewComponentResult> InvokeAsync(MTDSelectListTags tags)
        {
            MTDSelectListTagsModel model = new(tags);
            LocalizerModel(model);

            string viewName = model.MTDSelectListView.ToString();

            return await Task.Run(() => View(viewName, model));
        }


        private void LocalizerModel(MTDSelectListTagsModel model)
        {

            model.SearchTextPlaceHolder = Localizer["Search for text"];

            if (model.Label != null && model.LabelLocalized)
            {
                model.Label = Localizer[$"{model.Label}"];
            }

            model.Items.ForEach((item) =>
            {
                if (item.Localized)
                {
                    item.Value = Localizer[$"{item.Value}"];
                }
            });
        }
    }
}

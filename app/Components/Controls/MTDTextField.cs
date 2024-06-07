using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using MtdKey.OrderMaker.Models.Controls.MTDTextField;
using System.Threading.Tasks;

namespace MtdKey.OrderMaker.Components.Controls
{
    [ViewComponent(Name = "MTDTextField")]
    public class MTDTextField(IStringLocalizer<SharedResource> localizer) : ViewComponent
    {
        private readonly IStringLocalizer<SharedResource> Localizer = localizer;

        public async Task<IViewComponentResult> InvokeAsync(MTDTextFieldTags tags)
        {
            MTDTextFieldTagsModel model = new(tags);

            LocalizerModel(model);

            string viewName = model.MTDTexFieldView.ToString();
            return await Task.Run(() => View(viewName, model));
        }


        private void LocalizerModel(MTDTextFieldTagsModel model)
        {
            if (model.Label != null && model.LabelLocalized)
            {
                model.Label = Localizer[$"{model.Label}"];
            }

            if (model.Placeholder != null && model.PlaceholderLocalized)
            {
                model.Placeholder = Localizer[$"{model.Placeholder}"];
            }

            if (model.HelperText != null && model.HelperTextLocalizer)
            {
                model.HelperText = Localizer[$"{model.HelperText}"];
            }

            if (model.HelperError != null && model.HelperErrorLocalizer)
            {
                model.HelperError = Localizer[$"{model.HelperError}"];
            }
        }
    }
}

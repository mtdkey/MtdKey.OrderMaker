using Microsoft.Extensions.Localization;

namespace MtdKey.OrderMaker
{
    public class FormBuilderLocalizer
    {
        private readonly IStringLocalizer _localizer;

        public FormBuilderLocalizer(IStringLocalizer<FormBuilderLocalizer> localizer)
        {
            _localizer = localizer;
        }

        public string this[string index]
        {
            get
            {
                return _localizer[index];
            }
        }
    }
}

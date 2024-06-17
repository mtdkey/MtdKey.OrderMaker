using Microsoft.Extensions.Localization;


namespace MtdKey.OrderMaker.Localizer
{
    public class AttachedFilesLocalizer(IStringLocalizer<AttachedFilesLocalizer> localizer)
    {
        private readonly IStringLocalizer _localizer = localizer;

        public string this[string index]
        {
            get
            {
                return _localizer[index];
            }
        }
    }
}

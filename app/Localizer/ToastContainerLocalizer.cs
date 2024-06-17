using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MtdKey.OrderMaker.Localizer
{
    public class ToastContainerLocalizer
    {
        private readonly IStringLocalizer _localizer;

        public ToastContainerLocalizer(IStringLocalizer<ToastContainerLocalizer> localizer)
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

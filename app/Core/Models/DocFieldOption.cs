using System.Collections.Generic;
using System.Linq;

namespace MtdKey.OrderMaker.Core
{
    public enum DocFieldOption
    {
        Readyonly, Required
    }

    public static class DocFieldOptionExtesions
    {
        public static bool IsRequired(this Dictionary<DocFieldOption, bool> options)
            =>  options.Any(x => x.Key == DocFieldOption.Required && x.Value is true);

        public static bool IsReadOnly(this Dictionary<DocFieldOption, bool> options)
            => options.Any(x => x.Key == DocFieldOption.Readyonly && x.Value is true);
    }
}

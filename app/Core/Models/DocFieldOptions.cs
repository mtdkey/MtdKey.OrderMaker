using System.Collections.Generic;
using System.Linq;

namespace MtdKey.OrderMaker.Core
{
    public enum DocFieldOptions
    {
        ReadyOnly, Required
    }

    public static class DocFieldOptionExtesions
    {
        public static bool IsRequired(this Dictionary<DocFieldOptions, bool> options)
            =>  options.Any(x => x.Key == DocFieldOptions.Required && x.Value is true);

        public static bool IsReadOnly(this Dictionary<DocFieldOptions, bool> options)
            => options.Any(x => x.Key == DocFieldOptions.ReadyOnly && x.Value is true);
    }
}

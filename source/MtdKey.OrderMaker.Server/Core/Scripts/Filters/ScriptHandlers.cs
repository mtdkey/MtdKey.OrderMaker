using MtdKey.OrderMaker.Core.Scripts.StoreIds;

namespace MtdKey.OrderMaker.Core.Scripts.Filters
{
    public class ScriptHandlers
    {
        public static FormIdFilter FormId => new();
        public static StoreIdFilter StoreId => new();
        public static SortingFilter Sorting => new();
        public static DateRangeFilter DateRange => new();
        public static SearchTextFilter SearchText => new();
        public static SearchNumberFilter SearchNumber => new();
        public static OwnerFilter Owner => new();
        public static FieldsFilter Fields => new();
        public static TypeRequestFilter TypeRequest => new();

    }
}

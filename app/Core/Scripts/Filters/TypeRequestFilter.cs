namespace MtdKey.OrderMaker.Core.Scripts.StoreIds
{
    public class TypeRequestFilter : FilterHandler
    {
        public override string ReplaceFilter(string script, FilterSQLparams filter)
        {
            if (filter.TypeRequest == TypeRequest.GetIds)
            {
                var limitStart = filter.Page == 1 ? 0 : filter.PageSize * (filter.Page - 1);
                var limitEnd = filter.PageSize;
                script = script.Replace("limit 0,10", $"limit {limitStart},{limitEnd}");

            }
            else
            {
                script = script.Replace("limit 0,10", " ");
                script = script.Replace("select StoreId from (", "select count(StoreId) from (");
            }

            return script;
        }
    }
}

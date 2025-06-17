namespace MtdKey.OrderMaker.Core.Scripts.StoreIds
{
    public class StoreIdFilter : FilterHandler
    {
        public override string ReplaceFilter(string script, FilterSQLparams filter)
        {
            if (filter.StoreId != null && filter.StoreId != string.Empty)
                script = script.Replace("/*and StoreId*/", $" and StoreId = '{filter.StoreId}'");

            return script;
        }
    }
}

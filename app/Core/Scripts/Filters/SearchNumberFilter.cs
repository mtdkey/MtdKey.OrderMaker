namespace MtdKey.OrderMaker.Core.Scripts.StoreIds
{
    public class SearchNumberFilter : FilterHandler
    {
        public override string ReplaceFilter(string script, FilterSQLparams filter)
        {
            if (filter.SearchNumber != string.Empty)
            {
                var searchNumber = filter.SearchNumber.Replace("'", string.Empty);
                if (searchNumber.Length > 10) searchNumber = searchNumber[..10];
                script = script.Replace("/*and store.sequence*/", $"and store.sequence = '{searchNumber}'");
            }

            return script;
        }
    }
}

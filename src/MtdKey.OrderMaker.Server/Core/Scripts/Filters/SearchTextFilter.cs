namespace MtdKey.OrderMaker.Core.Scripts.StoreIds
{
    public class SearchTextFilter : FilterHandler
    {
        public override string ReplaceFilter(string script, FilterSQLparams filter)
        {
            if (filter.SearchText != string.Empty)
            {
                var searchText = filter.SearchText.Replace("'", string.Empty);
                if (searchText.Length > 250) searchText = searchText[..250];
                script = script.Replace("Result like '%%'", $"Result like '%{searchText}%'");
            }

            return script;
        }
    }
}

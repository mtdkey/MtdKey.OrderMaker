

namespace MtdKey.OrderMaker.Core.Scripts
{
    public enum TypeRequest
    {
        GetRowCount,
        GetIds
    }

    internal static class SqlScript
    {

        public static string GetStoreIds(FilterSQLparams filterParams)
        {
            var storeIdsScript = new StoreIdsScript();
            var factory = new FilterScriptFactory(storeIdsScript, filterParams);

            if (filterParams.FormId == string.Empty)
                return factory.Script;

            return factory.BuildScript();
        }


    }
}

namespace MtdKey.OrderMaker.Core.Scripts.StoreIds
{
    public class SortingFilter : FilterHandler
    {
        public override string ReplaceFilter(string script, FilterSQLparams filter)
        {
            if (filter.SortByFieldId == string.Empty || filter.SortByFieldId == "date")
                script = script.Replace("/*order by store.timecr desc*/", $"order by store.timecr {filter.SortOrder}");

            if (filter.SortByFieldId.Contains("field-"))
                script = script.Replace("[FieldId]", filter.SortByFieldId
                    .Replace("field-", "")).Replace("/*order by indexSort*/", $"order by indexSort {filter.SortOrder}");

            if (filter.SortByFieldId.Contains("number"))
                script = script.Replace("/*order by sequence desc*/", $"order by sequence {filter.SortOrder}");

            if (filter.SortByFieldId.Contains("approval"))
            {
                script = script.Replace("/*left join mtd_store_approval*/",
                    " left join mtd_store_approval as approval on store.id = approval.id ");
                script = script.Replace("/*order by approval.md_approve_stage desc*/",
                    $" order by approval.complete  {filter.SortOrder} ");
            }

            return script;
        }
    }
}

namespace MtdKey.OrderMaker.Core.Scripts.StoreIds
{
    public class OwnerFilter : FilterHandler
    {

        public override string ReplaceFilter(string script, FilterSQLparams filter)
        {
            bool ownerRequest = filter.OwnerId != null && filter.OwnerId != string.Empty;
            if (ownerRequest)
            {
                script = script.Replace("/*inner join mtd_store_owner*/",
                    $"inner join mtd_store_owner as o on f.StoreId = o.id and o.user_id='{filter.OwnerId}'");
            }

            if (!ownerRequest && filter.UserInGroupIds?.Count > 0)
            {
                var userIds = string.Empty;
                filter.UserInGroupIds.ForEach(userId =>
                {
                    if (userIds != string.Empty) userIds += ",";
                    userIds += $"'{userId}'";
                });

                script = script.Replace("/*inner join mtd_store_owner*/",
                    $"inner join mtd_store_owner as o on f.StoreId = o.id and o.user_id in ({userIds})");
            }

            return script;
        }
    }
}

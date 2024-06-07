namespace MtdKey.OrderMaker.Core.Scripts.StoreIds
{
    public class FormIdFilter : FilterHandler
    {
        public override string ReplaceFilter(string script, FilterSQLparams filter)
        {
            return script.Replace("[FormId]", filter.FormId.ToString());
        }
    }
}

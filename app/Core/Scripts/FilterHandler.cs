namespace MtdKey.OrderMaker.Core.Scripts
{
    public abstract class FilterHandler
    {
        public abstract string ReplaceFilter(string script, FilterSQLparams filter);
    }
}

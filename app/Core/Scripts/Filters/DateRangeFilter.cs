using System;

namespace MtdKey.OrderMaker.Core.Scripts.StoreIds
{
    public class DateRangeFilter : FilterHandler
    {
        public override string ReplaceFilter(string script, FilterSQLparams filter)
        {
            if (filter.DateStart != DateTime.MinValue)
            {
                var dateStart = filter.DateStart.ToString("yyyy-MM-dd");
                var dateEnd = filter.DateEnd.ToString("yyyy-MM-dd");
                script = script.Replace("/*and store.timecr between*/",
                    $" and store.timecr between '{dateStart} 00:00:00' and '{dateEnd} 23:59:59' ");
            }

            return script;
        }
    }
}

using Microsoft.AspNetCore.Mvc.Formatters;
using MtdKey.OrderMaker.Core.Scripts.Filters;
using System.Collections.Generic;

namespace MtdKey.OrderMaker.Core.Scripts
{
    public class StoreIdsScript : IScriptFile
    {
       
        public string ResourceName => "MtdKey.OrderMaker.Core.Scripts.Filters.StoreIdsScript.sql";

        public IEnumerable<FilterHandler> FilterHandlers => new FilterBuilder()
            .AddHandler(ScriptHandlers.FormId)
            .AddHandler(ScriptHandlers.StoreId)
            .AddHandler(ScriptHandlers.Sorting)
            .AddHandler(ScriptHandlers.DateRange)
            .AddHandler(ScriptHandlers.SearchText)
            .AddHandler(ScriptHandlers.SearchNumber)
            .AddHandler(ScriptHandlers.Owner)
            .AddHandler(ScriptHandlers.Fields)
            .AddHandler(ScriptHandlers.TypeRequest)
            .Build();
    }
}

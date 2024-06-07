/*
    MTD OrderMaker - http://mtdkey.com
    Copyright (c) 2019 Oleg Bruev <job4bruev@gmail.com>. All rights reserved.
*/

using Microsoft.AspNetCore.Mvc;
using MtdKey.OrderMaker.Core;
using System.Threading.Tasks;

namespace MtdKey.OrderMaker.Components.Index
{
    [ViewComponent(Name = "IndexCell")]
    public class Cell : ViewComponent
    {

        public Task<IViewComponentResult> InvokeAsync(DocFieldModel field)
        {
            string viewName = GetViewName(field.Type);
            return Task.FromResult((IViewComponentResult)View(viewName, field));
        }

        private static string GetViewName(int idType) =>
           idType switch
           {
               2 => "Integer",
               3 => "Decimal",
               4 => "Memo",
               5 => "Date",
               6 => "DateTime",
               7 => "File",
               8 => "Picture",
               10 => "Time",
               11 => "List",
               12 => "CheckBox",
               13 => "Link",
               _ => "Text"
           };
    }
}

/*
    MTD OrderMaker - http://mtdkey.com
    Copyright (c) 2019 Oleg Bruev <job4bruev@gmail.com>. All rights reserved.
*/

using Microsoft.AspNetCore.Mvc;
using MtdKey.OrderMaker.Core;
using System.Threading.Tasks;

namespace MtdKey.OrderMaker.Components.Store.Stack
{
    [ViewComponent(Name = "StoreStackViewer")]
    public class Viewer : ViewComponent
    {
 

        public async Task<IViewComponentResult> InvokeAsync(DocPartModel docPart, DocFieldModel docField)
        {
            string viewName = await GetViewNameAsync(docField.Type);
            ViewData["typeStyle"] = docPart.Type == DocPartType.Columns ? "Columns" : "Rows";

            return View(viewName, docField);
        }

        private static string GetViewName(int type)
        {
            string viewName;
            switch (type)
            {
                case 2: { viewName = "Integer"; break; }
                case 3: { viewName = "Decimal"; break; }
                case 4: { viewName = "Memo"; break; }
                case 5: { viewName = "Date"; break; }
                case 6: { viewName = "DateTime"; break; }
                case 7: { viewName = "File"; break; }
                case 8: { viewName = "Picture"; break; }
                case 11: { viewName = "List"; break; }
                case 12: { viewName = "CheckBox"; break; }
                case 13: { viewName = "Link"; break; }
                default: { viewName = "Text"; break; }
            };

            return viewName;
        }

        private static async Task<string> GetViewNameAsync(int type)
        {
            return await Task.Run(() => GetViewName(type));
        }

    }
}

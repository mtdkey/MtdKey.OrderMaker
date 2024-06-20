/*
    MTD OrderMaker - http://mtdkey.com
    Copyright (c) 2019 Oleg Bruev <job4bruev@gmail.com>. All rights reserved.
*/

using Microsoft.AspNetCore.Mvc;
using MtdKey.OrderMaker.Core;
using System.Threading.Tasks;

namespace MtdKey.OrderMaker.Components.Store.Stack
{
    [ViewComponent(Name = "StoreStackEditor")]
    public class Editor : ViewComponent
    {

        public async Task<IViewComponentResult> InvokeAsync(DocPartModel docPart, DocFieldModel docField)
        {
            string viewName = await GetViewNameAsync(docField.Type, docPart.Type);
            ViewData["typeStyle"] = docPart.Type == DocPartType.Columns ? "Columns" : "Rows";

            return View(viewName, docField);
        }

        private static string GetViewName(int fileType, int partType)
        {
            string viewName;
            switch (fileType)
            {
                case 2: { viewName = "Integer"; break; }
                case 3: { viewName = "Decimal"; break; }
                case 4: { viewName = "Memo"; break; }
                case 5: { viewName = "Date"; break; }
                case 6: { viewName = "DateTime"; break; }
                case 7:
                case 8:
                    {
                        viewName = partType == DocPartType.Columns ? "FileColumn" : "FileRow";
                        break;
                    }
                case 11: { viewName = "List"; break; }
                case 12: { viewName = "CheckBox"; break; }
                case 13: { viewName = "Link"; break; }
                case 14: { viewName = "FileStorage"; break; }
                case 15: { viewName = "HTMLEditor"; break; }
                default: { viewName = "Text"; break; }
            };

            return viewName;
        }

        private static async Task<string> GetViewNameAsync(int type, int partType)
        {
            return await Task.Run(() => GetViewName(type, partType));
        }
    }
}

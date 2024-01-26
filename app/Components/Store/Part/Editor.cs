/*
    MTD OrderMaker - http://mtdkey.com
    Copyright (c) 2019 Oleg Bruev <job4bruev@gmail.com>. All rights reserved.
*/

using Microsoft.AspNetCore.Mvc;
using MtdKey.OrderMaker.Core;
using System.Linq;


namespace MtdKey.OrderMaker.Components.Store.Part
{
    [ViewComponent (Name = "StorePartEditor")]
    public class Editor : ViewComponent
    {
        public IViewComponentResult Invoke(DocModel docModel, DocPartModel partModel)
        {
            var doc = new DocModel
            {
                Id = docModel.Id,
                Created = docModel.Created,
                Sequence = docModel.Sequence,
                Fields = docModel.Fields.Where(x => x.PartId == partModel.Id).ToList(),
                Parts = new() { partModel },
            };

            string viewName = partModel.Type == DocPartType.Columns ? "Columns" : "Rows";
            return View(viewName, doc);
        }
    }
}

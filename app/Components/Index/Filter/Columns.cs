/*
    MTD OrderMaker - http://mtdkey.com
    Copyright (c) 2019 Oleg Bruev <job4bruev@gmail.com>. All rights reserved.
*/

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MtdKey.OrderMaker.Entity;
using MtdKey.OrderMaker.Models.Index;
using MtdKey.OrderMaker.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MtdKey.OrderMaker.Components.Index
{
    [ViewComponent(Name = "IndexFilterColumns")]
    public class Columns : ViewComponent
    {
        private readonly DataConnector _context;
        private readonly UserHandler _userHandler;

        public Columns(DataConnector context, UserHandler userHandler)
        {
            _context = context;
            _userHandler = userHandler;
        }

        public async Task<IViewComponentResult> InvokeAsync(string formId)
        {
            var user = await _userHandler.GetUserAsync(HttpContext.User);
            List<MtdFormPart> parts = await _userHandler.GetAllowPartsForView(user, formId);
            List<string> partIds = parts.Select(x => x.Id).ToList();
            MtdFilter mtdFilter = await _context.MtdFilter.Where(x => x.MtdFormId == formId && x.IdUser == user.Id).FirstOrDefaultAsync();
            bool showNumber = true;
            bool showDate = true;
            if (mtdFilter != null)
            {
                showNumber = mtdFilter.ShowNumber == 1;
                showDate = mtdFilter.ShowDate == 1;
            }
            
            IList<MtdFilterColumn> columns = await _context.MtdFilterColumn
                .Where(x => x.MtdFilter == mtdFilter.Id)
                .OrderBy(x => x.Sequence)
                .ToListAsync() ?? new List<MtdFilterColumn>();

            
            IList<MtdFormPartField> fields = await _context.MtdFormPartField
                .Where(x => partIds.Contains(x.MtdFormPartId))
                .OrderBy(o => o.Sequence)
                .ToListAsync() ?? new List<MtdFormPartField>();


            List<ColumnItem> columnItems = new();
            int i = columns.Count();
            foreach (var p in parts)
            {                
                fields.Where(x => x.MtdFormPartId == p.Id).ToList().ForEach((fs) =>
                {
                    i++;
                    MtdFilterColumn column = columns.Where(x => x.MtdFormPartFieldId == fs.Id).FirstOrDefault();
                    columnItems.Add(new ColumnItem
                    {
                        PartId = p.Id,
                        PartName = p.Name,
                        FieldId = fs.Id,
                        FieldName = fs.Name,
                        IsChecked = column != null,
                        Sequence = column != null ? column.Sequence : i,                        
                    });
                });
            }

            ColumnsModelView fieldsModelView = new()
            {
                FormId = formId,
                ColumnItems = columnItems.OrderBy(x=>x.Sequence).ToList(),
                ShowNumber = showNumber,
                ShowDate = showDate
            };

            return View(fieldsModelView);
        }
    }
}

/*
    MTD OrderMaker - http://mtdkey.com
    Copyright (c) 2019 Oleg Bruev <job4bruev@gmail.com>. All rights reserved.
*/

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using MtdKey.OrderMaker;
using MtdKey.OrderMaker.AppConfig;
using MtdKey.OrderMaker.Core;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Mtd.OrderMaker.Web.Controllers.Index
{

    [Route("api/action/index")]
    [ApiController]
    [Authorize(Roles = "Admin,User")]
    public class ActionController : ControllerBase
    {

        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly LimitSettings limit;
        private readonly IStoreService storeService;

        public ActionController(IStoreService storeService,
            IStringLocalizer<SharedResource> localizer, IOptions<LimitSettings> limit)
        {

            _localizer = localizer;
            this.limit = limit.Value;
            this.storeService = storeService;
        }

        [HttpPost("excel/export")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PostExportAsync()
        {
            if (!limit.ExportExcel) { return NotFound(); }

            var form = await Request.ReadFormAsync();
            string formId = form["form-id"];

            var requestResult = await storeService.GetDocsBySQLRequestAsync(new()
            {
                FormId = formId,
                UserPrincipal = User,
                LimitRequest = true,
                UseFilter = true
            });

            IWorkbook workbook = CreateWorkbook(requestResult.Docs);

            var ms = new NpoiMemoryStream
            {
                AllowClose = false
            };
            workbook.Write(ms, true);
            ms.Flush();
            ms.Seek(0, SeekOrigin.Begin);
            ms.AllowClose = true;

            return new FileStreamResult(ms, new MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"))
            {
                FileDownloadName = $"{DateTime.UtcNow:yyyyMMddHHmmss}.xlsx"
            };
        }

        private IWorkbook CreateWorkbook(List<DocModel> docs)
        {
            IWorkbook workbook = new XSSFWorkbook();

            ISheet sheet1 = workbook.CreateSheet("Sheet1");
            var rowIndex = 0;
            var colIndex = 0;
            IRow rowTitle = sheet1.CreateRow(rowIndex);
            rowTitle.CreateCell(0).SetCellValue("ID");
            sheet1.SetColumnWidth(colIndex, 2560);
            rowTitle.CreateCell(1).SetCellValue(_localizer["Date"]);
            colIndex++;
            sheet1.SetColumnWidth(colIndex, 2560);
            List<DocFieldModel> partFields = docs.FirstOrDefault()?.Fields ?? new();
            partFields = partFields
                .Where(x => x.Type != FieldType.Image && x.Type != FieldType.File)
                .ToList();
            foreach (var field in partFields)
            {
                colIndex++;
                var cell = rowTitle.CreateCell(colIndex);
                cell.SetCellValue(field.Name);

            }

            colIndex = 0;
            rowIndex++;
            foreach (var doc in docs)
            {
                IRow row = sheet1.CreateRow(rowIndex);
                row.CreateCell(colIndex).SetCellValue(doc.Sequence.ToString("D9"));
                colIndex++;
                row.CreateCell(colIndex).SetCellValue(doc.Created.ToShortDateString());
                var fields = doc.Fields.Where(x => x.Type != FieldType.Image && x.Type != FieldType.File).ToList();
                foreach (var field in fields)
                {
                    colIndex++;
                    ICell cell = row.CreateCell(colIndex);
                    SetValuefoCell(field, cell);
                }
                colIndex = 0;
                rowIndex++;
            }



            return workbook;
        }

        private static void SetValuefoCell(DocFieldModel field, ICell cell)
        {

            switch (field.Type)
            {
                case 2:
                    {
                        int result = field.Value != null ? (int)field.Value : 0;
                        cell.SetCellType(CellType.Numeric);
                        cell.SetCellValue(result);
                        break;
                    }
                case 3:
                    {
                        decimal result = field.Value != null ? (decimal)field.Value : 0;
                        cell.SetCellType(CellType.Numeric);
                        cell.SetCellValue((double)result);
                        break;
                    }
                case 5:
                    {

                        if (field.Value != null && (DateTime)field.Value > DateTime.MinValue)
                            cell.SetCellValue(((DateTime)field.Value).ToShortDateString());
                        else
                            cell.SetCellValue("");
                        break;
                    }
                case 6:
                    {
                        if (field.Value != null && (DateTime)field.Value > DateTime.MinValue)
                            cell.SetCellValue(((DateTime)field.Value).ToString("g"));
                        else
                            cell.SetCellValue("");
                        break;
                    }
                case 11:
                    {
                        string result = field.Value is not null and string ? (string)field.Value : "";
                        string itemName = field.ListItems
                            .Where(x => x.Id == result).Select(x => x.Name)
                            .FirstOrDefault() ?? string.Empty;
                        cell.SetCellType(CellType.String);
                        cell.SetCellValue(itemName);
                        break;
                    }

                case 12:
                    {
                        int result = field.Value != null && (int)field.Value == 1 ? 1 : 0;
                        cell.SetCellType(CellType.Boolean);
                        cell.SetCellValue(result);
                        break;
                    }
                default:
                    {
                        string result = field.Value is not null and string ? (string)field.Value : "";
                        cell.SetCellType(CellType.String);
                        cell.SetCellValue(result);
                        break;
                    }
            }

        }

    }


    public class NpoiMemoryStream : MemoryStream
    {
        public NpoiMemoryStream()
        {
            AllowClose = true;
        }

        public bool AllowClose { get; set; }

        public override void Close()
        {
            if (AllowClose)
                base.Close();
        }
    }
}
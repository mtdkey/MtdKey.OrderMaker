/*
    MTD OrderMaker - http://mtdkey.com
    Copyright (c) 2019 Oleg Bruev <job4bruev@gmail.com>. All rights reserved.
*/


using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MtdKey.OrderMaker.Areas.Identity.Data;
using MtdKey.OrderMaker.Entity;
using MtdKey.OrderMaker.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MtdKey.OrderMaker.Controllers.Index
{
    [Route("api/index")]
    [ApiController]
    [Authorize(Roles = "Admin,User")]
    public class DataController : ControllerBase
    {
        private readonly DataConnector _context;
        private readonly UserHandler _userHandler;

        public DataController(DataConnector context, UserHandler userHandler)
        {
            _context = context;
            _userHandler = userHandler;
        }

        [HttpPost("sort")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PostSortAsync()
        {
            var form = await Request.ReadFormAsync();
            string field = form["field"];
            string formId = form["formId"];
            string sortOrder = form["order"];

            WebAppUser user = await _userHandler.GetUserAsync(User);
            MtdFilter filter = await _context.MtdFilter.FirstOrDefaultAsync(x => x.IdUser == user.Id && x.MtdFormId == formId);
            filter.Sort = field;
            filter.SortOrder = sortOrder;
            await _context.SaveChangesAsync();

            return Ok();
        }


        [HttpPost("search/text")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PostSearchTextAsync()
        {
            string form = Request.Form["indexForm"];
            string value = Request.Form["search-text"];
            WebAppUser user = await _userHandler.GetUserAsync(User);
            MtdFilter filter = await _context.MtdFilter.FirstOrDefaultAsync(x => x.IdUser == user.Id && x.MtdFormId == form);
            bool old = true;
            if (filter == null)
            {
                old = false;
                filter = new MtdFilter { IdUser = user.Id, MtdFormId = form };
            }

            filter.SearchNumber = "";
            filter.SearchText = value;
            filter.Page = 1;

            if (old)
            {
                _context.MtdFilter.Update(filter);
            }
            else
            {
                await _context.MtdFilter.AddAsync(filter);
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex) { throw ex.InnerException; }


            return Ok();
        }

        [HttpPost("search/number")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PostSerarchIndexAsync()
        {
            string form = Request.Form["formId"];
            string value = Request.Form["searchNumber"];

            WebAppUser user = await _userHandler.GetUserAsync(User);
            MtdFilter filter = await _context.MtdFilter.FirstOrDefaultAsync(x => x.IdUser == user.Id & x.MtdFormId == form);
            bool old = true;
            if (filter == null)
            {
                old = false;
                filter = new MtdFilter { IdUser = user.Id, MtdFormId = form };
            }

            filter.SearchNumber = value;
            filter.Page = 1;
            filter.SearchText = "";

            if (old)
            {
                _context.MtdFilter.Update(filter);
            }
            else
            {
                await _context.MtdFilter.AddAsync(filter);
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex) { throw ex.InnerException; }
            return Ok();
        }

        [HttpPost("{formId}/pagesize/{number}")]
        public async Task<IActionResult> PostPageSize(string formId, int number)
        {
            int temp = number;
            if (temp > 50) temp = 50;
            var user = await _userHandler.GetUserAsync(User);
            MtdFilter filter = await _context.MtdFilter.FirstOrDefaultAsync(x => x.IdUser == user.Id && x.MtdFormId == formId);
            if (filter == null)
            {
                filter = new MtdFilter { SearchNumber = "", SearchText = "" };
                await _context.MtdFilter.AddAsync(filter);
                await _context.SaveChangesAsync();
            }

            filter.PageSize = number;
            _context.MtdFilter.Update(filter);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPost("pagemove")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PostPageMove()
        {
            /* number
             * 1 -  First Page; 2 - back; 3 - forward; 4 - Last Page 
             */

            string formId = Request.Form["formId"];
            string formValue = Request.Form["formValue"];
            string pageCount = Request.Form["pageCount"];

            int number = int.Parse(formValue);

            var user = await _userHandler.GetUserAsync(User);
            MtdFilter filter = await _context.MtdFilter.FirstOrDefaultAsync(x => x.IdUser == user.Id && x.MtdFormId == formId);
            if (filter == null)
            {
                filter = new MtdFilter { SearchNumber = "", SearchText = "" };
                await _context.MtdFilter.AddAsync(filter);
                await _context.SaveChangesAsync();
            }

            int page = filter.Page;
            bool isOk = int.TryParse(pageCount, out int pageLast);
            if (!isOk) { pageLast = page; }

            switch (number)
            {
                case 2: { if (page > 1) { page--; } break; }
                case 3: { page++; break; }
                case 4: { page = pageLast; break; }
                default: { page = 1; break; }
            };

            filter.Page = page < 0 ? page = 1 : page;

            _context.MtdFilter.Update(filter);
            await _context.SaveChangesAsync();
            return Ok();
        }


        [HttpPost("filter/remove")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PostFilterRemoveAsync()
        {
            var form = await Request.ReadFormAsync();
            string strID = form["idField"];

            if (strID.Contains("-field"))
            {
                strID = strID.Replace("-field", "");
                bool ok = int.TryParse(strID, out int idField);
                if (!ok) return Ok();
                MtdFilterField mtdFilterField = new() { Id = idField };
                try
                {
                    _context.MtdFilterField.Remove(mtdFilterField);
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex) { throw ex.InnerException; }
            }

            if (strID.Contains("-date"))
            {
                strID = strID.Replace("-date", "");
                bool ok = int.TryParse(strID, out int idFilter);
                if (!ok) return Ok();
                MtdFilterDate filterDate = new() { Id = idFilter };
                try
                {
                    _context.MtdFilterDate.Remove(filterDate);
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex) { throw ex.InnerException; }
            }

            if (strID.Contains("-owner"))
            {
                strID = strID.Replace("-owner", "");
                bool ok = int.TryParse(strID, out int filterId);
                if (!ok) return Ok();
                MtdFilterOwner filterOwner = new() { Id = filterId };
                try
                {
                    _context.MtdFilterOwner.Remove(filterOwner);
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex) { throw ex.InnerException; }
            }

            if (strID.Contains("-script"))
            {
                return BadRequest(new JsonResult("Error: Bad request."));
            }


            return Ok();

        }

        [HttpPost("filter/removeAll")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PostFilterRemoveAllAsync()
        {
            var form = await Request.ReadFormAsync();
            string strID = form["filter-id"];
            string formId = form["form-id"];
            bool isOk = int.TryParse(strID, out int filterId);
            if (!isOk) { return NotFound(); }

            IList<MtdFilterField> mtdFilterFields = await _context.MtdFilterField.Where(x => x.MtdFilter == filterId).ToListAsync();
            WebAppUser user = await _userHandler.GetUserAsync(HttpContext.User);
            IList<MtdFilterScript> filterScripts = await _userHandler.GetFilterScriptsAsync(user, formId);

            MtdFilterDate mtdFilterDate = await _context.MtdFilterDate.Where(x => x.Id == filterId).FirstOrDefaultAsync();
            if (mtdFilterDate != null)
            {
                _context.MtdFilterDate.Remove(mtdFilterDate);
            }

            MtdFilterOwner mtdFilterOwner = await _context.MtdFilterOwner.FindAsync(filterId);
            if (mtdFilterOwner != null)
            {
                _context.MtdFilterOwner.Remove(mtdFilterOwner);
            }

            try
            {
                _context.MtdFilterField.RemoveRange(mtdFilterFields);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                return BadRequest(new JsonResult(ex.Message));
            }


            return Ok();

        }

        [HttpPost("filter/columns/add")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PostFilterColumnsAsync()
        {

            string formId = Request.Form["form-id"];
            string data = Request.Form["indexDataColumnList"];
            string showNumber = Request.Form["indexDataColumnNumber"];
            string showDate = Request.Form["indexDataColumnDate"];

            List<string> fieldIds = new();
            if (data != null && data.Length > 0) fieldIds = data.Split(",").ToList();
            WebAppUser user = await _userHandler.GetUserAsync(User);
            MtdFilter filter = await _context.MtdFilter
                .FirstOrDefaultAsync(x => x.IdUser == user.Id & x.MtdFormId == formId);

            if (filter == null)
            {
                filter = new MtdFilter
                {
                    IdUser = user.Id,
                    MtdFormId = formId,
                    SearchNumber = "",
                    SearchText = "",
                    Page = 1,
                    PageSize = 10
                };
                await _context.MtdFilter.AddAsync(filter);
                await _context.SaveChangesAsync();
            }

            await _context.Entry(filter).Collection(x=>x.MtdFilterColumns).LoadAsync();

            List<MtdFilterColumn> columns = new();
            int seq = 0;
            foreach (string field in fieldIds.Where(x => x != ""))
            {
                seq++;
                columns.Add(new MtdFilterColumn
                {
                    MtdFilter = filter.Id,
                    MtdFormPartFieldId = field,
                    Sequence = seq
                });
            }


            try
            {
                filter.ShowNumber = showNumber == "true" ? (sbyte)1 : (sbyte)0;
                filter.ShowDate = showDate == "true" ? (sbyte)1 : (sbyte)0;

                _context.MtdFilter.Update(filter);

                if (filter.MtdFilterColumns != null)
                {
                    _context.MtdFilterColumn.RemoveRange(filter.MtdFilterColumns);
                    await _context.SaveChangesAsync();
                }

                await _context.MtdFilterColumn.AddRangeAsync(columns);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex) { throw ex.InnerException; }

            return Ok();
        }


        [HttpPost("waitlist/set")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PostWaitListSetAsync()
        {
            string formId = Request.Form["id-form-waitlist"];
            WebAppUser user = await _userHandler.GetUserAsync(HttpContext.User);
            MtdFilter mtdFilter = await _context.MtdFilter.Where(x => x.IdUser == user.Id && x.MtdFormId == formId).FirstOrDefaultAsync();
            if (mtdFilter == null)
            {
                mtdFilter = new MtdFilter
                {
                    IdUser = user.Id,
                    MtdFormId = formId,
                    PageSize = 10,
                    SearchText = "",
                    SearchNumber = "",
                    Page = 1,
                };
                await _context.MtdFilter.AddAsync(mtdFilter);
                await _context.SaveChangesAsync();
                return Ok();
            }

            mtdFilter.Page = 1;
            _context.MtdFilter.Update(mtdFilter);
            await _context.SaveChangesAsync();

            return Ok();

        }


    }
}

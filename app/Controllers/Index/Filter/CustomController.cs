using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using MtdKey.OrderMaker.Areas.Identity.Data;
using MtdKey.OrderMaker.Entity;
using MtdKey.OrderMaker.Services;
using Newtonsoft.Json;

namespace MtdKey.OrderMaker.Controllers.Index.Filter
{
    [Route("api/filter/custom")]
    [ApiController]
    [Authorize(Roles = "Admin,User")]
    public class CustomController : ControllerBase
    {
        private readonly DataConnector context;
        private readonly UserHandler userHandler;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public CustomController(DataConnector context, UserHandler userHandler, IStringLocalizer<SharedResource> localizer)
        {
            this.context = context;
            this.userHandler = userHandler;
            this._localizer = localizer;
        }

        [HttpPost("add")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PostFilterAddAsync()
        {
            var form = await Request.ReadFormAsync();

            string formId = form["form-id"];
            string fieldId = form["field-id"];
            string fieldType = form["field-type"];
            string fieldAction = form["field-action"];
            string fieldValue = form["field-value"];
            string fieldValueExt = form["field-value-ext"];
            string dateFormat = form["date-format"];

            MtdFilter filter = await userHandler.GetFilterAsync(User, formId);
            string valueExtra = dateFormat;


            bool isOk = int.TryParse (fieldAction, out int term);
            if (!isOk) { return BadRequest(_localizer["Error: Bad request."]); }

            if (fieldType == "5" || fieldType == "6") { fieldValue = $"{fieldValue}***{fieldValueExt}"; } else { valueExtra = null; }

            if (fieldType == "11")
            {
                var item = await context.MtdFormPartFieldItems.FirstOrDefaultAsync(x => x.Id.ToString().ToLower() == fieldValue.ToLower());
                if (item != null)
                {
                    fieldValue = item.Name;
                    valueExtra = item.Id.ToString();
                }
            }

            MtdFilterField field = new() { 
                MtdFilter = filter.Id, 
                MtdFormPartFieldId = fieldId, 
                MtdTerm = term, 
                Value = fieldValue, 
                ValueExtra = valueExtra 
            };

            try
            {
                await context.MtdFilterField.AddAsync(field);
                await context.SaveChangesAsync();
            }
            catch (Exception ex) { throw ex.InnerException; }



            return Ok();
        }


        [HttpPost("get/list")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PostFilterGetListAsync()
        {
            var form = await Request.ReadFormAsync();

            
            string fieldId = form["field-id"];
            var list  = await context.MtdFormPartFieldItems
                .Where(x => x.FieldId == fieldId && x.IsDeleted == false)
                .OrderBy(x=>x.Name)
                .ToListAsync();

            return new JsonResult(new { value = list });
        }

    }
}

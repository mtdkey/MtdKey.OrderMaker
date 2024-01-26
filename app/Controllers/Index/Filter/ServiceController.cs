using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using MtdKey.OrderMaker.Areas.Identity.Data;
using MtdKey.OrderMaker.Entity;
using MtdKey.OrderMaker.Services;

namespace MtdKey.OrderMaker.Controllers.Index.Filter
{
    [Route("api/filter/service")]
    [ApiController]
    [Authorize(Roles = "Admin,User")]
    public class ServiceController : ControllerBase
    {
        private readonly DataConnector context;
        private readonly UserHandler userHandler;

        public ServiceController(DataConnector context, UserHandler userHandler)
        {
            this.context = context;
            this.userHandler = userHandler;
        }

        [HttpPost("add/date")]
        [Produces("application/json")]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> AddDate()
        {
            var form = await Request.ReadFormAsync();

            string formId = form["form-id"];
            string dateStart = form["date-start"];
            string dateFinish = form["date-finish"];
            string dateFormat = form["date-format"];

            WebAppUser user = await userHandler.GetUserAsync(User);
            MtdFilter filter = await userHandler.GetFilterAsync(User, formId);

            bool isOkDateStart = DateTime.TryParseExact(dateStart, dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateTimeStart);
            bool isOkDateFinish = DateTime.TryParseExact(dateFinish, dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateTimeFinish);

            //bool isOkDateStart = DateTime.TryParse(dateStart, out DateTime dateTimeStart);
            //bool isOkDateFinish = DateTime.TryParse(dateFinish, out DateTime dateTimeFinish);

            if (isOkDateStart && isOkDateFinish)
            {
                MtdFilterDate mtdFilterDate = new()
                {
                    Id = filter.Id,
                    DateStart = dateTimeStart,
                    DateEnd = dateTimeFinish
                };

                bool isExists = await context.MtdFilterDate.Where(x => x.Id == filter.Id).AnyAsync();

                if (isExists)
                {
                    context.MtdFilterDate.Update(mtdFilterDate);
                }
                else
                {
                    await context.MtdFilterDate.AddAsync(mtdFilterDate);
                }

                await context.SaveChangesAsync();
            }

            return Ok();
        }


        [HttpPost("add/owner")]
        [Produces("application/json")]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> AddOwner()
        {
            var form = await Request.ReadFormAsync();
            string formId = form["form-id"];
            string ownerId = form["owner-id"];
            
            WebAppUser user = await userHandler.GetUserAsync(User);
            MtdFilter filter = await userHandler.GetFilterAsync(User, formId);

            MtdFilterOwner mtdFilterOwner = new()
            {
                Id = filter.Id,
                OwnerId = ownerId                 
            };

            bool isExists = await context.MtdFilterOwner.Where(x => x.Id == filter.Id).AnyAsync();

            if (isExists)
            {
                context.MtdFilterOwner.Update(mtdFilterOwner);
            }
            else
            {
                await context.MtdFilterOwner.AddAsync(mtdFilterOwner);
            }

            await context.SaveChangesAsync();

            return Ok();
        }
    }

}

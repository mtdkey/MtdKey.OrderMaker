/*
    MTD OrderMaker - http://mtdkey.com
    Copyright (c) 2019 Oleg Bruev <job4bruev@gmail.com>. All rights reserved.
*/

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MtdKey.OrderMaker.Areas.Identity.Data;
using MtdKey.OrderMaker.Core;
using MtdKey.OrderMaker.Entity;
using MtdKey.OrderMaker.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MtdKey.OrderMaker.Controllers.Store
{
    [Route("api/store")]
    [ApiController]
    [Authorize(Roles = "Admin,User")]
    public partial class DataController : ControllerBase
    {
        private readonly DataConnector _context;
        private readonly UserHandler _userHandler;
        private readonly IStoreService storeService;

        private enum TypeAction { Create, Edit };

        public DataController(DataConnector context, UserHandler userHandler, IStoreService storeService)
        {
            _context = context;
            _userHandler = userHandler;
            this.storeService = storeService;
        }

        [HttpPost("create")]
        [ValidateAntiForgeryToken]
        [Produces("application/json")]
        public async Task<IActionResult> OnPostCreateAsync()
        {
            var form = await Request.ReadFormAsync();

            StorePostRequest storeRequest = new()
            {
                FormId = form["form-id"],
                StoreId = form["store-id"],
                Fields = new Dictionary<string, string>(),
                DateCreated = form["date-create"],
                Files = form.Files,
                UserPrincipal = HttpContext.User,
                ActionType = ActionTypeRequest.Create,
            };

            form.Where(x => x.Key.Contains("field-")).ToList().ForEach(item =>
            {
                var itemKey = item.Key.Replace("field-", "");
                storeRequest.Fields.Add(itemKey, item.Value);
            });

            await storeService.CreateStoreAsync(storeRequest);

            return Ok();

        }

        // POST: api/store/save
        [HttpPost("save")]
        [ValidateAntiForgeryToken]
        [Produces("application/json")]
        //[DisableRequestSizeLimit]
        public async Task<IActionResult> OnPostSaveAsync()
        {
            var form = await Request.ReadFormAsync();
            StorePostRequest storeRequest = new()
            {
                FormId = form["formId"],
                StoreId = form["storeId"],
                Fields = new Dictionary<string, string>(),
                DateCreated = form["date-create"],
                Files = form.Files,
                UserPrincipal = HttpContext.User,
                ActionType = ActionTypeRequest.Edit,
            };

            form.Where(x => x.Key.Contains("field-")).ToList().ForEach(item =>
            {
                var itemKey = item.Key.Replace("field-", "");
                storeRequest.Fields.Add(itemKey, item.Value);
            });

            form.Where(x => x.Key.Contains("-delete")).ToList().ForEach(item =>
            {
                var itemKey = item.Key.Replace("-delete", "");
                storeRequest.DeleteFields.Add(itemKey, item.Value);
            });

            await storeService.SaveStoreAsync(storeRequest);
            return Ok();
        }

        [HttpPost("delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostDeleteAsync()
        {
            string idStore = Request.Form["store-delete-id"];
            MtdStore mtdStore = await _context.MtdStore.FindAsync(idStore);

            if (mtdStore == null)
            {
                return NotFound();
            }

            WebAppUser webAppUser = await _userHandler.GetUserAsync(HttpContext.User);
            bool isEraser = await _userHandler.IsEraser(webAppUser, mtdStore.MtdFormId, mtdStore.Id);

            if (!isEraser)
            {
                return Ok(403);
            }

            _context.MtdStore.Remove(mtdStore);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("setowner")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostSetOwnerAsync()
        {
            string idStore = Request.Form["setowner-id-store"];
            string idUser = Request.Form["setowner-id-user"];

            MtdStore mtdStore = await _context.MtdStore.FindAsync(idStore);
            if (mtdStore == null) { return Ok(403); }

            WebAppUser webAppUser = await _userHandler.FindByIdAsync(idUser);
            if (webAppUser == null) { return Ok(403); }

            WebAppUser currentUser = await _userHandler.GetUserAsync(HttpContext.User);
            bool isInstallerOwner = await _userHandler.IsInstallerOwner(currentUser, mtdStore.MtdFormId);

            if (!isInstallerOwner) { return Ok(403); }

            List<WebAppUser> webAppUsers = new();
            bool isViewAll = await _userHandler.CheckUserPolicyAsync(currentUser, mtdStore.MtdFormId, RightsType.ViewAll);
            if (isViewAll)
            {
                webAppUsers = await _userHandler.Users.Where(x => x.DatabaseId == currentUser.DatabaseId).ToListAsync();
            }
            else
            {
                webAppUsers = await _userHandler.GetUsersInGroupsAsync(currentUser);
            }

            if (!webAppUsers.Where(x => x.Id == idUser).Any()) { return Ok(403); }

            MtdStoreOwner mtdStoreOwner = await _context.MtdStoreOwner
                .FirstOrDefaultAsync(x => x.Id == idStore);

            mtdStoreOwner.UserId = webAppUser.Id;
            mtdStoreOwner.UserName = webAppUser.Title;
            _context.Entry(mtdStoreOwner).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok();
        }

    }

}

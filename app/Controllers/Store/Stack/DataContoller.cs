/*
    MTD OrderMaker - http://mtdkey.com
    Copyright (c) 2019 Oleg Bruev <job4bruev@gmail.com>. All rights reserved.
*/

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MtdKey.OrderMaker.Core;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MtdKey.OrderMaker.Controllers.Store.Stack
{
    [Route("api/store/stack")]
    [ApiController]
    [Authorize(Roles = "Admin,User")]
    public class DataContoller : ControllerBase
    {

        private readonly IStoreService storeService;

        public DataContoller(IStoreService storeService)
        {
            this.storeService = storeService;
        }

 
        [HttpGet("form/{formId}/file/{fieldId}/store/{storeId}")]
        public async Task<ActionResult> GetStackFile(string formId, string fieldId, string storeId)
        {            

            var requestResult = await storeService.GetDocsBySQLRequestAsync(new()
            {
                FormId = formId,
                StoreId = storeId,
                UserPrincipal = HttpContext.User,                
            });

            var doc = requestResult.Docs.FirstOrDefault();
            if (doc == null) { return NotFound(); }

            var field = doc.Fields.FirstOrDefault(x => x.Id == fieldId);
            if (field == null || field.Value == null) { return NotFound(); }

            return new FileStreamResult(new MemoryStream((byte[])field.Value), field.FileType)
            {
                FileDownloadName = field.FileName
            };

        }
    }
}

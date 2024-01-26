using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MtdKey.OrderMaker.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MtdKey.OrderMaker.Controllers
{
    [Route("images")]
    [ApiController]
    [AllowAnonymous]
    public class ImagesController : ControllerBase
    {
        public readonly DataConnector context;

        public ImagesController(DataConnector context)
        {    
            this.context = context;
        }

        [HttpGet("logo.png")]
        public async Task<IActionResult> OnGetLogoAsync()
        {
            byte[] fileData = await context.MtdConfigFiles.Where(x => x.Id == 1).Select(x => x.FileData).FirstOrDefaultAsync();
            return new FileContentResult(fileData, "image/png"); /// { FileDownloadName = "logo.png" };
        }

    }
}

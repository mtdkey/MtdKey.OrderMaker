using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MtdKey.OrderMaker.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MtdKey.OrderMaker.Controllers
{
    [Route("images")]
    [ApiController]
    [AllowAnonymous]
    public class ImagesController : ControllerBase
    {
        public readonly OrderMakerContext context;
        private readonly IWebHostEnvironment hostingEnvironment;
        public ImagesController(OrderMakerContext context, IWebHostEnvironment hostingEnvironment)
        {
            this.context = context;
            this.hostingEnvironment = hostingEnvironment;
            
        }

        [HttpGet("logo.png")]
        public async Task<IActionResult> OnGetLogoAsync()
        {
            
            byte[] fileData = await context.MtdConfigFiles.Where(x => x.Id == 1).Select(x => x.FileData).FirstOrDefaultAsync();
            if (fileData == null)
            {
                string contentRootPath = hostingEnvironment.ContentRootPath;
                var file = Path.Combine(contentRootPath, "wwwroot", "lib", "mtd-ordermaker", "images", "logo-mtd.png");
                fileData = System.IO.File.ReadAllBytes(file);
            }
            return new FileContentResult(fileData, "image/png"); /// { FileDownloadName = "logo.png" };
        }

    }
}

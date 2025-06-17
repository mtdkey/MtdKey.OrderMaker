using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MtdKey.OrderMaker.Components.AttachedFiles;
using MtdKey.OrderMaker.Entity;
using MtdKey.OrderMaker.Services;
using MtdKey.OrderMaker.Services.FileStorage;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MtdKey.OrderMaker.Controllers
{
    [Route("api/file")]
    [ApiController]
    [Authorize(Roles = "Admin,User")]
    public class FileStorageController(IFileStorageService storageService, UserHandler userHandler, OrderMakerContext context) : ControllerBase
    {
        private readonly IFileStorageService _fileStorageService = storageService;
        private readonly UserHandler _userHandler = userHandler;
        private readonly OrderMakerContext _context = context;
        

        [HttpGet("{fileId}")]
        public async Task<IActionResult> OnGetAsync(Guid fileId)
        {

            var fileLink = await _context.MtdStoreFileLinks.FirstOrDefaultAsync(x => x.Result == fileId);
            if (fileLink == null) { return NotFound(); }

            var partId = await _context.MtdFormPartField.Where(x=>x.Id  == fileLink.FieldId)
                .Select(x=>x.MtdFormPartId).FirstOrDefaultAsync();

            var user = await _userHandler.GetUserAsync(User);
            var accepted = await _userHandler.IsViewerPartAsync(user, partId);

            if(accepted is not true) { return NotFound(); }

            var fileData = await _fileStorageService.GetFileAsync(fileId);

            FileContentResult fileResult;
            if (OpenTypeFile.CheckType(fileLink.FileType))
                fileResult = File(fileData, fileLink.FileType);
            else
                fileResult = File(fileData, fileLink.FileType, fileLink.FileName);

            return fileResult;
        }
    }
}

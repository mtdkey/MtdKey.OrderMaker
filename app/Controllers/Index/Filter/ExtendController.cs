using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using MtdKey.OrderMaker.Services;

namespace MtdKey.OrderMaker.Controllers.Index.Filter
{
    [Route("api/filter/extension")]
    [ApiController]
    [Authorize(Roles = "Admin,User")]
    public class ExtendController : ControllerBase
    {

        private readonly UserHandler userHandler;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public ExtendController(UserHandler userHandler, IStringLocalizer<SharedResource> localizer)
        {

            this.userHandler = userHandler;
            this._localizer = localizer;
        }

        [HttpPost("add")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PostFilterScriptAsync()
        {
            var form = await Request.ReadFormAsync();
     
            string scriptId = form["script-id"];
            bool isOk = int.TryParse(scriptId, out int id);
            if (!isOk) { return BadRequest(_localizer["Error: Bad request."]); }
            
            bool available = await userHandler.IsFilterAccessingAsync(User, id);
            if (!available) { return BadRequest(_localizer["Error: Bad request."]); }

            return BadRequest(_localizer["Error: Bad request."]);


        }
    }
}

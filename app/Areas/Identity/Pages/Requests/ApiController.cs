using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MtdKey.OrderMaker.Core;
using MtdKey.OrderMaker.Services;
using MtdKey.OrderMaker.Services.Tokens;
using System;
using System.Threading.Tasks;

namespace MtdKey.OrderMaker.Areas.Identity.Pages.Requests
{
    [Route("api/requests")]
    [ApiController]
    public class ApiController(RegistrationService registrationService, ILogger<ApiController> logger) : ControllerBase
    {
        private readonly RegistrationService registrationService = registrationService;
        private readonly ILogger<ApiController> logger = logger;

        [HttpGet("create/form")]
        public async Task<IActionResult> RequestFormAsync(string userName, string email, string formId, string token)
        {

            if (await registrationService.APIRequestAccepting() is not true)
                return NotFound();

            try
            {
                var apiKey = await registrationService.GetAPITokenAsync();

                if (apiKey != token)
                    return BadRequest();

                var requestToken = new RequestFormToken()
                {
                    Email = email,
                    UserName = userName,
                    FormId = formId
                };

                var hostInfo = new HostInfo(Request.Scheme, Request.Host.Value);
                await registrationService.SendTokenByEmailAsync(requestToken, hostInfo);
            }
            catch (Exception ex)
            {
                logger.LogWarning("{message}", ex.Message);
                return BadRequest();
            }

            return new OkResult();

        }

    }
}

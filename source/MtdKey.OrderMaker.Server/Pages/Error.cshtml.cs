/*
    MTD OrderMaker - http://mtdkey.com
    Copyright (c) 2019 Oleg Bruev <job4bruev@gmail.com>. All rights reserved.

*/

using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using MtdKey.OrderMaker.AppConfig;
using MtdKey.OrderMaker.Services;
using System.Diagnostics;
using System.IO;

namespace MtdKey.OrderMaker.Pages
{
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public class ErrorModel : PageModel
    {

        private readonly IEmailSenderBlank _emailSender;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly ConfigSettings _emailSupport;

        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        public ErrorModel(
            IEmailSenderBlank emailSender,
            IWebHostEnvironment hostingEnvironment,
            IOptions<ConfigSettings> emailSupport)
        {

            _emailSender = emailSender;
            _hostingEnvironment = hostingEnvironment;
            _emailSupport = emailSupport.Value;
        }


        public async void OnGetAsync()
        {
            var exceptionFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            if (exceptionFeature != null)
            {

                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;

                string contentRootPath = _hostingEnvironment.ContentRootPath;
                var file = Path.Combine(contentRootPath, "wwwroot", "lib", "mtd-ordermaker", "emailform", "error.html");
                var htmlArray = System.IO.File.ReadAllText(file);
                string htmlText = htmlArray.ToString();

                htmlText = htmlText.Replace("{RequestID}", RequestId);
                htmlText = htmlText.Replace("{Host}", HttpContext.Request.Host.Value);
                htmlText = htmlText.Replace("{Path}", exceptionFeature.Path);
                htmlText = htmlText.Replace("{Query}", HttpContext.Request.QueryString.Value);
                htmlText = htmlText.Replace("{Message}", exceptionFeature.Error.Message);
                htmlText = htmlText.Replace("{Sorce}", exceptionFeature.Error.Source);
                htmlText = htmlText.Replace("{UserName}", User.Identity.Name);
                htmlText = htmlText.Replace("{StackTrace}", exceptionFeature.Error.StackTrace);

                await _emailSender.SendEmailAsync(_emailSupport.EmailSupport, "Server Error", htmlText, false);
            }

        }
    }
}

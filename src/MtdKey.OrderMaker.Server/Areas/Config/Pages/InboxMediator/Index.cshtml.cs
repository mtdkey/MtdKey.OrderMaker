using MailKit.Net.Imap;
using MailKit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MtdKey.InboxMediator.Data;
using MtdKey.OrderMaker.Entity;
using System;
using System.Linq;
using System.Threading.Tasks;
using MimeKit;
using MtdKey.InboxMediator.Service;

namespace MtdKey.OrderMaker.Areas.Config.Pages.InboxMediator
{
    public class IndexModel(IEmailMediatorService emailMediator) : PageModel
    {
        private readonly IEmailMediatorService emailMediator = emailMediator;

        public EmailMediator[] EmailMediators {  get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            EmailMediators = await emailMediator.GetAllMediatorsAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var form = await Request.ReadFormAsync();
            string email = form["email"];

            await emailMediator.RemoveMediatorAsync(email);

            return new OkResult();

        }

        public async Task<IActionResult> OnPostTestAsync(string email)
        {

            var mediator = await emailMediator.GetMediatorAsync(email);
            if (mediator == null) return BadRequest();

            try
            {
                using var client = new ImapClient();
                client.Connect(mediator.Server, mediator.Port, mediator.UseSSL);
                client.Authenticate(mediator.EmailAsKey, mediator.Password);
                
                // The Inbox folder is always available on all IMAP servers...
                var inbox = client.Inbox;
                inbox.Open(FolderAccess.ReadOnly);

                //var fetched = client.Inbox.Fetch(1200, -1, MessageSummaryItems.Headers);
                client.Disconnect(true);
            } catch (Exception ex) {
                return BadRequest(ex);
            }


            return new OkResult();
        }
    }
}

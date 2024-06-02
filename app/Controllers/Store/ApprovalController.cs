/*
    MTD OrderMaker - http://mtdkey.com
    Copyright (c) 2019 Oleg Bruev <job4bruev@gmail.com>. All rights reserved.
*/

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MtdKey.OrderMaker.Areas.Identity.Data;
using MtdKey.OrderMaker.Entity;
using MtdKey.OrderMaker.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using MtdKey.OrderMaker.Core;

namespace MtdKey.OrderMaker.Controllers.Store
{

    [Route("api/store/approval")]
    [ApiController]
    [Authorize(Roles = "Admin,User")]
    public class ApprovalController : ControllerBase
    {
        private readonly DataConnector _context;
        private readonly UserHandler _userHandler;
        private readonly IEmailSenderBlank _emailSender;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public ApprovalController(DataConnector context, UserHandler userHandler, IEmailSenderBlank emailSender, IStringLocalizer<SharedResource> sharedLocalizer)
        {
            _context = context;
            _userHandler = userHandler;
            _emailSender = emailSender;
            _localizer = sharedLocalizer;
        }

        [HttpPost("start")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostStartAsync()
        {
            string storeId = Request.Form["id-store"];
            string resolutionId = Request.Form["id-resolution"];
            string comment = Request.Form["comment-start-id"];
            if (comment.Length == 0) { comment = null; }
            WebAppUser webAppUser = await _userHandler.GetUserAsync(HttpContext.User);
            ApprovalHandler approvalHandler = new(_context, storeId);
            bool isApprover = await approvalHandler.IsApproverAsync(webAppUser);
            bool isFirstStage = await approvalHandler.IsFirstStageAsync();
            if (!isApprover || !isFirstStage) { return NotFound(); }

            MtdApprovalStage stageNext = await approvalHandler.GetNextStageAsync();
            bool sendEmail = stageNext != null && stageNext.UserId != webAppUser.Id;

            bool isOk = await approvalHandler.ActionApprove(webAppUser, resolutionId, comment);

            if (isOk && sendEmail)
            {
                await SendEmailStart(approvalHandler, comment);
            }
            return Ok();
        }

        [HttpPost("approve")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostApproveAsync()
        {
            string storeId = Request.Form["id-store"];
            string resolutionId = Request.Form["id-resolution"];
            string comment = Request.Form["comment-confirm-id"];
            if (comment.Length == 0) { comment = null; }

            WebAppUser webAppUser = await _userHandler.GetUserAsync(HttpContext.User);
            ApprovalHandler approvalHandler = new(_context, storeId);
            bool isApprover = await approvalHandler.IsApproverAsync(webAppUser);
            bool isSign = await approvalHandler.IsSignAsync();

            if (!isApprover || isSign) { return NotFound(); }

            bool isOk = await approvalHandler.ActionApprove(webAppUser, resolutionId, comment);
            if (isOk)
            {
                await SendEmailApprove(approvalHandler, comment);
            }
            return Ok();
        }

        [HttpPost("reject/{rtype}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostRejectAsync(int rtype = 0)
        {
            string storeId = Request.Form["id-store"];
            string rejectionId = Request.Form["id-rejection"];
            string comment = Request.Form["comment-reject-id"];
            if (comment.Length == 0) { comment = null; }
            if (rtype == 1) { rejectionId = null; }

            bool completeOk = bool.TryParse(Request.Form["checkbox-complete"], out bool completeCheck);
            bool stageOk = int.TryParse(Request.Form["next-stage"], out int stageId);
            if (!stageOk || !completeOk) { return NotFound(); }

            WebAppUser webAppUser = await _userHandler.GetUserAsync(HttpContext.User);
            ApprovalHandler approvalHandler = new(_context, storeId);

            bool isFirstStage = await approvalHandler.IsFirstStageAsync();
            bool isApprover = await approvalHandler.IsApproverAsync(webAppUser);
            if (!isApprover || isFirstStage) { return NotFound(); }

            bool isOk = await approvalHandler.ActionReject(completeCheck, stageId, webAppUser, rejectionId, comment);
            if (isOk)
            {
                await SendEmailReject(approvalHandler, comment);
            }
            return Ok();
        }

        [HttpPost("restart")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostNewAsync()
        {
            string storeId = Request.Form["id-store"];
            string formId = Request.Form["id-form"];

            WebAppUser webAppUser = await _userHandler.GetUserAsync(HttpContext.User);
            ApprovalHandler approvalHandler = new(_context, storeId);
            bool isReviewer = await _userHandler.IsReviewerAsync(webAppUser, formId);

            if (!isReviewer) { return NotFound(); }

            bool isOk = await approvalHandler.ActionApproveReset(webAppUser);
            if (isOk)
            {
                await SendEmailReStart(approvalHandler);
            }
            return Ok();
        }

        [HttpPost("request")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostRequestAsync()
        {
            string storeId = Request.Form["id-store"];
            string formId = Request.Form["id-form"];
            string recipient = Request.Form["id-user-recipient"];
            string comment = Request.Form["comment-request-id"];

            WebAppUser userSender = await _userHandler.GetUserAsync(HttpContext.User);
            WebAppUser userRecepient = await _userHandler.FindByIdAsync(recipient);

            ApprovalHandler approvalHandler = new(_context, storeId);
            bool isApprover = await approvalHandler.IsApproverAsync(userSender);
            bool isViewer = await _userHandler.IsViewer(userRecepient, formId, storeId);

            bool SendEmail = false;
            if (isViewer && isApprover)
            {
                SendEmail = await approvalHandler.ActionRequest(userSender, userRecepient, comment);
            }

            if (SendEmail)
            {
                await SendEmailRequest(approvalHandler, userRecepient, comment);
            }

            return Ok();
        }

        [HttpPost("response/{signtype}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostApproveSignAsync(int signtype = 0)
        {
            string storeId = Request.Form["id-store"];
            string comment = Request.Form["comment-request-id"];
            if (comment.Length == 0) { comment = null; }

            WebAppUser webAppUser = await _userHandler.GetUserAsync(HttpContext.User);
            ApprovalHandler approvalHandler = new(_context, storeId);
            bool isApprover = await approvalHandler.IsApproverAsync(webAppUser);
            bool isSign = await approvalHandler.IsSignAsync();

            if (!isApprover || !isSign) { return NotFound(); }

            bool isOk = await approvalHandler.ActionResponceSignAsync(webAppUser, comment, signtype);
            string recepientId = await approvalHandler.GetCurrentUserIdAsync();
            WebAppUser userRecipient = await _userHandler.FindByIdAsync(recepientId);


            if (isOk && userRecipient != null)
            {
                await SendEmailSignAsync(approvalHandler, userRecipient, comment);
            }

            return Ok();
        }

        [HttpPost("considered")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostApproveConsideredAsync()
        {
            var form = await Request.ReadFormAsync();
            string storeId = form["store-id"];
            string formId = form["form-id"];
            string resultValue = form["result-value"];
            string comment = form["comment-considered"];
            if (comment.Length == 0) { comment = null; }

            WebAppUser user = await _userHandler.GetUserAsync(HttpContext.User);
            ApprovalHandler approvalHandler = new(_context, storeId);
            
            bool isReviewer = await _userHandler.IsReviewerAsync(user, formId);
            bool isFormApproval = await approvalHandler.IsApprovalFormAsync();
            bool isComplete = await approvalHandler.IsComplete();

            if (!isReviewer || !isFormApproval || isComplete) { return BadRequest(_localizer["Error! Access denied."]); }


            int result = resultValue == "yes" ? 1 : -1;
            bool isOk = await approvalHandler.ActionConsiderApproved(user, result, comment);
            if (!isOk) { return BadRequest(_localizer["Error!"]); }

            if (result == 1)
            {
                await SendEmailApprove(approvalHandler, comment);
            } else
            {
                await SendEmailReject(approvalHandler, comment);
            }

            return Ok();
        }

        private async Task<bool> SendEmailStart(ApprovalHandler approvalHandler, string comment = null)
        {

            string ownerId = await approvalHandler.GetOwnerID();
            WebAppUser userCurrent = await _userHandler.GetUserAsync(HttpContext.User);
            WebAppUser userOwner = _userHandler.Users.Where(x => x.Id == ownerId).FirstOrDefault();
            string storeId = await approvalHandler.GetStoreID();
            MtdForm mtdForm = await approvalHandler.GetFormAsync();

            MtdApprovalStage stageNext = await approvalHandler.GetNextStageAsync();

            if (await approvalHandler.IsFirstStageAsync())
            {
                WebAppUser userNext = _userHandler.Users.Where(x => x.Id == stageNext.UserId).FirstOrDefault();
                BlankEmail blankEmail = new()
                {
                    Subject = _localizer["Approval event"],
                    Email = userNext.Email,
                    Header = _localizer["Approval required"],
                    Content = new List<string> {
                        $"<strong>{_localizer["Document"]} - {mtdForm.Name}</strong>",
                        $"{_localizer["User"]} {userCurrent.Title} {_localizer["started a new approval at"]} {DateTime.UtcNow}"
                        }
                };

                if (comment != null && comment.Length>0)
                {
                    blankEmail.Content.Add($"{ _localizer["User's comment"]}: <em>{comment}</em>");
                }

                blankEmail.Content.Add($"{_localizer["Click on the link to view the document that required to approve."]}");
                blankEmail.Content.Add($"<a href='{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/workplace/store/details?id={storeId}'>{_localizer["Document link"]}</a>");

                await _emailSender.SendEmailBlankAsync(blankEmail);
            }

            return true;
        }

        private async Task<bool> SendEmailReStart(ApprovalHandler approvalHandler, string comment = null)
        {

            string ownerId = await approvalHandler.GetOwnerID();
            WebAppUser userCurrent = await _userHandler.GetUserAsync(HttpContext.User);
            WebAppUser userOwner = _userHandler.Users.Where(x => x.Id == ownerId).FirstOrDefault();
            string storeId = await approvalHandler.GetStoreID();
            MtdForm mtdForm = await approvalHandler.GetFormAsync();

            MtdApprovalStage stageNext = await approvalHandler.GetNextStageAsync();

            BlankEmail blankEmail = new()
            {
                Subject = _localizer["Approval event"],
                Email = userOwner.Email,
                Header = _localizer["Approval process event"],
                Content = new List<string> {
                              $"<strong>{_localizer["Document"]} - {mtdForm.Name}</strong>",
                              $"{_localizer["User"]} {userCurrent.Title} {_localizer["restarted approval workflow at"]} {DateTime.UtcNow}"
                              }
            };

            if (comment != null && comment.Length > 0)
            {
                blankEmail.Content.Add($"{ _localizer["User's comment"]}: <em>{comment}</em>");
            }

            blankEmail.Content.Add($"{_localizer["Click on the link to view the document and start new approval."]}");
            blankEmail.Content.Add($"<a href='{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/workplace/store/details?id={storeId}'>{_localizer["Document link"]}</a>");
            await _emailSender.SendEmailBlankAsync(blankEmail);

            return true;
        }

        private async Task<bool> SendEmailApprove(ApprovalHandler approvalHandler, string comment = null)
        {

            string ownerId = await approvalHandler.GetOwnerID();
            WebAppUser userCurrent = await _userHandler.GetUserAsync(HttpContext.User);
            WebAppUser userOwner = _userHandler.Users.Where(x => x.Id == ownerId).FirstOrDefault();
            string storeId = await approvalHandler.GetStoreID();
            MtdForm mtdForm = await approvalHandler.GetFormAsync();

            MtdApprovalStage stageNext = await approvalHandler.GetNextStageAsync();

            if (stageNext != null)
            {
                WebAppUser userNext = _userHandler.Users.Where(x => x.Id == stageNext.UserId).FirstOrDefault();
                BlankEmail blankEmail = new()
                {
                    Subject = _localizer["Approval event"],
                    Email = userNext.Email,
                    Header = _localizer["Approval required"],
                    Content = new List<string> {
                        $"<strong>{_localizer["Document"]} - {mtdForm.Name}</strong>",
                        $"{_localizer["User"]} {userCurrent.Title} {_localizer["approved the document at"]} {DateTime.UtcNow}"
                        }
                };

                if (comment != null && comment.Length > 0)
                {
                    blankEmail.Content.Add($"{ _localizer["User's comment"]}: <em>{comment}</em>");
                }

                blankEmail.Content.Add($"{_localizer["Click on the link to view the document that required to approve."]}");
                blankEmail.Content.Add($"<a href='{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/workplace/store/details?id={storeId}'>{_localizer["Document link"]}</a>");

                await _emailSender.SendEmailBlankAsync(blankEmail);
            }
            else
            {
                BlankEmail blankEmail = new()
                {
                    Subject = _localizer["Approval event"],
                    Email = userOwner.Email,
                    Header = _localizer["Approval process event"],
                    Content = new List<string> {
                              $"<strong>{_localizer["Document"]} - {mtdForm.Name}</strong>",
                              $"{_localizer["User"]} {userCurrent.Title} {_localizer["approved the document at"]} {DateTime.UtcNow}"
                              }
                };

                if (comment != null && comment.Length > 0)
                {
                    blankEmail.Content.Add($"{ _localizer["User's comment"]}: <em>{comment}</em>");
                }

                blankEmail.Content.Add($"{_localizer["Approval process is complete. Click on the link to view the document."]}");
                blankEmail.Content.Add($"<a href='{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/workplace/store/details?id={storeId}'>{_localizer["Document link"]}</a>");

                await _emailSender.SendEmailBlankAsync(blankEmail);
            }
            return true;
        }

        private async Task<bool> SendEmailReject(ApprovalHandler approvalHandler, string comment = null)
        {
            string ownerId = await approvalHandler.GetOwnerID();
            WebAppUser userCurrent = await _userHandler.GetUserAsync(HttpContext.User);
            WebAppUser userOwner = _userHandler.Users.Where(x => x.Id == ownerId).FirstOrDefault();
            MtdForm mtdForm = await approvalHandler.GetFormAsync();
            string storeId = await approvalHandler.GetStoreID();

            MtdApprovalStage stagePrev = await approvalHandler.GetPrevStage();
            MtdApprovalStage stageFirst = await approvalHandler.GetFirstStageAsync();

            bool cacheReload = false;

            if (stagePrev != null)
            {
                WebAppUser user = userOwner;
                if (stagePrev.UserId != "owner")
                {
                    user = _userHandler.Users.Where(x => x.Id == stagePrev.UserId).FirstOrDefault();
                }

                BlankEmail blankEmail = new()
                {
                    Subject = _localizer["Approval event"],
                    Email = user.Email,
                    Header = _localizer["Approval required"],
                    Content = new List<string> {
                        $"<strong>{_localizer["Document"]} - {mtdForm.Name}</strong>",
                        $"{_localizer["User"]} {userCurrent.Title} {_localizer["rejected the document at"]} {DateTime.UtcNow}"
                        }
                };

                if (comment != null && comment.Length > 0)
                {
                    blankEmail.Content.Add($"{ _localizer["User's comment"]}: <em>{comment}</em>");
                }

                blankEmail.Content.Add($"{_localizer["Click on the link to view the document that required to approve."]}");
                blankEmail.Content.Add($"<a href='{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/workplace/store/details?id={storeId}'>{_localizer["Document link"]}</a>");

                approvalHandler.ClearCache();
                cacheReload = true;

                if (!await approvalHandler.IsComplete())
                {
                    await _emailSender.SendEmailBlankAsync(blankEmail);
                }

            }

            if (!cacheReload)
            {
                approvalHandler.ClearCache();
            }

            if (await approvalHandler.IsComplete())
            {
                BlankEmail blankOwner = new()
                {
                    Subject = _localizer["Approval event"],
                    Email = userOwner.Email,
                    Header = _localizer["Approval process event"],
                    Content = new List<string> {
                        $"<strong>{_localizer["Document"]} - {mtdForm.Name}</strong>",
                        $"{_localizer["User"]} {userCurrent.Title} {_localizer["rejected the document at"]} {DateTime.UtcNow}"
                        }
                };

                if (comment != null && comment.Length > 0)
                {
                    blankOwner.Content.Add($"{ _localizer["User's comment"]}: <em>{comment}</em>");
                }

                blankOwner.Content.Add($"{_localizer["Approval process is complete. Click on the link to view the document."]}");
                blankOwner.Content.Add($"<a href='{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/workplace/store/details?id={storeId}'>{_localizer["Document link"]}</a>");

                await _emailSender.SendEmailBlankAsync(blankOwner);
            }

            return true;
        }

        private async Task<bool> SendEmailRequest(ApprovalHandler approvalHandler, WebAppUser userRecipient, string comment = null)
        {

            WebAppUser userCurrent = await _userHandler.GetUserAsync(HttpContext.User);
            string storeId = await approvalHandler.GetStoreID();
            MtdForm mtdForm = await approvalHandler.GetFormAsync();

            BlankEmail blankEmail = new()
            {
                Subject = _localizer["Approval event"],
                Email = userRecipient.Email,
                Header = _localizer["Approval process event - signature request"],
                Content = new List<string> {
                              $"<strong>{_localizer["Document"]} - {mtdForm.Name}</strong>",
                              $"{_localizer["User"]} {userCurrent.Title} {_localizer["requested a signature to approve the document at"]} {DateTime.UtcNow}"
                              }
            };

            if (comment != null && comment.Length > 0)
            {
                blankEmail.Content.Add($"{ _localizer["User's comment"]}: <em>{comment}</em>");
            }

            blankEmail.Content.Add($"{_localizer["Click on the link to view the document."]}");
            blankEmail.Content.Add($"<a href='{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/workplace/store/details?id={storeId}'>{_localizer["Document link"]}</a>");
            await _emailSender.SendEmailBlankAsync(blankEmail);

            return true;
        }

        private async Task<bool> SendEmailSignAsync(ApprovalHandler approvalHandler, WebAppUser userRecipient, string comment = null)
        {

            WebAppUser userCurrent = await _userHandler.GetUserAsync(HttpContext.User);
            string storeId = await approvalHandler.GetStoreID();
            MtdForm mtdForm = await approvalHandler.GetFormAsync();

            BlankEmail blankEmail = new()
            {
                Subject = _localizer["Approval event"],
                Email = userRecipient.Email,
                Header = _localizer["Approval process event - answer to request"],
                Content = new List<string> {
                              $"<strong>{_localizer["Document"]} - {mtdForm.Name}</strong>",
                              $"{_localizer["User"]} {userCurrent.Title} {_localizer["answered to request signature document at"]} {DateTime.UtcNow}"
                              }
            };

            if (comment != null && comment.Length > 0)
            {
                blankEmail.Content.Add($"{ _localizer["User's comment"]}: <em>{comment}</em>");
            }

            blankEmail.Content.Add($"{_localizer["Click on the link to view the document."]}");
            blankEmail.Content.Add($"<a href='{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/workplace/store/details?id={storeId}'>{_localizer["Document link"]}</a>");

            await _emailSender.SendEmailBlankAsync(blankEmail);

            return true;
        }

    }
}

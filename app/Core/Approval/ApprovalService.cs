using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MtdKey.OrderMaker.Areas.Identity.Data;
using MtdKey.OrderMaker.Entity;
using MtdKey.OrderMaker.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MtdKey.OrderMaker.Core.Approval
{
    public class ApprovalService(ILogger<ApprovalService> logger, OrderMakerContext dataContext, IdentityDbContext userContext) : IApprovalService
    {
        private readonly ILogger<ApprovalService> logger = logger;
        private readonly OrderMakerContext dataContext = dataContext;
        private readonly IdentityDbContext userContext = userContext;

        public async Task<bool> StartApprovalChainAsync(Guid docId)
        {
            var formId = await dataContext.MtdStore.Where(x=>x.Id == docId.ToString()).Select(x=>x.MtdFormId).FirstOrDefaultAsync();
            if (formId == null || !await dataContext.MtdApproval.AnyAsync(x => x.MtdForm == formId)) return false;

            var docOwnerUserId = await dataContext.MtdStoreOwner
                .Where(x => x.Id == docId.ToString())
                .Select(x => x.UserId)
                .FirstOrDefaultAsync();

            var docOwnerUser = await userContext.Users.FirstOrDefaultAsync(x => x.Id == docOwnerUserId);
            var approval = await dataContext.MtdApproval
                .Include(x => x.MtdApprovalStages)
                .ThenInclude(x => x.MtdApprovalResolution)
                .AsSplitQuery()
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.MtdForm == formId);



            MtdApprovalStage currentStage = approval.MtdApprovalStages.FirstOrDefault(x => x.Stage == 1);
            MtdApprovalStage nextStage = approval.MtdApprovalStages.FirstOrDefault(x => x.Stage == 2);
            MtdApprovalResolution resolution = currentStage.MtdApprovalResolution.OrderBy(x => x.Sequence).FirstOrDefault();

            MtdStoreApproval storeApproval = new()
            {
                Id = docId.ToString(),
                MtdApproveStage = nextStage.Id,
                PartsApproved = currentStage.BlockParts,
                Complete = 0,
                Result = 1,
                LastEventTime = DateTime.UtcNow,
            };

            await dataContext.MtdStoreApproval.AddAsync(storeApproval);


            MtdLogApproval mtdLogApproval = new()
            {
                MtdStore = docId.ToString(),
                Result = 1,
                Stage = currentStage.Id,
                Timecr = DateTime.UtcNow,
                UserId = docOwnerUser.Id,
                UserName = docOwnerUser.GetFullName(),
                Comment = string.Empty,
            };

            if (resolution != null)
            {
                mtdLogApproval.ImgData = resolution.ImgData;
                mtdLogApproval.ImgType = resolution.ImgType;
                mtdLogApproval.Note = resolution.Name;
                mtdLogApproval.Color = resolution.Color;
            }

            await dataContext.MtdLogApproval.AddAsync(mtdLogApproval);
            await dataContext.SaveChangesAsync();


            ///ADD TASK TO SEND EMAIL

            return true;
        }

        //private async Task<bool> SendEmailForUser(string UserId)
        //{

        //    string storeId = await approvalHandler.GetStoreID();
        //    MtdForm mtdForm = await approvalHandler.GetFormAsync();

        //    MtdApprovalStage stageNext = await approvalHandler.GetNextStageAsync();

        //    if (await approvalHandler.IsFirstStageAsync())
        //    {
        //        WebAppUser userNext = userHandler.Users.Where(x => x.Id == stageNext.UserId).FirstOrDefault();
        //        BlankEmail blankEmail = new()
        //        {
        //            Subject = localizer["Approval event"],
        //            Email = userNext.Email,
        //            Header = localizer["Approval required"],
        //            Content = new List<string> {
        //                $"<strong>{localizer["Document"]} - {mtdForm.Name}</strong>",
        //                $"{localizer["User"]} {currentUser.Title} {localizer["started a new approval at"]} {DateTime.UtcNow}"
        //                }
        //        };

        //        if (comment != null && comment.Length > 0)
        //        {
        //            blankEmail.Content.Add($"{localizer["User's comment"]}: <em>{comment}</em>");
        //        }

        //        blankEmail.Content.Add($"{localizer["Click on the link to view the document that required to approve."]}");
        //        blankEmail.Content.Add($"<a href='{httpContextAccessor.HttpContext.Request.Scheme}://" +
        //            $"{httpContextAccessor.HttpContext.Request.Host}/workplace/store/details?id={storeId}'>" +
        //            $"{localizer["Document link"]}</a>");

        //        await emailSender.SendEmailBlankAsync(blankEmail);
        //    }

        //    return true;
        //}
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MtdKey.OrderMaker.Areas.Identity.Data;
using MtdKey.OrderMaker.Entity;
using MtdKey.OrderMaker.Services;
using MtdKey.OrderMaker.Services.EmailService;
using MtdKey.OrderMaker.Services.EmailService.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MtdKey.OrderMaker.Core.Approval
{
    public class ApprovalService(ILogger<ApprovalService> logger, OrderMakerContext dataContext, 
        IdentityDbContext userContext, IEmailService emailService) : IApprovalService
    {
        private readonly ILogger<ApprovalService> logger = logger;
        private readonly OrderMakerContext dataContext = dataContext;
        private readonly IdentityDbContext userContext = userContext;
        private readonly IEmailService emailService = emailService;

        public async Task<bool> StartApprovalChainAsync(Guid docId)
        {
            var mtdStore = await dataContext.MtdStore.Where(x => x.Id == docId.ToString()).FirstOrDefaultAsync();
           
            if (mtdStore == null || !await dataContext.MtdApproval.AnyAsync(x => x.MtdForm == mtdStore.MtdFormId)) return false;
            var formId = mtdStore.MtdFormId;


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


            var recepient = await userContext.Users.FirstOrDefaultAsync(u => u.Id == nextStage.UserId);
            if (recepient == null) return true;

            var formName = await dataContext.MtdForm.Where(x=>x.Id == formId).Select(x=>x.Name).FirstOrDefaultAsync();
            await emailService.AddEmailTaskAsync(recepient.Email, new StartApprovalDesigner
            {
                DocId = docId,
                FormId = Guid.Parse(formId),
                FormName = formName,
                Created = mtdStore.Timecr,
                Number = mtdStore.Sequence.ToString("D9"),
            });

            return true;
        }
    }
}

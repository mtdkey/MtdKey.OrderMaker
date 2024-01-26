using MtdKey.OrderMaker.Entity;
using System.Threading.Tasks;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace MtdKey.OrderMaker.Core
{
    public partial class StoreService : IStoreService
    {
        public async Task CreateStoreAsync(StorePostRequest storeRequest)
        {
            var mtdStore = await AddMtdStoreAsync(storeRequest);
            
            await AddStoreItemsAsync(storeRequest, mtdStore);
            await context.SaveChangesAsync();

            var approvalHandler = new ApprovalHandler(context, mtdStore.Id);
            if (await approvalHandler.IsApprovalFormAsync())
            {
                var firstStage = await approvalHandler.GetFirstStageAsync();
                MtdStoreApproval storeApproval = new()
                {
                    Id = mtdStore.Id,
                    MtdApproveStage = firstStage.Id,
                    Complete = 0,
                    LastEventTime = DateTime.UtcNow,
                    PartsApproved = string.Empty,
                    Result = 0,
                };
                
                await context.MtdStoreApproval.AddAsync(storeApproval);
                await context.SaveChangesAsync();
            }
        }

        private async Task<MtdStore> AddMtdStoreAsync(StorePostRequest storeRequest)
        {
            var user = await userHandler.GetUserAsync(storeRequest.UserPrincipal);
            MtdStore mtdStore = new()
            {
                Id = storeRequest.StoreId,
                MtdFormId = storeRequest.FormId,
                Timecr = DateTime.UtcNow,
                MtdStoreOwner = new MtdStoreOwner
                {
                    UserId = user.Id,
                    UserName = user.GetFullName(),
                }
            };

            if (DateTime.TryParse(storeRequest.DateCreated, out DateTime dateTime))
            {
                mtdStore.Timecr = dateTime;
            }

            int sequence = await context.MtdStore
                .Where(x => x.MtdFormId == storeRequest.FormId)
                .MaxAsync(x => (int?)x.Sequence) ?? 0;            
            mtdStore.Sequence = sequence + 1;

            await context.AddAsync(mtdStore);

            return mtdStore;
        }

    }
}

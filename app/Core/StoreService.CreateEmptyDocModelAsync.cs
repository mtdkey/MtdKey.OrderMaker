using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MtdKey.OrderMaker.Core
{
    public partial class StoreService : IStoreService
    {
        public async Task<DocModel> CreateEmptyDocModelAsync(StoreDocRequest request)
        {
            var appUser = await userHandler.GetUserAsync(request.UserPrincipal);
            var parts = await GetAllowedPartsAsync(request);
            var partIds = parts.Select(x=>x.Id).ToList();
            List<DocFieldModel> docFields = await GetDocFields(request.FormId, appUser, partIds);
            var form = await context.MtdForm.Include(x => x.MtdFormHeader).AsSplitQuery()
                .FirstOrDefaultAsync(x => x.Id == request.FormId);

            DocModel docModel = new() {
                Id = request.StoreId,
                FormName = form.Name,
                Image = form.MtdFormHeader?.Image,
                FormId = request.FormId,
                Parts = parts,
                Fields = docFields,
                Created = DateTime.UtcNow,
                EditDate = await userHandler.IsReviewerAsync(appUser, request.FormId),                
            };

            return docModel;
        }
    }
}

using Microsoft.Extensions.Options;
using MtdKey.OrderMaker.AppConfig;
using MtdKey.OrderMaker.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MtdKey.OrderMaker.Core
{
    public partial class StoreService : IStoreService
    {
        private readonly DataConnector context;
        private readonly UserHandler userHandler;
        private readonly LimitSettings limitSettings;

        public StoreService(DataConnector context, UserHandler userHandler, IOptions<LimitSettings> limitSettings)
        {
            this.context = context;
            this.userHandler = userHandler;
            this.limitSettings = limitSettings.Value;
        }


        public async Task<AllowedData> SecurityHandlerAsync(StoreDocRequest docRequest)
        {

            var appUser = await userHandler.GetUserAsync(docRequest.UserPrincipal);

            if (docRequest.ActionTypeRequest == ActionTypeRequest.Show)
            {
                if (await userHandler.IsViewer(appUser, docRequest.FormId) is not true)
                { return new(); }

                if (docRequest.StoreId != null && docRequest.StoreId != string.Empty
                    && !await userHandler.IsViewer(appUser, docRequest.FormId, docRequest.StoreId))
                { return new(); }
            }

            if (docRequest.ActionTypeRequest == ActionTypeRequest.Edit)
            {
                if (docRequest.StoreId != null && docRequest.StoreId != string.Empty
                    && !await userHandler.IsEditor(appUser, docRequest.FormId, docRequest.StoreId))
                { return new(); }
            }

            var docParts = await GetAllowedPartsAsync(docRequest);

            var partIds = docParts.Select(x => x.Id).ToList();
            List<DocFieldModel> docFields = await GetDocFields(docRequest.FormId, appUser, partIds);

            AllowedData allowedData = new()
            {
                DocFields = docFields,
                DocParts = docParts
            };

            var userInGroup = await userHandler
                .CheckUserPolicyAsync(appUser, docRequest.FormId, Services.RightsType.ViewGroup);

            if (userInGroup)
            {
                var users = await userHandler.GetUsersInGroupsAsync(appUser);
                allowedData.UsersInGroupIds = users.Select(x => x.Id).ToList();
            }

            var viewOwn = await userHandler
              .CheckUserPolicyAsync(appUser, docRequest.FormId, Services.RightsType.ViewOwn);


            if (viewOwn)
            {
                allowedData.UsersInGroupIds = new() { appUser.Id };
            }

            return allowedData;
        }

        public async Task<List<DocPartModel>> GetAllowedPartsAsync(StoreDocRequest request)
        {
            var appUser = await userHandler.GetUserAsync(request.UserPrincipal);
            /*check access*/
            var formParts = await userHandler.GetAllowPartsForView(appUser, request.FormId);

            /*reject approval stage parts*/
            var revisor = await userHandler.IsReviewerAsync(appUser, request.FormId);
            if (request.StoreId != null
                && request.StoreId != string.Empty
                && !revisor && request.ActionTypeRequest == ActionTypeRequest.Edit)
            {
                ApprovalHandler approvalHandler = new(context, request.StoreId);
                var ids = await approvalHandler.GetBlockedPartsIdsAsync();
                formParts = formParts.Where(x => !ids.Contains(x.Id)).ToList();
            }

            List<string> partIds = formParts.Select(x => x.Id).ToList();

            if (request.ActionTypeRequest == ActionTypeRequest.Edit)
            {
                partIds = new();
                foreach (var part in formParts)
                    if (await userHandler.IsEditorPartAsync(appUser, part.Id))
                        partIds.Add(part.Id);
            }

            if (request.ActionTypeRequest == ActionTypeRequest.Create)
            {
                partIds = new();
                foreach (var part in formParts)
                    if (await userHandler.IsCreatorPartAsync(appUser, part.Id))
                        partIds.Add(part.Id);
            }


            var docParts = formParts.Where(x => partIds.Contains(x.Id))
                .Select(x => new DocPartModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Image = x.MtdFormPartHeader?.Image,
                    Sequence = x.Sequence,
                    ShowTitle = x.Title == 1,
                    Type = x.MtdSysStyle,
                }).ToList();

            return docParts;
        }


        private static void MarkDeletedStoreItems<T>(ICollection<T> items, string fieldId) where T : IStoreField
        {
            var oldItems = items?.Where(x => x.FieldId == fieldId).ToList();
            if (oldItems.Any())
                oldItems.ForEach(item => item.IsDeleted = true);
        }
    }
}

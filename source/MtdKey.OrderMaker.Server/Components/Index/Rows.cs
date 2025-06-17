/*
    MTD OrderMaker - http://mtdkey.com
    Copyright (c) 2019 Oleg Bruev <job4bruev@gmail.com>. All rights reserved.
*/

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MtdKey.OrderMaker.Core;
using MtdKey.OrderMaker.Entity;
using MtdKey.OrderMaker.Models.Index;
using MtdKey.OrderMaker.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MtdKey.OrderMaker.Components.Index
{
    [ViewComponent(Name = "IndexRows")]
    public class Rows : ViewComponent
    {
        private readonly OrderMakerContext _context;
        private readonly UserHandler _userHandler;
        private readonly IStoreService storeService;


        public Rows(OrderMakerContext context, UserHandler userHandler, IStoreService storeService)
        {
            _context = context;
            _userHandler = userHandler;
            this.storeService = storeService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string formId)
        {
            var user = await _userHandler.GetUserAsync(HttpContext.User);

            StoreDocRequest docRequest = new()
            {
                FormId = formId,
                UseFilter = true,
                UserPrincipal = HttpContext.User
            };


            var requestResult = await storeService.GetDocsBySQLRequestAsync(docRequest);

            var storeIds = requestResult.Docs.Select(x => x.Id).ToList();

            IList<MtdStoreApproval> mtdStoreApprovals = await _context.MtdStoreApproval.Where(x => storeIds.Contains(x.Id)).ToListAsync();

            List<ApprovalStore> approvalStores = await ApprovalHandler.GetStoreStatusAsync(_context, storeIds, user);
            MtdApproval mtdApproval = await _context.MtdApproval.Where(x => x.MtdForm == formId).FirstOrDefaultAsync();
            MtdFilter filter = await _context.MtdFilter.FirstOrDefaultAsync(x => x.IdUser == user.Id && x.MtdFormId == formId);


            RowsModelView rowsModel = new()
            {
                FormId = formId,
                SearchNumber = filter.SearchNumber ?? string.Empty,
                PageCount = requestResult.PageCount,
                ShowDate = filter.ShowDate == 1,
                ShowNumber = filter.ShowNumber == 1,
                ApprovalStores = approvalStores,
                MtdApproval = mtdApproval,
                SearchText = filter == null ? string.Empty : filter.SearchText,
                IsCreator = await _userHandler.IsCreator(user, formId),
                PageSize = filter.PageSize,
                PageCurrent = filter.Page,
                DocList = requestResult.Docs,
                FieldsCount = requestResult.Docs.FirstOrDefault()?.Fields.Count ?? 0,
            };

            return View("Default", rowsModel);
        }
    }
}

/*
    MTD OrderMaker - http://mtdkey.com
    Copyright (c) 2019 Oleg Bruev <job4bruev@gmail.com>. All rights reserved.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MtdKey.OrderMaker.Areas.Identity.Data;
using MtdKey.OrderMaker.Areas.Workplace.Pages.Store.Models;
using MtdKey.OrderMaker.Core;
using MtdKey.OrderMaker.Entity;
using MtdKey.OrderMaker.Models.Controls.MTDSelectList;
using MtdKey.OrderMaker.Models.LogDocument;
using MtdKey.OrderMaker.Services;

namespace MtdKey.OrderMaker.Areas.Workplace.Pages.Store
{
    public class DetailsModel : PageModel
    {
        private readonly DataConnector _context;
        private readonly UserHandler _userHandler;

        public DetailsModel(DataConnector context, UserHandler userHandler)
        {
            _context = context;
            _userHandler = userHandler;
        }

        public MtdStore MtdStore { get; set; }
        public MtdForm MtdForm { get; set; }
        public ChangesHistory ChangesHistory { get; set; }
        public MtdStoreOwner StoreOwner { get; set; }
        public WebAppUser UserOwner { get; set; }
        public bool IsInstallerOwner { get; set; }
        public bool IsEditor { get; set; }
        public bool IsEraser { get; set; }
        public bool IsApprover { get; set; }
        public bool IsReviewer { get; set; }
        public bool IsFirstStage { get; set; }
        public bool IsSign { get; set; }
        public ApprovalStatus ApprovalStatus { get; set; }
        public List<MtdFormPart> BlockParts { get; set; }
        public bool IsFormApproval { get; set; }
        public MtdApproval MtdApproval { get; set; }

        public List<ApprovalLog> ApprovalHistory { get; set; }

        public List<MTDSelectListItem> ListResolutions { get; set; }
        public List<MTDSelectListItem> ListRejections { get; set; }

        public List<MTDSelectListItem> UsersList { get; set; }

        public List<MTDSelectListItem> Stages { get; set; }
        public List<MTDSelectListItem> UsersRequest { get; set; }

        public WebAppUser CurrentUser { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null) { return NotFound(); }

            MtdStore = await _context.MtdStore.FirstOrDefaultAsync(m => m.Id == id);

            if (MtdStore == null) { return NotFound(); }
            
            await _context.Entry(MtdStore).Reference(x => x.MtdFormNavigation).LoadAsync();
            await _context.Entry(MtdStore.MtdFormNavigation).Reference(x => x.MtdFormHeader).LoadAsync();

            var user = await _userHandler.GetUserAsync(HttpContext.User);
            bool isViewer = await _userHandler.IsViewer(user, MtdStore.MtdFormId, MtdStore.Id);

            if (!isViewer)
            {
                return NotFound();
            }

            CurrentUser = user;
            IsEditor = await _userHandler.IsEditor(user, MtdStore.MtdFormId, MtdStore.Id);
            IsInstallerOwner = await _userHandler.IsInstallerOwner(user, MtdStore.MtdFormId);
            IsEraser = await _userHandler.IsEraser(user, MtdStore.MtdFormId, MtdStore.Id);

            MtdForm = await _context.MtdForm.FirstOrDefaultAsync(x => x.Id == MtdStore.MtdFormId);

            MtdLogDocument edited = await _context.MtdLogDocument.Where(x => x.MtdStore == MtdStore.Id).OrderByDescending(x => x.TimeCh).FirstOrDefaultAsync();
            MtdLogDocument created = await _context.MtdLogDocument.Where(x => x.MtdStore == MtdStore.Id).OrderBy(x => x.TimeCh).FirstOrDefaultAsync();

            IsReviewer = await _userHandler.IsReviewerAsync(user, MtdForm.Id);

            UserOwner = new WebAppUser();
            StoreOwner = await _context.MtdStoreOwner.Where(x => x.Id == MtdStore.Id).FirstOrDefaultAsync();
            if (StoreOwner != null) { UserOwner = await _userHandler.FindByIdAsync(StoreOwner.UserId); }

            ChangesHistory = new ChangesHistory();

            if (edited != null)
            {
                WebAppUser userEditor = await _userHandler.FindByIdAsync(edited.UserId);
                ChangesHistory.LastEditedUser = userEditor == null ? edited.UserName : userEditor.GetFullName();
                ChangesHistory.LastEditedTime = edited.TimeCh.ToLocalTime().ToString();
            }

            if (created != null)
            {
                WebAppUser userCreator = await _userHandler.FindByIdAsync(created.UserId);
                ChangesHistory.CreateByUser = userCreator == null ? created.UserName : userCreator.GetFullName();
                ChangesHistory.CreateByTime = created.TimeCh.ToLocalTime().ToString();
            }

            ApprovalHandler approvalHandler = new(_context, MtdStore.Id);

            await SetApprovalInfo(id, user, approvalHandler);

            await SetUsersList(user);

            await SetUsersRequest(user, approvalHandler);            

            await SetResolutions(approvalHandler);

            return Page();
        }

        private async Task SetApprovalInfo(string id, WebAppUser user, ApprovalHandler approvalHandler)
        {
            IsApprover = await approvalHandler.IsApproverAsync(user);
            IsFirstStage = await approvalHandler.IsFirstStageAsync();

            Stages = new List<MTDSelectListItem>();
            IList<MtdApprovalStage> stages = await approvalHandler.GetStagesDownAsync();
            stages.OrderBy(x => x.Stage).ToList().ForEach((stage) =>
            {
                Stages.Add(new MTDSelectListItem { Id = stage.Id.ToString(), Value = stage.Name });
            });

            MtdApproval = await approvalHandler.GetApproval();
            List<string> partIds = await approvalHandler.GetWilBeBlockedPartsIds();
            BlockParts = new List<MtdFormPart>();
            if (partIds.Count > 0)
            {
                BlockParts = await _context.MtdFormPart.Where(x => partIds.Contains(x.Id) && x.Title == 1).OrderBy(x => x.Sequence).ToListAsync();
            }
            IsFormApproval = await approvalHandler.IsApprovalFormAsync();

            if (IsFormApproval)
            {
                ApprovalStatus = await approvalHandler.GetStatusAsync(user);

            }

            IsSign = await approvalHandler.IsSignAsync();

            IList<MtdLogApproval> logs = await _context.MtdLogApproval
                .Where(x => x.MtdStore == id).ToListAsync();

            bool isComplete = await approvalHandler.IsComplete();

            ApprovalHistory = new List<ApprovalLog>();
            foreach (var log in logs)
            {
                WebAppUser appUser = await _userHandler.FindByIdAsync(log.UserId);
                ApprovalLog temp = new()
                {
                    Time = log.Timecr,
                    UserName = appUser == null ? log.UserName : appUser.GetFullName(),
                    Result = log.Result,
                    ImgData = log.ImgData,
                    ImgType = log.ImgType,
                    Note = log.Note,
                    Color = log.Color,
                    Comment = log.Comment ?? string.Empty,
                    IsSign = log.IsSign != 0,
                    UserRecipient = log.UserRecipientName,
                };

                ApprovalHistory.Add(temp);
            }
        }


        private async Task SetUsersRequest(WebAppUser user, ApprovalHandler approvalHandler)
        {
            UsersRequest = [];
            if (IsApprover && !IsFirstStage)
            {

                List<WebAppUser> usersRequest = await _userHandler.GetUsersForViewingForm(MtdStore.MtdFormId, MtdStore.Id);

                MtdApprovalStage stage = await approvalHandler.GetCurrentStageAsync();
                List<string> userIds = await approvalHandler.GetUsersWaitSignAsync();
                IList<MtdApprovalStage> mas = await approvalHandler.GetStagesAsync();
                List<string> userInStagesIds = mas.Where(x => x.UserId != "owner").GroupBy(x => x.UserId).Select(x => x.Key).ToList();

                usersRequest = usersRequest.Where(x => !userIds.Contains(x.Id)
                    && !userInStagesIds.Contains(x.Id)
                    && x.Id != user.Id
                    && x.Id != stage.UserId).ToList();

                usersRequest.OrderBy(x => x.Title).ToList().ForEach((item) =>
                {
                    UsersRequest.Add(new MTDSelectListItem { Id = item.Id, Value = item.Title });
                });
            }
        }

        private async Task SetUsersList(WebAppUser user)
        {

            UsersList = new List<MTDSelectListItem>();
            List<WebAppUser> webAppUsers = new();
            bool isViewAll = await _userHandler.CheckUserPolicyAsync(user, MtdStore.MtdFormId, RightsType.ViewAll);

            if (isViewAll)
            {
                webAppUsers = await _userHandler.Users.ToListAsync();
            }
            else
            {
                webAppUsers = await _userHandler.GetUsersInGroupsAsync(user);
            }

            webAppUsers.OrderBy(x => x.Title).ToList().ForEach((item) =>
            {
                UsersList.Add(new MTDSelectListItem
                {
                    Id = item.Id,
                    Value = item.GetFullName()
                });
            });
        }

        private async Task SetResolutions(ApprovalHandler approvalHandler)
        {

            MtdApprovalStage currentStage = await approvalHandler.GetCurrentStageAsync();
            if (currentStage != null)
            {
                List<MtdApprovalRejection> listRejections = await _context.MtdApprovalRejection
                    .Where(x => x.MtdApprovalStageId == currentStage.Id).OrderBy(x => x.Sequence).ToListAsync();
                ListRejections = new List<MTDSelectListItem>();
                listRejections.ForEach((item) =>
                {
                    ListRejections.Add(new MTDSelectListItem
                    {
                        Id = item.Id,
                        Value = item.Name
                    });
                });

                List<MtdApprovalResolution> listResolution = await _context.MtdApprovalResolution
                    .Where(x => x.MtdApprovalStageId == currentStage.Id).OrderBy(x => x.Sequence).ToListAsync();
                ListResolutions = new List<MTDSelectListItem>();
                listResolution.ForEach((item) =>
                {
                    ListResolutions.Add(new MTDSelectListItem
                    {
                        Id = item.Id,
                        Value = item.Name
                    });
                });
            }
        }


    }
}

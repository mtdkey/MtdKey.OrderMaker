/*
    MTD OrderMaker - http://mtdkey.com
    Copyright (c) 2019 Oleg Bruev <job4bruev@gmail.com>. All rights reserved.
*/

using Microsoft.EntityFrameworkCore;
using MtdKey.OrderMaker.Areas.Identity.Data;
using MtdKey.OrderMaker.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MtdKey.OrderMaker.Core
{
    public partial class ApprovalHandler
    {
        private readonly DataConnector _context;
        private readonly string StoreId;
        private MtdStore storeCache;
        private MtdApproval approvalCache;

        public static async Task<bool> UpdateStatusForStartAsync(DataConnector context, string approvalID)
        {
            MtdApproval mtdApproval = await context.MtdApproval
                .Where(x => x.Id == approvalID)
                .FirstOrDefaultAsync();

            if (mtdApproval == null) return false;
            await context.Entry(mtdApproval).Collection(x => x.MtdApprovalStages).LoadAsync();

            MtdApprovalStage firstStage = mtdApproval.MtdApprovalStages.OrderBy(x => x.Stage).FirstOrDefault();
            List<int> stagesIds = mtdApproval.MtdApprovalStages.Select(a => a.Id).ToList();
            if (firstStage == null) return false;

            var deleteList = context.MtdStoreApproval.Where(x => stagesIds.Contains(x.MtdApproveStage) && x.Complete == 0 && x.Result == 0);
            context.MtdStoreApproval.RemoveRange(deleteList);
            await context.SaveChangesAsync();

            var list = context.MtdStoreApproval.Where(x => stagesIds.Contains(x.MtdApproveStage)).Select(x => x.Id);
            var stores = context.MtdStore.Where(x => x.MtdFormId == mtdApproval.MtdForm && !list.Contains(x.Id))
                .Select(x => new MtdStoreApproval { Id = x.Id, MtdApproveStage = firstStage.Id, PartsApproved = "&", Complete = 0, Result = 0, LastEventTime = DateTime.UtcNow });
            await context.MtdStoreApproval.AddRangeAsync(stores);
            await context.SaveChangesAsync();

            return true;
        }

        public static async Task<List<string>> GetWaitStoreIds(DataConnector context, WebAppUser user, string formId)
        {
            List<string> resultList = new();

            List<string> approvalIds = await context.MtdApproval.Where(x => x.MtdForm == formId).Select(x => x.Id).ToListAsync();
            List<int> stagesUser = await context.MtdApprovalStage.Where(x => approvalIds.Contains(x.MtdApproval) && x.UserId == user.Id).Select(x => x.Id).ToListAsync();
            List<int> stagesOwner = await context.MtdApprovalStage.Where(x => approvalIds.Contains(x.MtdApproval) && x.UserId == "owner").Select(x => x.Id).ToListAsync();

            List<string> waitList = await context.MtdStoreApproval
                .Where(x => x.Complete == 0 && stagesUser.Contains(x.MtdApproveStage) && (x.SignChain == null || x.SignChain.Length == 0))
                .Select(x => x.Id).ToListAsync();

            List<string> waitListSign = await context.MtdStoreApproval
                .Where(x => x.Complete == 0 && x.SignChain != null && x.SignChain.Contains(user.Id))
                .Select(x => x.Id).ToListAsync();

            List<string> tempList = await context.MtdStoreApproval.Where(x => x.Complete == 0 && stagesOwner.Contains(x.MtdApproveStage)).Select(x => x.Id).ToListAsync();
            List<string> ownerList = await context.MtdStoreOwner.Where(x => tempList.Contains(x.Id) && x.UserId == user.Id).Select(x => x.Id).ToListAsync();

            resultList.AddRange(waitList);
            resultList.AddRange(ownerList);

            if (waitListSign != null) { resultList.AddRange(waitListSign); }

            return resultList;
        }

        public static async Task<List<ApprovalStore>> GetStoreStatusAsync(DataConnector context, IList<string> storeIds, WebAppUser appUser)
        {
            List<ApprovalStore> resultList = new();

            foreach (string storeId in storeIds)
            {
                ApprovalHandler approvalHandler = new(context, storeId);

                ApprovalStore approvalStore = new()
                {
                    StoreId = storeId
                };

                bool isComplete = await approvalHandler.IsComplete();
                int result = await approvalHandler.GetResultAsync();
                bool isFirst = await approvalHandler.IsFirstStageAsync();
                bool isApprover = await approvalHandler.IsApproverAsync(appUser);
                approvalStore.Status = DefineStatus(isComplete, result, isFirst, isApprover);
                approvalStore.OwnerId = await approvalHandler.GetOwnerID();
                resultList.Add(approvalStore);
            }

            return resultList;
        }

        public static async Task<bool> IsApprovalFormAsync(DataConnector context, string formId)
        {
            bool isApprovalForm = await context.MtdApproval.Where(x => x.MtdForm == formId).AnyAsync();
            return isApprovalForm;
        }

        private async Task<MtdStore> GetStoreAsync()
        {
            if (storeCache != null) return storeCache;
            
            using var db = new OrderMakerContext(_context.Database.GetConnectionString());
            storeCache = await db.MtdStore.FirstOrDefaultAsync(x => x.Id == StoreId);
            if (storeCache == null) return new() { Id = StoreId };

            await db.Entry(storeCache).Reference(x=>x.MtdFormNavigation).LoadAsync();
            await db.Entry(storeCache).Reference(x => x.MtdStoreApproval).LoadAsync();
            await db.Entry(storeCache).Reference(x => x.MtdStoreOwner).LoadAsync();
            db.Entry(storeCache).State = EntityState.Detached;

            return storeCache;
        }

        public async Task<MtdApproval> GetApproval()
        {
            if (approvalCache != null) return approvalCache;
            MtdStore mtdStore = await GetStoreAsync();
            
            var approval = await _context.MtdApproval
                .FirstOrDefaultAsync(x => x.MtdForm == mtdStore.MtdFormId);
            
            if(approval == null) return null;
            
            await _context.Entry(approval).Collection(x => x.MtdApprovalStages).LoadAsync();
            _context.Entry(approval).State = EntityState.Detached; 
            return approval;
        }

        public ApprovalHandler(DataConnector context, string storeId)
        {
            _context = context;
            StoreId = storeId;
            storeCache = null;
            approvalCache = null;
        }

        public void ClearCache()
        {
            approvalCache = null;
            storeCache = null;
        }

        public async Task<bool> IsSignAsync()
        {
            List<string> list = await GetUsersWaitSignAsync();
            return list.Count > 0;
        }

        public async Task<bool> IsApprovalFormAsync()
        {
            MtdApproval mtdApproval = await GetApproval();
            return mtdApproval != null;
        }

        public async Task<MtdApprovalStage> GetCurrentStageAsync()
        {
            MtdApproval approval = await GetApproval();
            if (approval == null) return null;

            MtdStore mtdStore = await GetStoreAsync();
            if (mtdStore.MtdStoreApproval == null) return await GetFirstStageAsync();

            MtdApprovalStage stage = approval.MtdApprovalStages.Where(x => x.Id == mtdStore.MtdStoreApproval.MtdApproveStage).FirstOrDefault();
            if (stage != null && stage.UserId == "owner")
            {
                stage.UserId = await GetOwnerID();
            }
            return stage;
        }

        public async Task<List<string>> GetUsersWaitSignAsync()
        {
            MtdStore mtdStore = await GetStoreAsync();
            List<string> result = new();
            if (mtdStore.MtdStoreApproval != null && mtdStore.MtdStoreApproval.SignChain != null && mtdStore.MtdStoreApproval.SignChain.Length > 0)
            {
                result = mtdStore.MtdStoreApproval.SignChain.Split('&').ToList();
            }
            return result;
        }

        public async Task<bool> IsApproverAsync(WebAppUser user)
        {
            bool isComplete = await IsComplete();
            bool isApprovalForm = await IsApprovalFormAsync();
            if (!isApprovalForm || isComplete) { return false; }

            MtdStore mtdStore = await GetStoreAsync();
            if (mtdStore.MtdStoreApproval != null && mtdStore.MtdStoreApproval.SignChain != null && mtdStore.MtdStoreApproval.SignChain.Length > 0)
            {
                List<string> userIds = await GetUsersWaitSignAsync();
                string userId = userIds.TakeLast(1).FirstOrDefault();
                return user.Id.Equals(userId);
            }

            MtdApprovalStage mtdApprovalStage = await GetCurrentStageAsync();
            MtdStore store = await GetStoreAsync();
            bool forOwner = false;
            if (store.MtdStoreOwner != null)
            {
                MtdApprovalStage firstStage = await GetFirstStageAsync();
                if (firstStage != null)
                {
                    forOwner = (store.MtdStoreOwner.UserId.Equals(user.Id) && mtdApprovalStage.Id.Equals(firstStage.Id));
                }
            }

            if (mtdApprovalStage != null && (mtdApprovalStage.UserId.Equals(user.Id) || forOwner)) { return true; }
            return false;

        }

        public async Task<bool> IsFirstStageAsync()
        {
            MtdApprovalStage first = await GetCurrentStageAsync();
            MtdApprovalStage current = await GetFirstStageAsync();
            if (current == null) { return false; }
            return first.Id.Equals(current.Id);
        }

        public async Task<bool> IsComplete()
        {
            bool result = false;
            MtdStore mtdStore = await GetStoreAsync();
            if (mtdStore.MtdStoreApproval != null && mtdStore.MtdStoreApproval.Complete == 1)
            {
                result = true;
            }
            return result;
        }

        public async Task<IList<MtdApprovalStage>> GetStagesAsync()
        {
            MtdApproval mtdApproval = await GetApproval();
            if (mtdApproval != null)
            {
                return mtdApproval.MtdApprovalStages.OrderBy(x => x.Stage).ToList();
            }

            return new List<MtdApprovalStage>();
        }

        public async Task<List<MtdApprovalStage>> GetStagesDownAsync()
        {
            MtdApproval mtdApproval = await GetApproval();
            if (mtdApproval != null)
            {
                MtdApprovalStage currentStage = await GetCurrentStageAsync();
                return mtdApproval.MtdApprovalStages.Where(x => x.Stage < currentStage.Stage).OrderBy(x => x.Stage).ToList();
            }

            return new List<MtdApprovalStage>();
        }

        public async Task<MtdApprovalStage> GetFirstStageAsync()
        {
            IList<MtdApprovalStage> mtdApprovalStages = await GetStagesAsync();
            return mtdApprovalStages.FirstOrDefault();
        }

        public async Task<MtdApprovalStage> GetLastStageAsync()
        {
            IList<MtdApprovalStage> mtdApprovalStages = await GetStagesAsync();
            return mtdApprovalStages.OrderByDescending(x => x.Stage).FirstOrDefault();
        }

        public async Task<MtdApprovalStage> GetPrevStage()
        {
            MtdApprovalStage prevStage = await GetCurrentStageAsync();
            IList<MtdApprovalStage> stages = await GetStagesAsync();
            var stage = stages.Where(x => x.Stage < prevStage.Stage).FirstOrDefault();
            if (stage != null) { prevStage = stage; }
            
            if (prevStage.UserId == "owner")
            {
                prevStage.UserId = await GetOwnerID();
            }
            
            return prevStage;
        }

        public async Task<MtdApprovalStage> GetNextStageAsync()
        {
            MtdApprovalStage current = await GetCurrentStageAsync();
            IList<MtdApprovalStage> stages = await GetStagesAsync();
            MtdApprovalStage next = stages.Where(x => x.Stage > current.Stage).FirstOrDefault();
            if (next != null && next.UserId == "owner")
            {
                next.UserId = await GetOwnerID();
            }
            return next;

        }

        public async Task<string> GetCurrentUserIdAsync()
        {
            MtdApprovalStage mtdStage = await GetCurrentStageAsync();
            List<string> userIds = await GetUsersWaitSignAsync();
            if (userIds.Count > 0) { return userIds.TakeLast(1).FirstOrDefault(); }
            
            if (mtdStage.UserId == "owner")
            {
                mtdStage.UserId = await GetOwnerID();
            }

            return mtdStage.UserId; 
        }

        public async Task<MtdForm> GetFormAsync()
        {
            MtdStore mtdStore = await GetStoreAsync();
            return mtdStore.MtdFormNavigation;
        }

        public async Task<bool> ActionApproveReset(WebAppUser webAppUser)
        {
            MtdStore mtdStore = await GetStoreAsync();
            MtdApprovalStage mtdApprovalStage = await GetFirstStageAsync();
            MtdStoreApproval mtdStoreApproval = new()
            {
                Id = mtdStore.MtdStoreApproval.Id,
                Complete = 0,
                MtdApproveStage = mtdApprovalStage.Id,
                Result = -1,
                PartsApproved = "&",
                LastEventTime = DateTime.UtcNow,
            };

            MtdLogApproval mtdLogApproval = new()
            {
                MtdStore = mtdStore.Id,
                Result = -2,
                Stage = mtdApprovalStage.Id,
                Timecr = DateTime.UtcNow,
                UserId = webAppUser.Id,
                UserName = webAppUser.GetFullName(),
            };

            _context.MtdStoreApproval.Update(mtdStoreApproval);
            _context.MtdLogApproval.Update(mtdLogApproval);

            bool result = true;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch
            {
                result = false;
            }

            return result;
        }

        public async Task<bool> ActionApprove(WebAppUser webAppUser, string resolution = null, string comment = null)
        {

            MtdStore mtdStore = await GetStoreAsync();
            if (mtdStore.MtdStoreApproval != null && mtdStore.MtdStoreApproval.Complete == 1) { return false; }

            MtdApprovalStage currentStage = await GetCurrentStageAsync();
            MtdApprovalStage nextStage = await GetNextStageAsync();
            sbyte complete = 0;
            if (nextStage == null) { complete = 1; nextStage = await GetLastStageAsync(); };

            MtdStoreApproval storeApproval = new()
            {
                Id = mtdStore.Id,
                MtdApproveStage = nextStage.Id,
                PartsApproved = currentStage.BlockParts,
                Complete = complete,
                Result = 1,
                LastEventTime = DateTime.UtcNow,
            };


            if (mtdStore.MtdStoreApproval == null)
            {
                await _context.MtdStoreApproval.AddAsync(storeApproval);
            }
            else
            {
                _context.MtdStoreApproval.Update(storeApproval);
            }

            string commentText = comment ?? string.Empty;
            MtdLogApproval mtdLogApproval = new()
            {
                MtdStore = mtdStore.Id,
                Result = 1,
                Stage = currentStage.Id,
                Timecr = DateTime.UtcNow,
                UserId = webAppUser.Id,
                UserName = webAppUser.GetFullName(),
                Comment = commentText.Length > 250 ? commentText[..250] : commentText
            };

            if (resolution != null)
            {
                MtdApprovalResolution Resolution = await _context.MtdApprovalResolution.FindAsync(resolution);
                if (Resolution != null)
                {
                    mtdLogApproval.ImgData = Resolution.ImgData;
                    mtdLogApproval.ImgType = Resolution.ImgType;
                    mtdLogApproval.Note = Resolution.Name;
                    mtdLogApproval.Color = Resolution.Color;
                }
            }

            await _context.MtdLogApproval.AddAsync(mtdLogApproval);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }

            return true;
        }

        public async Task<bool> ActionResponceSignAsync(WebAppUser webAppUser, string comment = null , int result = 0)
        {
            /*
             * Back one step on sign chain   
             */

            MtdStore mtdStore = await GetStoreAsync();
            if (mtdStore.MtdStoreApproval != null && mtdStore.MtdStoreApproval.Complete == 1) { return false; }
            if (mtdStore.MtdStoreApproval.SignChain == null || mtdStore.MtdStoreApproval.SignChain.Length == 0) { return false; }

            MtdStoreApproval storeApproval = mtdStore.MtdStoreApproval;
            List<string> userIds = await GetUsersWaitSignAsync();
            string signChain = storeApproval.SignChain ?? string.Empty;
            if (userIds.Count > 1)
            {
                string lastId = userIds.TakeLast(1).FirstOrDefault();
                signChain = signChain.Replace($"&{lastId}","");
            }
            else 
            {
                signChain = string.Empty;
            }
            
            storeApproval.SignChain = signChain;
            storeApproval.LastEventTime = DateTime.UtcNow;

            _context.MtdStoreApproval.Update(storeApproval);
            MtdApprovalStage currentStage = await GetCurrentStageAsync();

            string commentText = comment ?? string.Empty;
            MtdLogApproval mtdLogApproval = new()
            {
                MtdStore = mtdStore.Id,
                Result = result,
                Stage = currentStage.Id,
                Timecr = DateTime.UtcNow,
                UserId = webAppUser.Id,
                UserName = webAppUser.GetFullName(),
                Comment = commentText.Length > 250 ? commentText[..250] : commentText,
                IsSign = 1,
            };

            await _context.MtdLogApproval.AddAsync(mtdLogApproval);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }

            return true;
        }

        public async Task<bool> ActionReject(bool complete, int idStage, WebAppUser webAppUser, string rejection = null, string comment = null)
        {
            MtdStore mtdStore = await GetStoreAsync();
            if (mtdStore.MtdStoreApproval != null && mtdStore.MtdStoreApproval.Complete == 1) { return false; }

            MtdApproval mtdApproval = await GetApproval();
            MtdApprovalStage currentStage = await GetCurrentStageAsync();
            MtdApprovalStage prevStage;
            if (!complete)
            {
                prevStage = mtdApproval.MtdApprovalStages.Where(x => x.Id == idStage).FirstOrDefault();
            }
            else
            {
                prevStage = await GetCurrentStageAsync();
            }

            if (prevStage == null) { return false; }

            var allStages = await GetStagesAsync();
            var blockPartsStage = allStages.Where(x => x.Stage < prevStage.Stage).FirstOrDefault();

            MtdStoreApproval storeApproval = new()
            {
                Id = mtdStore.Id,
                MtdApproveStage = prevStage.Id,
                PartsApproved = blockPartsStage == null ? "&" : blockPartsStage.BlockParts,
                Complete = complete ? (sbyte)1 : (sbyte)0,
                Result = -1,
                LastEventTime = DateTime.UtcNow,
            };

            if (mtdStore.MtdStoreApproval == null)
            {
                await _context.MtdStoreApproval.AddAsync(storeApproval);
            }
            else
            {
                try
                {
                    _context.MtdStoreApproval.Update(storeApproval);
                }
                catch
                {
                    //Set logger 
                }
            }

            string commentText = comment ?? string.Empty;
            MtdLogApproval mtdLogApproval = new()
            {
                MtdStore = mtdStore.Id,
                Result = complete ? -1 : -2,
                Stage = currentStage.Id,
                Timecr = DateTime.UtcNow,
                UserId = webAppUser.Id,
                UserName = webAppUser.GetFullName(),
                Comment = commentText.Length > 250 ? commentText[..250] : commentText
            };

            if (rejection != null)
            {
                MtdApprovalRejection Rejection = await _context.MtdApprovalRejection.FindAsync(rejection);
                if (Rejection != null)
                {
                    mtdLogApproval.ImgData = Rejection.ImgData;
                    mtdLogApproval.ImgType = Rejection.ImgType;
                    mtdLogApproval.Note = Rejection.Name;
                    mtdLogApproval.Color = Rejection.Color;
                }
            }

            await _context.MtdLogApproval.AddAsync(mtdLogApproval);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch
            {
                return false;
            }

            return true;
        }

        public async Task<bool> ActionRequest(WebAppUser userSender, WebAppUser userRecipient, string comment = null)
        {
            MtdStore mtdStore = await GetStoreAsync();
            MtdApprovalStage approvalStage = await GetCurrentStageAsync();
            MtdLogApproval mtdLog = new()
            {
                MtdStore = StoreId,
                Stage = approvalStage.Id,
                UserId = userSender.Id,
                UserName = userSender.GetFullName(),
                Result = 0,
                Timecr = DateTime.UtcNow,
                Comment = comment,
                UserRecipientId = userRecipient.Id,
                UserRecipientName = userRecipient.Title,
                IsSign = 1,                
            };

            string singChain = string.Empty;
            if (mtdStore.MtdStoreApproval.SignChain != null && mtdStore.MtdStoreApproval.SignChain.Length > 0)
            {
                singChain = mtdStore.MtdStoreApproval.SignChain;
            }

            singChain += $"&{userRecipient.Id}";
            mtdStore.MtdStoreApproval.SignChain = singChain;
            mtdStore.MtdStoreApproval.LastEventTime = DateTime.UtcNow;

            _context.MtdStoreApproval.Update(mtdStore.MtdStoreApproval);
            await _context.MtdLogApproval.AddAsync(mtdLog);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch
            {
                return false;
            }

            return true;
        }

        public async Task<bool> ActionConsiderApproved (WebAppUser webAppUser, int result, string comment = null)
        {
            MtdStore mtdStore = await GetStoreAsync();
            if (mtdStore.MtdStoreApproval != null && mtdStore.MtdStoreApproval.Complete == 1) { return false; }

            MtdApprovalStage currentStage = await GetCurrentStageAsync();
            MtdApprovalStage lastStage = await GetLastStageAsync();

            sbyte complete = 1;

            MtdStoreApproval storeApproval = new()
            {
                Id = mtdStore.Id,
                MtdApproveStage = lastStage.Id,
                PartsApproved = lastStage.BlockParts,
                Complete = complete,
                Result = result,
                LastEventTime = DateTime.UtcNow
            };


            if (mtdStore.MtdStoreApproval == null)
            {
                await _context.MtdStoreApproval.AddAsync(storeApproval);
            }
            else
            {
                _context.MtdStoreApproval.Update(storeApproval);
            }

            string commentText = comment ?? string.Empty;
            MtdLogApproval mtdLogApproval = new()
            {
                MtdStore = mtdStore.Id,
                Result = result,
                Stage = currentStage.Id,
                Timecr = DateTime.UtcNow,
                UserId = webAppUser.Id,
                UserName = webAppUser.GetFullName(),
                Comment = commentText.Length > 250 ? commentText[..250] : commentText,
                Note = lastStage.Name
            };


            await _context.MtdLogApproval.AddAsync(mtdLogApproval);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }

            ClearCache();

            return true;
        }

        public async Task<List<string>> GetBlockedPartsIdsAsync()
        {
            MtdStore mtdStore = await GetStoreAsync();
            List<string> ids = new();
            if (mtdStore.MtdStoreApproval != null)
            {
                ids = mtdStore.MtdStoreApproval.PartsApproved.Split("&").ToList();
                if (ids.Any())
                {
                    ids.RemoveAt(ids.Count - 1);
                }
            }

            return ids;
        }

        public async Task<List<string>> GetWilBeBlockedPartsIds()
        {
            List<string> ids = new();
            MtdApprovalStage mtdApprovalStage = await GetCurrentStageAsync();
            if (mtdApprovalStage != null)
            {
                ids = mtdApprovalStage.BlockParts.Split("&").ToList();
                if (ids.Any())
                {
                    ids.RemoveAt(ids.Count - 1);
                }
            }

            return ids;
        }

        public async Task<string> GetOwnerID()
        {
            MtdStore mtdStore = await GetStoreAsync();
            if (mtdStore.MtdStoreOwner == null) return string.Empty;
            return mtdStore.MtdStoreOwner.UserId;
        }

        public async Task<string> GetStoreID()
        {
            string result = string.Empty;
            MtdStore mtdStore = await GetStoreAsync();
            if (mtdStore != null) { result = mtdStore.Id; }
            return result;
        }

        public async Task<ApprovalStatus> GetStatusAsync(WebAppUser appUser)
        {
            ApprovalStatus status = ApprovalStatus.Start;
            MtdApprovalStage stage = await GetCurrentStageAsync();
            if (stage == null) return status;

            bool isComplete = await IsComplete();
            int result = await GetResultAsync();
            bool isFirst = await IsFirstStageAsync();
            bool isApprover = await IsApproverAsync(appUser);
            status = DefineStatus(isComplete, result, isFirst, isApprover);

            return status;
        }

        private static ApprovalStatus DefineStatus(bool isComplete, int result, bool isFirst, bool isApprover)
        {
            ApprovalStatus status = ApprovalStatus.Start;

            switch (result)
            {
                case 0:
                    {
                        status = ApprovalStatus.Start;
                        break;
                    }
                case -1:
                    {
                        status = ApprovalStatus.Rejected;
                        break;
                    }
                case 1:
                    {
                        status = ApprovalStatus.Approved;
                        break;
                    }
            }

            if (!isComplete && result != 0) { status = ApprovalStatus.Waiting; }

            if (!isComplete && isFirst && result == -1)
            {
                status = ApprovalStatus.Iteraction;
            }

            if (!isComplete && isApprover && !isFirst)
            {
                status = ApprovalStatus.Required;
            }

            return status;
        }

        public async Task<int> GetResultAsync()
        {
            MtdStore mtdStore = await GetStoreAsync();

            if (mtdStore.MtdStoreApproval != null)
            {
                return mtdStore.MtdStoreApproval.Result;
            }

            return 0;
        }

    }
}

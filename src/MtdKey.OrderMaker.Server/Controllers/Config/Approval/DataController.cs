/*
    MTD OrderMaker - http://mtdkey.com
    Copyright (c) 2019 Oleg Bruev <job4bruev@gmail.com>. All rights reserved.
*/

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MtdKey.OrderMaker.Core;
using MtdKey.OrderMaker.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MtdKey.OrderMaker.Controllers.Config.Approval
{
    [Route("api/config/approval")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class DataController : ControllerBase
    {

        private readonly DataConnector _context;


        public DataController(DataConnector context)
        {
            _context = context;
        }

        [HttpPost("delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostDeleteAsync()
        {
            string approvalId = Request.Form["IdApproval"];
            if (approvalId == null)
            {
                return NotFound();
            }

            MtdApproval mtdApproval = new() { Id = approvalId };
            _context.MtdApproval.Remove(mtdApproval);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPost("stage/create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostStageCreateAsync()
        {
            string approvalId = Request.Form["approvalId"];
            string fieldName = Request.Form["fieldName"];
            string fieldNote = Request.Form["fieldNote"];
            string fieldUser = "owner";

            string blockParts = string.Empty;
            MtdApproval mtdApproval = await _context.MtdApproval.FindAsync(approvalId ?? "");

            if (mtdApproval == null)
            {
                return BadRequest(new JsonResult("Error. Approval not found."));
            }


            int stageLevel = await _context.MtdApprovalStage.CountAsync(x => x.MtdApproval == approvalId);
            stageLevel++;

            MtdApprovalStage stage = new()
            {
                MtdApproval = approvalId,
                BlockParts = blockParts,
                Description = fieldNote,
                Name = fieldName,
                UserId = fieldUser,
                Stage = stageLevel,
            };

            await _context.MtdApprovalStage.AddAsync(stage);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("stage/edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostStageEditAsync()
        {
            string stageId = Request.Form["stageId"];
            string fieldName = Request.Form["fieldName"];
            string fieldNote = Request.Form["fieldNote"];
            string fieldUser = Request.Form["fieldUser"];

            if (stageId == null
                || !int.TryParse(stageId, out int id)
                || !int.TryParse(Request.Form["fieldStage"], out int fieldStage)
                ) return NotFound();

            string blockParts = string.Empty;

            MtdApprovalStage mtdApprovalStage = await _context.MtdApprovalStage
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            if (mtdApprovalStage == null) { return NotFound(); }
            await _context.Entry(mtdApprovalStage)
                .Reference(x => x.MtdApprovalNavigation).LoadAsync();

            IList<string> partIds = await _context.MtdFormPart
                .Where(x => x.MtdFormId == mtdApprovalStage.MtdApprovalNavigation.MtdForm)
                .Select(x => x.Id).ToListAsync();

            foreach (string idPart in partIds)
            {
                bool result = bool.TryParse(Request.Form[idPart], out bool check);
                if (result && check)
                {
                    blockParts += $"{idPart}&";
                }
            }

            mtdApprovalStage.BlockParts = blockParts;
            mtdApprovalStage.Description = fieldNote;
            mtdApprovalStage.Name = fieldName;
            mtdApprovalStage.UserId = fieldUser;
            mtdApprovalStage.Stage = fieldStage;

            _context.MtdApprovalStage.Update(mtdApprovalStage);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("stage/delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostStageDeleteAsync()
        {
            string stageId = Request.Form["id-stage-delete"];
            bool ok = int.TryParse(stageId, out int id);
            if (stageId == null && !ok)
            {
                return NotFound();
            }

            MtdApprovalStage stage = new() { Id = id };
            _context.MtdApprovalStage.Remove(stage);
            await _context.SaveChangesAsync();
            return Ok();
        }


        [HttpPost("stage/sequence")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostStageSequenceAsync()
        {
            string strData = Request.Form["drgData"];
            string approvalId = Request.Form["IdApproval"];
            string formId = Request.Form["fieldForm"];

            string[] data = strData.Split("&");

            IList<MtdApprovalStage> stages = await _context.MtdApprovalStage.Where(x => x.MtdApproval == approvalId).ToListAsync();

            int counter = 1;
            foreach (string idStr in data)
            {
                bool isOk = int.TryParse(idStr, out int id);
                if (isOk)
                {
                    var field = stages.Where(x => x.Id == id).FirstOrDefault();
                    if (field != null)
                    {
                        field.Stage = counter;
                        counter++;
                    }
                }
            }

            _context.MtdApprovalStage.UpdateRange(stages);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("stage/update")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostStageUpdateAsync()
        {
            string approvalId = Request.Form["approval-id"];

            await ApprovalHandler.UpdateStatusForStartAsync(_context, approvalId);

            return Ok();
        }

    }
}
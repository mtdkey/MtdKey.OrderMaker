/*
    MTD OrderMaker - http://mtdkey.com
    Copyright (c) 2019 Oleg Bruev <job4bruev@gmail.com>. All rights reserved.
*/

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MtdKey.OrderMaker.Entity;
using MtdKey.OrderMaker.Services;
using System;
using System.Threading.Tasks;

namespace MtdKey.OrderMaker.Controllers.Users
{

    [Route("api/users/admin/groups")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class GroupsController : ControllerBase
    {
        private readonly DataConnector _context;

        public GroupsController(DataConnector context)
        {
            _context = context;
        }


        [HttpPost("add")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostAddAsync()
        {
            var requestForm = await Request.ReadFormAsync();
            string name = requestForm["group-name"];
            string note = requestForm["group-note"];         

            MtdGroup mtdGroup = new() { Id = Guid.NewGuid().ToString(), Name = name, Description = note };

            await _context.MtdGroup.AddAsync(mtdGroup);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostEditAsync()
        {
            var requestForm = await Request.ReadFormAsync();
            string id = requestForm["group-id"];
            string name = requestForm["group-name"];
            string note = requestForm["group-note"];          

            MtdGroup mtdGroup = await _context.MtdGroup.FindAsync(id);
            if (mtdGroup == null) { return NotFound(); }

            mtdGroup.Name = name;
            mtdGroup.Description = note;   

            _context.MtdGroup.Update(mtdGroup);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostDeleteAsync()
        {
            string id = Request.Form["group-id"];

            MtdGroup mtdGroup = new() { Id = id };

            _context.MtdGroup.Remove(mtdGroup);
            await _context.SaveChangesAsync();

            return Ok();
        }


        [HttpGet("group/{groupId}")]
        public async Task<IActionResult> GetGroupInfo(string groupId)
        {

            MtdGroup mtdGroup = await _context.MtdGroup.FindAsync(groupId);
  
            if (mtdGroup != null)
            {
                return new JsonResult(new { mtdGroup.Id, groupName = mtdGroup.Name, groupOwner = "No owner selected" });
            }

            if (mtdGroup == null)
            {
                return new JsonResult(new { Id = "null", groupName = "No group", groupOwner = "No owner selected" });
            }

            return null;

        }

    }
}

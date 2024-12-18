/*
    MTD OrderMaker - http://mtdkey.com
    Copyright (c) 2019 Oleg Bruev <job4bruev@gmail.com>. All rights reserved.
*/

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MtdKey.OrderMaker.Entity;
using System;

namespace MtdKey.OrderMaker.Areas.Identity.Pages.Users.Policy
{
    public class CreateModel : PageModel
    {
        private readonly DataConnector _context;

        public CreateModel(DataConnector context)
        {
            _context = context;
        }

        [BindProperty]
        public MtdPolicy MtdPolicy { get; set; }

        public void OnGet()
        {
            MtdPolicy = new MtdPolicy()
            {
                Id = Guid.NewGuid().ToString()
            };
        }



    }
}
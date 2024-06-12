/*
     MTD OrderMaker - http://mtdkey.com
    Copyright (c) 2019 Oleg Bruev <job4bruev@gmail.com>. All rights reserved.
 */

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MtdKey.OrderMaker.AppConfig;
using MtdKey.OrderMaker.Entity;
using System;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;

namespace MtdKey.OrderMaker.Areas.Config.Pages.Interface
{

    public class IndexModel : PageModel
    {

        private readonly DataConnector _context;


        public IndexModel(DataConnector context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            ViewData["ImgMenu"] = await GetImageFromConfig(1);
            ViewData["ImgAppBar"] = await GetImageFromConfig(2);
            var barColor = await _context.MtdConfigParam.Where(x => x.Id == ParamId.BarColor).Select(x => x.Value).FirstOrDefaultAsync();
            var iconColor = await _context.MtdConfigParam.Where(x => x.Id == ParamId.IconColor).Select(x => x.Value).FirstOrDefaultAsync();
            ViewData["BarColor"] = barColor ?? "#00008a";
            ViewData["IconColor"] = iconColor ?? "#ffffff";
            return Page();
        }


        public async Task<IActionResult> OnPostAsync()
        {

            await SaveImg(1);
            await SaveImg(2);

            string colorBar = Request.Form["color-bar"];
            string colorIcon = Request.Form["color-icon"];

            await SaveBarColor(colorBar);
            await SaveIconColor(colorIcon);

            await _context.SaveChangesAsync();



            return Page();
        }


        private async Task SaveImg(int code)
        {
            string idCheckBox = $"{code}-delete";
            if (Request.Form[idCheckBox].FirstOrDefault() == null || Request.Form[idCheckBox].FirstOrDefault() == "false")
            {
                string idInput = $"{code}-file-upload-input";
                IFormFile file = Request.Form.Files.FirstOrDefault(x => x.Name == idInput);
                if (file != null)
                {
                    byte[] streamArray = new byte[file.Length];
                    await file.OpenReadStream().ReadAsync(streamArray, 0, streamArray.Length);
                    MtdConfigFile imgConfig = new()
                    {
                        Id = code,
                        Name = code == 1 ? "Image for menu" : "Image for AppBar",
                        FileData = streamArray,
                        FileSize = streamArray.Length,
                        FileType = file.ContentType
                    };

                    bool exists = await _context.MtdConfigFiles.Where(x => x.Id == code).AnyAsync();
                    if (exists)
                        _context.Attach(imgConfig).State = EntityState.Modified;
                    else
                        _context.Attach(imgConfig).State = EntityState.Added;
                }
            }
            else
            {
                MtdConfigFile header = new() { Id = code };
                _context.Attach(header).State = EntityState.Deleted;
            }

        }

        private async Task SaveBarColor(string color)
        {

            MtdConfigParam mtdConfigParam = await _context.MtdConfigParam.FindAsync(1);
            if (mtdConfigParam == null)
            {
                mtdConfigParam = new MtdConfigParam
                {
                    Id = 1,
                    Name = "Bar Color",
                    Value = color
                };

                await _context.MtdConfigParam.AddAsync(mtdConfigParam);
                return;
            }

            mtdConfigParam.Value = color;
            _context.MtdConfigParam.Update(mtdConfigParam);
            return;
        }

        private async Task SaveIconColor(string color)
        {

            MtdConfigParam mtdConfigParam = await _context.MtdConfigParam.FindAsync(2);
            if (mtdConfigParam == null)
            {
                mtdConfigParam = new MtdConfigParam
                {
                    Id = 2,
                    Name = "Icon Color",
                    Value = color
                };

                await _context.MtdConfigParam.AddAsync(mtdConfigParam);
                return;
            }

            mtdConfigParam.Value = color;
            _context.MtdConfigParam.Update(mtdConfigParam);
            return;
        }

        private async Task<string> GetImageFromConfig(int code)
        {
            string result = string.Empty;
            MtdConfigFile configFile = await _context.MtdConfigFiles.FindAsync(code);
            if (configFile != null && configFile.FileData != null)
            {
                string base64 = Convert.ToBase64String(configFile.FileData);
                result = string.Format("data:{0};base64,{1}", configFile.FileType, base64);
            }

            return result;
        }

    }
}
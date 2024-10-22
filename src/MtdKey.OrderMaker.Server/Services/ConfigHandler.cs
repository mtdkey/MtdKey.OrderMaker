using Microsoft.Extensions.Options;
using MtdKey.OrderMaker.AppConfig;
using MtdKey.OrderMaker.Entity;
using System;
using System.Threading.Tasks;

namespace MtdKey.OrderMaker.Services
{
    public class ConfigHandler(DataConnector context, IOptions<ConfigSettings> options)
    {
        public int CodeImgMenu => 1;
        public int CodeImgAppBar => 2;
        public string AppName => options.Value.AppName;

        private readonly DataConnector _context = context;
        public readonly IOptions<ConfigSettings> options = options;

        public async Task<string> GetImageFromConfig(int code)
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

        public async Task<string> GetColorBar()
        {
            string color = "#00008a";
            MtdConfigParam mtdConfigParam = await _context.MtdConfigParam.FindAsync(1);
            if (mtdConfigParam != null)
            {
                color = mtdConfigParam.Value;
            }
            return color;
        }

        public async Task<string> GetColorIcon()
        {
            string color = "#ffffff";
            MtdConfigParam mtdConfigParam = await _context.MtdConfigParam.FindAsync(2);
            if (mtdConfigParam != null)
            {
                color = mtdConfigParam.Value;
            }
            return color;
        }
    }
}

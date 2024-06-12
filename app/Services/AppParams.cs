using Microsoft.EntityFrameworkCore;
using MtdKey.OrderMaker.AppConfig;
using MtdKey.OrderMaker.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MtdKey.OrderMaker.Services
{
    public interface IAppParams
    {
        Task<Dictionary<ParamId, string>> GetAllAsync();
        Task<string> GetValueAsync(ParamId paramId);
        Task SaveAsync(ParamId paramId, string value);
    }

    public class AppParams(OrderMakerContext context) : IAppParams
    {
        private readonly OrderMakerContext context = context;

        public async Task<Dictionary<ParamId, string>> GetAllAsync()
        {
            Dictionary<ParamId, string> list = await context.MtdConfigParam
                .ToDictionaryAsync(t => (ParamId)t.Id, t => t.Value);
            return list;
        }

        public async Task<string> GetValueAsync(ParamId paramId)
        {
            int id = paramId;
            var data = await context.MtdConfigParam.FindAsync(id);
            return data?.Value ?? string.Empty;
        }

        public async Task SaveAsync(ParamId paramId, string value)
        {
            int id = paramId;
            var data = await context.MtdConfigParam.FindAsync(id);
            if (data == null)
            {
                data = new MtdConfigParam { Id = id, Name = paramId.ToString() };
                await context.MtdConfigParam.AddAsync(data);
            };

            data.Value = value;
            await context.SaveChangesAsync();

        }
    }
}

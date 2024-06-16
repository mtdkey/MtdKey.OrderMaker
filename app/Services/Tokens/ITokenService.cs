using System;
using System.Threading.Tasks;

namespace MtdKey.OrderMaker.Services.Tokens
{
    public interface ITokenService
    {        
        Task<string> EncodeTokenAsync<TModel>(TModel model) where TModel: class;
        Task<TModel> DecodeTokenAsync<TModel>(string token) where TModel : class, new();
        Task<bool> CheckTokenValidityAsync(string token);
        Task RemoveTokenAsync(string token);
        Task RemoveTokensBeforeDate(DateTime dateTime);
    }
}

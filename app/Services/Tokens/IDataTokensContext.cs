using Microsoft.EntityFrameworkCore;

namespace MtdKey.OrderMaker.Services.Tokens
{
    public interface IDataTokensContext
    {
        DbSet<TokenProof> TokenProofs { get; }
    }
}

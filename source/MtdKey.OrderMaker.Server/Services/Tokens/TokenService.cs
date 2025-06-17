using Microsoft.EntityFrameworkCore;
using MtdKey.Cipher;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace MtdKey.OrderMaker.Services.Tokens
{
    public class TokenService<TContext>(TContext context) : ITokenService where TContext : DbContext, IDataTokensContext
    {
        private readonly TContext _context = context;

        public async Task<bool> CheckTokenValidityAsync(string token)
        {
            var tokenSchema = AesCore.SplitToken(token);
            return await _context.TokenProofs.AnyAsync(x => x.Id == tokenSchema.IV);
        }

        public async Task<TModel> DecodeTokenAsync<TModel>(string token) where TModel : class, new()
        {
            var tokenSchema = AesCore.SplitToken(token);
            using Aes aes = Aes.Create();
            aes.KeySize = 256;

            var proof = await _context.TokenProofs.AsNoTracking().FirstOrDefaultAsync(x => x.Id == tokenSchema.IV);
            if (proof == null) { return new TModel(); }

            return aes.DecryptModel<TModel>(token, proof.SecretKey, 256);

        }

        public async Task<string> EncodeTokenAsync<TModel>(TModel model) where TModel : class
        {
            using Aes aes = Aes.Create();
            aes.KeySize = 256;
            aes.GenerateIV();
            aes.GenerateKey();
            var iv = Convert.ToBase64String(aes.IV);
            var key = Convert.ToBase64String(aes.Key);
            var token = aes.EncryptModel(model, aes.IV, key, 256);

            await _context.TokenProofs.AddAsync(new()
            {
                Id = iv,
                SecretKey = key,
            });
            
            await _context.SaveChangesAsync();

            return token;
        }

        public async Task RemoveTokenAsync(string token)
        {
            var tokenSchema = AesCore.SplitToken(token);
            _context.TokenProofs.Remove(new TokenProof { Id = tokenSchema.IV });
            await _context.SaveChangesAsync();

        }

        public async Task RemoveTokensBeforeDate(DateTime dateTime)
        {
            IList<TokenProof> data = await _context.TokenProofs.Where(x => x.Created.Date < dateTime.Date).ToListAsync();
            _context.RemoveRange(data);
            await _context.SaveChangesAsync();
        }
    }
}

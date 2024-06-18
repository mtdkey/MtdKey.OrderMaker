using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Threading.Tasks;

namespace MtdKey.OrderMaker.Services.FileStorage
{
    public class FileStorageService<TContext>(TContext context,
        IOptions<FileStorageOption> options, ILogger<FileStorageService<TContext>> logger) : IFileStorageService
            where TContext : DbContext, IFileStorageContext
    {
        private readonly TContext _context = context;
        private readonly FileStorageOption _option = options.Value;
        private readonly ILogger<FileStorageService<TContext>> _logger = logger;

        private async Task<FileStorageItem> GetItemAsync(Guid fileId)
        {
            var item = await _context.FileStorageItems.FirstOrDefaultAsync(x => x.Id == fileId);
            if (item == null) { return new(); }
            return item;
        }

        private string GetPath(DateTime date)
        {
            return Path.Combine(_option.RootPath, date.ToString("yyyyMMdd"));
        } 

        public async Task<byte[]> GetFileAsync(Guid fileId)
        {
            var item = await GetItemAsync(fileId);
            var path = GetPath(item.Created);
            var fullPath = Path.Combine(path, item.Id.ToString());
            return await File.ReadAllBytesAsync(fullPath);

        }

        public async Task<string> GetFileMimeAsync(Guid fileId)
        {
            var item = await GetItemAsync(fileId);
            return item.Mime;
        }

        public async Task<string> GetFileName(Guid fileId)
        {
            var item = await GetItemAsync(fileId);
            return item.Name;
        }

        public async Task<long> GetFileSizeAsync(Guid fileId)
        {
            var item = await GetItemAsync(fileId);
            return item.Size;
        }

        public async Task<Guid?> PutFileAsync(string name, long size, string mime, byte[] bytes)
        {

            var item = new FileStorageItem
            {
                Name = name,
                Size = size,
                Mime = mime,
            };

            try
            {
                var path = GetPath(item.Created);
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);

                var fullPath = Path.Combine(path, item.Id.ToString());
                await File.WriteAllBytesAsync(fullPath, bytes);

                await _context.FileStorageItems.AddAsync(item);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("{message}", ex.Message);
                return null;
            }

            return item.Id;
        }

    }
}

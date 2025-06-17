using System;
using System.Drawing;
using System.Threading.Tasks;

namespace MtdKey.OrderMaker.Services.FileStorage
{
    public interface IFileStorageService
    {
        Task<Guid?> PutFileAsync(string name, long size, string mime, byte[] bytes);
        Task<byte[]> GetFileAsync(Guid fileId);
        Task<string> GetFileName(Guid fileId);
        Task<long> GetFileSizeAsync(Guid fileId);
        Task<string> GetFileMimeAsync(Guid fileId);
    }
}

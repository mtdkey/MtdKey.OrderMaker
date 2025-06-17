using Microsoft.EntityFrameworkCore;

namespace MtdKey.OrderMaker.Services.FileStorage
{
    public interface IFileStorageContext
    {
        DbSet<FileStorageItem> FileStorageItems { get; }
    }
}

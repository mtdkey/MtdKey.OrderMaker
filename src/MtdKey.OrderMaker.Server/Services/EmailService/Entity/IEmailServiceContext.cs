using Microsoft.EntityFrameworkCore;
using MtdKey.OrderMaker.Services.FileStorage;

namespace MtdKey.OrderMaker.Services.EmailService.Entity
{
    public interface IEmailServiceContext
    {
        DbSet<EmailTask> EmailTasks { get; set; }
    }
}

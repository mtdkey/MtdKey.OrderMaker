using System.Collections.Generic;
using System.Threading.Tasks;
using MtdKey.EmailBuilder;
using MtdKey.OrderMaker.Services.EmailService.Templates;

namespace MtdKey.OrderMaker.Services.EmailService
{
    public interface IEmailService
    {
        Task AddEmailTaskAsync(string recepient, EmailTaskDesigner designer);

        Task<long[]> GetActiveTasksAsync();
        Task SendEmailAsync(long taskId);
    }
}

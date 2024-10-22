using MtdKey.InboxMediator.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MtdKey.InboxMediator.Service
{
    public interface IEmailMediatorWorker
    {
        Task LoadHeadersAsync(EmailMediator mediator);
        Task<bool> GetLoaderFlagStatusAsync(string email);
        Task SetLoaderFlagStatusAsync(string email, bool status);
    }
}

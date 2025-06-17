
using Microsoft.Extensions.DependencyInjection;

namespace MtdKey.InboxMediator
{
    public abstract class EmailModelHandler
    {
        public abstract void SetServiceScopeFactory(IServiceScopeFactory serviceScopeFactory);
        public abstract Task<bool> HandleAsync(EmailModel emailModel);
    }
}

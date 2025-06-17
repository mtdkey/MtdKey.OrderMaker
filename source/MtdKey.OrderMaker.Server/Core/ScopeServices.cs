using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace MtdKey.OrderMaker.Core
{
    public static class ScopeServices
    {
        public static void AddCoreServices(this IServiceCollection services)
        {
            services.TryAddScoped<IStoreService, StoreService>();
        }
    }
}

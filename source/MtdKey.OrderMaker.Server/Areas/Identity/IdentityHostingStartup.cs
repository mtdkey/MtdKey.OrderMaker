using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(MtdKey.OrderMaker.Areas.Identity.IdentityHostingStartup))]
namespace MtdKey.OrderMaker.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
            });
        }
    }
}
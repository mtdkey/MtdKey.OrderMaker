using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System.IO;
using System.Reflection;

namespace MtdKey.OrderMaker
{
    public class Program
    {
        public static string TemplateConnectionStaring { get; set; }

        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();           
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            //.UseContentRoot(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location))
                .UseStartup<Startup>();
    }
}

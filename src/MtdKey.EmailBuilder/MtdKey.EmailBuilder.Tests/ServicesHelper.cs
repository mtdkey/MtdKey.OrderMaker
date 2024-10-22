using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Microsoft.DependencyInjection.Abstracts;
using Xunit.Microsoft.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;


namespace MtdKey.EmailBuilder.Tests
{
  
    public class ServicesHelper : TestBedFixture
    {
        protected override void AddServices(IServiceCollection services, IConfiguration? configuration) 
            => services
            .AddTransient<ITemplateStorage, FileTemplates>()
            .Configure<AppOptions>(config => configuration?.GetSection("Options")
            .Bind(config));

        protected override ValueTask DisposeAsyncCore()
            => new();

        protected override IEnumerable<TestAppSettings> GetTestAppSettings()
        {
            yield return new() { Filename = "appsettings.json", IsOptional = false };
        }
    }
}

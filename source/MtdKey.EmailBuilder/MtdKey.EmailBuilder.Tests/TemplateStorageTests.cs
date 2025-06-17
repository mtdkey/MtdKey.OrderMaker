using System.Reflection;
using Xunit.Abstractions;
using Xunit.Microsoft.DependencyInjection.Abstracts;

namespace MtdKey.EmailBuilder.Tests
{
  
    public class TemplateStorageTests(ITestOutputHelper testOutputHelper, ServicesHelper fixture) 
        : TestBed<ServicesHelper>(testOutputHelper, fixture)        
    {
        [Fact]
        public async Task Get_All_EmailTemplates_By_Name()
        {
            var templateStorage = _fixture.GetService<ITemplateStorage>(_testOutputHelper)!;
            EmailTemplate emailTemplates = new("");
            var properties = emailTemplates.GetType().GetProperties(BindingFlags.Public | BindingFlags.Static);
            foreach(var property in properties)
            {
                EmailTemplate emailTemplate = property.GetValue(null) as EmailTemplate ?? new EmailTemplate("");
                var template = await templateStorage.GetTemplateAsync(emailTemplate);
                Assert.True(template != string.Empty, $"Property name: {property.Name}");
            }
        }       

    }
}
using System.Reflection;


namespace MtdKey.EmailBuilder
{
    public class FileTemplates : ITemplateStorage
    {
        public async Task<string> GetTemplateAsync(EmailTemplate name)
        {
            Assembly? assembly = Assembly.GetExecutingAssembly();
            if (assembly == null) { return string.Empty; }
            using Stream? stream = assembly.GetManifestResourceStream($"MtdKey.EmailBuilder.TemplateStorage.{name}.html");
            if (stream == null) { return string.Empty; }
            using StreamReader reader = new(stream);
            string template = await reader.ReadToEndAsync();

            return template;
        }
    }
}

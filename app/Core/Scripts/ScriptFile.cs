using System.IO;
using System.Reflection;

namespace MtdKey.OrderMaker.Core.Scripts
{

    public class ScriptFile
    {
        private string _script;
        private readonly IScriptFile scriptFile;

        public string Script => GetScript();

        public ScriptFile(IScriptFile scriptFile)
        {
            this.scriptFile = scriptFile;
        }

        private string GetScriptFormFile()
        {
            var assembly = Assembly.GetExecutingAssembly();
            using Stream stream = assembly.GetManifestResourceStream(scriptFile.ResourceName);
            using StreamReader reader = new(stream);
            string sqlScript = reader.ReadToEnd();
            return sqlScript;
        }

        private string GetScript()
        {
            _script ??= GetScriptFormFile();
            return _script;
        }

    }
}

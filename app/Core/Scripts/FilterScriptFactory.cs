using System.Collections;
using System.Linq;

namespace MtdKey.OrderMaker.Core.Scripts
{
    public class FilterScriptFactory : ScriptFile
    {
         
        private readonly IScriptFile scriptFile;
        private readonly FilterSQLparams filter;

        public FilterScriptFactory(IScriptFile scriptFile, FilterSQLparams filter) :base(scriptFile) {
            this.scriptFile = scriptFile;
            this.filter = filter;
        }
        
        public string BuildScript()
        {
            var script = Script;

            scriptFile.FilterHandlers.ToList().ForEach(handler =>
            {
                script = handler.ReplaceFilter(script, filter);
            });

            return script;
        }
    }
}

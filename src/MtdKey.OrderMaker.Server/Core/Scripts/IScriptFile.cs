using System.Collections.Generic;

namespace MtdKey.OrderMaker.Core.Scripts
{
    public interface IScriptFile
    {
        string ResourceName { get; }
        IEnumerable<FilterHandler> FilterHandlers { get; }

    }
}

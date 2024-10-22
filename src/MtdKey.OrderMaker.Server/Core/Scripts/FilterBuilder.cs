using System.Collections.Generic;

namespace MtdKey.OrderMaker.Core.Scripts
{
    public class FilterBuilder
    {
        private readonly List<FilterHandler> filterHandlers = [];

        public FilterBuilder() { }
        public FilterBuilder(FilterBuilder filterBuilder)
            => this.filterHandlers = filterBuilder.filterHandlers;

        public FilterBuilder AddHandler(FilterHandler handler)
        {
            filterHandlers.Add(handler);
            return this;
        }

        public List<FilterHandler> Build()
        {
            return filterHandlers;
        }

    }
}

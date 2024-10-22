

namespace MtdKey.OrderMaker.Core.Models
{
    public abstract class DocFieldBase<T> where T : class
    {
        public abstract object Value { get; set; }
        public T GetValue()
        {
            return (T) Value;
        }
    }
}

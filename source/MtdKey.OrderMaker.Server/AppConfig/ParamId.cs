using System.Linq;
using System.Reflection;

namespace MtdKey.OrderMaker.AppConfig
{
    public class ParamId(int id)
    {
        private readonly int id = id;

        public static implicit operator int(ParamId constant) => constant.id;
        public static explicit operator ParamId(int constant) => new(constant);

        public static ParamId BarColor => new(1);
        public static ParamId IconColor => new(2);
        public static ParamId RegisterByPage => new(3);
        public static ParamId RegisterByAPI => new(4);
        public static ParamId RegisterAPIKey => new(5);
        public static ParamId RegisterPolicy => new(6);
        public static ParamId RegisterGroup => new(7);

        public override string ToString()
        {
            var items = this.GetType().GetProperties(BindingFlags.Public | BindingFlags.Static)
                .ToDictionary(x => x.Name, x => (ParamId) x.GetValue(null, null));            
            var result = items.FirstOrDefault(x => x.Value.id == id);
            return result.Key;
        }

    }
}

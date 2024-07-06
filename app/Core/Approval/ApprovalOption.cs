using MtdKey.OrderMaker.AppConfig;
using System.Linq;
using System.Reflection;

namespace MtdKey.OrderMaker.Core.Approval
{
    public class ApprovalOption(int id)
    {
        private readonly int id = id;

        public static implicit operator int(ApprovalOption constant) => constant.id;
        public static explicit operator ApprovalOption(int constant) => new(constant);

        public override string ToString()
        {
            var items = this.GetType().GetProperties(BindingFlags.Public | BindingFlags.Static)
                .ToDictionary(x => x.Name, x => (ApprovalOption)x.GetValue(null, null));
            var result = items.FirstOrDefault(x => x.Value.id == id);
            return result.Key;
        }

        public static ApprovalOption Comment => new(1);
        public static ApprovalOption ResolutionId => new(2);
        public static ApprovalOption CurrentUserId => new(3);
        public static ApprovalOption DocOwnerUserId => new(4);
        public static ApprovalOption ServerHost => new(5);

    }
}

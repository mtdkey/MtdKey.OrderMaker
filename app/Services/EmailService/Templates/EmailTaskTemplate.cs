using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MtdKey.OrderMaker.Services.EmailService.Templates
{
    public class EmailTaskTemplate(int id)
    {
        private readonly int id = id;

        public static implicit operator int(EmailTaskTemplate constant) => constant.id;
        public static explicit operator EmailTaskTemplate(int constant) => new(constant);

        public override string ToString()
        {
            var items = this.GetType().GetProperties(BindingFlags.Public | BindingFlags.Static)
                .ToDictionary(x => x.Name, x => (EmailTaskTemplate)x.GetValue(null, null));
            var result = items.FirstOrDefault(x => x.Value.id == id);
            return result.Key;
        }
        
        public EmailTaskDesigner GetDesigner(EmailTaskTemplate template)
        {
            return Items[template];
        }

        public static readonly Dictionary<int, EmailTaskDesigner> Items = new()
        {
            {StartApproval, new StartApprovalDesigner()}
        };

        public static EmailTaskTemplate StartApproval => new(1);
    }
}

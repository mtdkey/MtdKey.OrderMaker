using MtdKey.OrderMaker.Areas.Identity.Data;

namespace MtdKey.OrderMaker.Core
{
    public static class UserExtensions
    {
        public static string GetFullName(this WebAppUser user)
        {
            string name = user.Title ?? "No Name";
            string group = user.TitleGroup ?? "";
            if (user.TitleGroup != null && user.TitleGroup.Length > 1) { group = $"({group})"; }

            return $"{name} {group}";
        }
    }
}

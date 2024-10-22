using System;

namespace MtdKey.OrderMaker.Areas.Identity.Pages.Users.Registration
{
    public class ParamsModel
    {
        public string RegisterByPage { get; set; } = string.Empty;
        public string RegisterByPageTitle { get; set; } = "TURNED OFF";
        public string RegisterByAPI { get; set; } = string.Empty;
        public string RegisterByAPITitle { get; set; } = "TURNED OFF";
        public string RegisterAPIKey { get; set; } = "3gd15jri5wa5f01nbaump5eygaddhaww5w6";
        public string RegisterUserPolicyId {  get; set; } = string.Empty;
        public string RegisterUserPolicyName { get; set; } = string.Empty;
        public string RegisterUserGroupId { get; set;} = string.Empty;
        public string RegisterUserGroupName { get; set; } = string.Empty;

    }
}

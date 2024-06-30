using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Security.Claims;

namespace MtdKey.OrderMaker.Core
{
    public class StorePostRequest
    {
        public string FormId { get; set; }
        public string StoreId { get; set; }
        public string DateCreated { get; set; }
        public Dictionary<string, string> Fields { get; set; }
        public IFormFileCollection Files { set; get; }
        public ClaimsPrincipal UserPrincipal { get; set; }
        public Dictionary<string, string> DeleteFields { get; set; } = [];
        public ActionTypeRequest ActionType { get; set; } = ActionTypeRequest.Show;
    }
}

using System.Security.Claims;

namespace MtdKey.OrderMaker.Core
{
    public enum ActionTypeRequest
    {
       Create, Edit, Show
    }

    public class StoreDocRequest
    {
        public string FormId { get; set; }
        public string StoreId { get; set; } = string.Empty;
        public bool UseFilter { get; set; } = false;        
        public ClaimsPrincipal UserPrincipal { get; set; }
        public ActionTypeRequest ActionTypeRequest { get; set; } = ActionTypeRequest.Show;
        public bool LimitRequest { get; set; } = false;
    }
}

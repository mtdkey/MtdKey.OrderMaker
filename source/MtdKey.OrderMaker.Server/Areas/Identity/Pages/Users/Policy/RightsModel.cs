namespace MtdKey.OrderMaker.Areas.Identity.Pages.Users.Policy
{
    public class RightsModel
    {
        public string FormId { get; set; }
        public bool Create { get; set; }
        public bool ViewAll { get; set; }
        public bool ViewGroup { get; set; }
        public bool ViewOwn { get; set; }
        public bool EditAll { get; set; }
        public bool EditGroup { get; set; }
        public bool EditOwn { get; set; }

        public bool DeleteAll { get; set; }
        public bool DeleteGroup { get; set; }
        public bool DeleteOwn { get; set; }
        public bool OwnDenyGroup { get; set; }
        public bool SetOwner { get; set; }
        public bool Reviewer { get; set; }
        public bool SetDate { get; set; }
        public bool ExportToExcel { get; set; }
        public bool RelatedCreate { get; set; }
        public bool RelatedEdit { get; set; }

    }
}

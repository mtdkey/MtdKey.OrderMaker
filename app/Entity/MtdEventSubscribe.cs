namespace MtdKey.OrderMaker.Entity
{
    public class MtdEventSubscribe
    {
        public string Id { get; set; }
        public string MtdFormId { get; set; }
        public string UserId { get; set; }

        public sbyte EventCreate { get; set; }
        public sbyte EventEdit { get; set; }

        public virtual MtdForm MtdForm { get; set; }

    }

}

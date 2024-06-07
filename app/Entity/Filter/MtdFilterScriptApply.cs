namespace MtdKey.OrderMaker.Entity
{
    public class MtdFilterScriptApply
    {
        public string Id { get; set; }
        public int MtdFilterId { get; set; }
        public int MtdFilterScriptId { get; set; }

        public virtual MtdFilter MtdFilter { get; set; }
        public virtual MtdFilterScript MtdFilterScript { get; set; }
    }
}

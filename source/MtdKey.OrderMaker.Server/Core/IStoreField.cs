namespace MtdKey.OrderMaker.Core
{
    public interface IStoreField
    {
        string FieldId { get; set; }
        bool IsDeleted { get; set; }
    }
}

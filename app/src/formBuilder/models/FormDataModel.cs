namespace MtdKey.OrderMaker
{
    public class FormDataModel
    {
        public FormModel FormModel { get; set; }
        public PartModel[] PartModels { get; set; }
        public FieldModel[] FieldModels { get; set; }
        public FormInfoModel[] FormInfoModels { get; set; }
    }
}

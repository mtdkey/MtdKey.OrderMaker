using MtdKey.OrderMaker.Models.Controls.MTDSelectList;
using System.Collections.Generic;

namespace MtdKey.OrderMaker.Areas.Config.Pages.EmailLoader
{
    public class FormEditorModel
    {
        public List<MTDSelectListItem> FormItems { get; set; } = [];
        public List<MTDSelectListItem> FieldItems { get; set; } = [];
        public List<MTDSelectListItem> MediatorItems { get; set; } = [];
        public List<MTDSelectListItem> LocationItems { get; set; } = [];

        public string FormId { get; set; }
        public string SenderId { get; set; }
        public string SubjectId { get; set; }
        public string BodyId { get; set; }
        public string AttachmentsId { get; set; }
        public string EmailAsKey { get; set; }
        public string KeyWord { get; set;}
    }
}

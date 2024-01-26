using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MtdKey.OrderMaker.Areas.Workplace.Pages.Store.Models
{
    public class ApprovalLog
    {
        public byte[] ImgData { get; set; }
        public string ImgType { get; set; }
        public string UserName { get; set; }
        public string Note { get; set; }
        public DateTime Time { get; set; }
        public int Result { get; set; }
        public string Color { get; set; }
        public string Comment { get; set; }
        public bool IsSign { get; set; }
        public string UserRecipient { get; set; }

    }
}

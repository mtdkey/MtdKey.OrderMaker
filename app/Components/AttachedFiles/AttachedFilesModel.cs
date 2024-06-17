using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MtdKey.OrderMaker.Components.AttachedFiles
{
    public class AttachedFilesModel
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; } = "Name";
        public string Label { get; set; } = "Label";
        public int MaxSize { get; set; } = 4000000;
        public AttachedFilesView AttachedFilesView { get; set; } = AttachedFilesView.Default;
        public List<AttachedFile> Files { get; set; } = new();
    }
}

using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MtdKey.OrderMaker.Components.AttachedFiles
{
    public class AttachedFiles : ViewComponent
    {
        public Task<IViewComponentResult> InvokeAsync(AttachedFilesModel model)
        {
            model ??= new AttachedFilesModel();
            return Task.FromResult<IViewComponentResult>(View(model.AttachedFilesView.ToString(), model));
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MtdKey.OrderMaker.Components.ToastContainer
{
    public class ToastContainer : ViewComponent
    {
        public Task<IViewComponentResult> InvokeAsync(ToastContainerModel model)
        {
            model ??= new ToastContainerModel();
            return Task.FromResult<IViewComponentResult>(View("Default", model));
        }
    }
}

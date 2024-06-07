/*
    MTD OrderMaker - http://mtdkey.com
    Copyright (c) 2019 Oleg Bruev <job4bruev@gmail.com>. All rights reserved.
*/

using Microsoft.AspNetCore.Mvc;
using MtdKey.OrderMaker.Core;
using MtdKey.OrderMaker.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace MtdKey.OrderMaker.Components.Store
{
    public enum FormType { Create, Edit, Details, Print }

    [ViewComponent(Name = "StoreForm")]
    public class Form : ViewComponent
    {

        private readonly IStoreService storeService;

        public Form(IStoreService storeService)
        {
            this.storeService = storeService;
        }

        public async Task<IViewComponentResult> InvokeAsync(MtdStore store, FormType type = FormType.Details)
        {

            if (store == null)
            {
                return View();
            }

            if (type == FormType.Create)
            {
                var docModel = await storeService.CreateEmptyDocModelAsync(new()
                {
                    ActionTypeRequest = ActionTypeRequest.Create,
                    FormId = store.MtdFormId,
                    StoreId = store.Id,
                    UserPrincipal = HttpContext.User
                });

                return View(type.ToString(), docModel);
            }

            var actionType = type == FormType.Details || type == FormType.Print ? ActionTypeRequest.Show : ActionTypeRequest.Show;
            if (type == FormType.Edit) actionType = ActionTypeRequest.Edit;
            if (type == FormType.Create) actionType = ActionTypeRequest.Create;

            var requestResult = await storeService.GetDocsBySQLRequestAsync(new()
            {
                FormId = store.MtdFormId,
                StoreId = store.Id,
                UserPrincipal = HttpContext.User,
                ActionTypeRequest = actionType
            });

            return View(type.ToString(), requestResult.Docs.FirstOrDefault() ?? new());

        }


    }
}

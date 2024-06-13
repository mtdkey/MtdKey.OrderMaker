using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;
using MtdKey.OrderMaker.AppConfig;
using MtdKey.OrderMaker.Models.Controls.MTDSelectList;
using MtdKey.OrderMaker.Services;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace MtdKey.OrderMaker.Areas.Identity.Pages.Users.Registration
{
    public class IndexModel(IAppParams appParams, UserHandler userHandler, IStringLocalizer<SharedResource> localizer) : PageModel
    {
        private readonly IAppParams _appParams = appParams;
        private readonly UserHandler _userHandler = userHandler;
        private readonly IStringLocalizer<SharedResource> _localizer=localizer;

        public ParamsModel ParamsModel { get; set; } = new();
        public List<MTDSelectListItem> UserPolicies { get; set; } = [];
        public List<MTDSelectListItem> UserGroups { get; set; } = [];

        public async Task<IActionResult> OnGetAsync()
        {
            ParamsModel.RegisterByPage = await _appParams.GetValueAsync(ParamId.RegisterByPage) == "true" ? "checked" : string.Empty;
            ParamsModel.RegisterByAPI = await _appParams.GetValueAsync(ParamId.RegisterByAPI) == "true" ? "checked" : string.Empty;
            ParamsModel.RegisterAPIKey = await _appParams.GetValueAsync(ParamId.RegisterAPIKey);
            ParamsModel.RegisterUserPolicyId = await _appParams.GetValueAsync(ParamId.RegisterPolicy);
            ParamsModel.RegisterUserGroupId = await _appParams.GetValueAsync(ParamId.RegisterGroup);

            ParamsModel.RegisterByPageTitle = ParamsModel.RegisterByPage == "checked" ?  _localizer["TURN ON"] : _localizer["TURN OFF"];
            ParamsModel.RegisterByAPITitle = ParamsModel.RegisterByAPI == "checked" ? _localizer["TURN ON"] : _localizer["TURN OFF"];

            var policies = await _userHandler.GetPoliciesAsync();      

            foreach (var policy in policies)
                UserPolicies.Add(new() { Id = policy.Id, Value = policy.Name, Selectded = policy.Id==ParamsModel.RegisterUserPolicyId });

            ParamsModel.RegisterUserPolicyName = UserPolicies.Where(x => x.Selectded).Select(x => x.Value).FirstOrDefault() ?? _localizer["Not Selected"];

            var groups = await _userHandler.GetAllGroups();
            foreach (var group in groups)
                UserGroups.Add(new() { Id = group.Id, Value=group.Name, Selectded = group.Id == ParamsModel.RegisterUserGroupId });

            ParamsModel.RegisterUserGroupName = UserGroups.Where(x => x.Selectded).Select(x => x.Value).FirstOrDefault() ?? _localizer["Not Selected"];

            return Page();
        }

        public async Task<IActionResult> OnPostAsync() {
            var form = await Request.ReadFormAsync();
            var registerByPage = form["registerByPage"];
            var registerByAPI = form["registerByAPI"];
            var registerAPIKey = form["registerAPIKey"];
            var registerPolicyId = form["RegisterUserPolicyId"];

            await _appParams.SaveAsync(ParamId.RegisterByPage, registerByPage);
            await _appParams.SaveAsync(ParamId.RegisterByAPI, registerByAPI);
            await _appParams.SaveAsync(ParamId.RegisterAPIKey, registerAPIKey);
            await _appParams.SaveAsync(ParamId.RegisterPolicy, registerPolicyId);

            return RedirectToPage("./Index");
        }
    }
}

using Abp.AspNetCore.Mvc.Controllers;

namespace TaobaoAuthorization.Web.Controllers
{
    public abstract class TaobaoAuthorizationControllerBase: AbpController
    {
        protected TaobaoAuthorizationControllerBase()
        {
            LocalizationSourceName = TaobaoAuthorizationConsts.LocalizationSourceName;
        }
    }
}
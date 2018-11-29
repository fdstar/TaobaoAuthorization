using Abp.AspNetCore.Mvc.Views;

namespace TaobaoAuthorization.Web.Views
{
    public abstract class TaobaoAuthorizationRazorPage<TModel> : AbpRazorPage<TModel>
    {
        protected TaobaoAuthorizationRazorPage()
        {
            LocalizationSourceName = TaobaoAuthorizationConsts.LocalizationSourceName;
        }
    }
}

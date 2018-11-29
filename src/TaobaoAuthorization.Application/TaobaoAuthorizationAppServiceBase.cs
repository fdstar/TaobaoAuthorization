using Abp.Application.Services;

namespace TaobaoAuthorization
{
    /// <summary>
    /// Derive your application services from this class.
    /// </summary>
    public abstract class TaobaoAuthorizationAppServiceBase : ApplicationService
    {
        protected TaobaoAuthorizationAppServiceBase()
        {
            LocalizationSourceName = TaobaoAuthorizationConsts.LocalizationSourceName;
        }
    }
}
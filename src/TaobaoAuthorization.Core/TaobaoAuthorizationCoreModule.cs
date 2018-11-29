using Abp.Modules;
using Abp.Reflection.Extensions;
using TaobaoAuthorization.Localization;

namespace TaobaoAuthorization
{
    public class TaobaoAuthorizationCoreModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Auditing.IsEnabledForAnonymousUsers = true;

            TaobaoAuthorizationLocalizationConfigurer.Configure(Configuration.Localization);
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(TaobaoAuthorizationCoreModule).GetAssembly());
        }
    }
}
using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;

namespace TaobaoAuthorization
{
    [DependsOn(
        typeof(TaobaoAuthorizationCoreModule), 
        typeof(AbpAutoMapperModule))]
    public class TaobaoAuthorizationApplicationModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(TaobaoAuthorizationApplicationModule).GetAssembly());
        }
    }
}
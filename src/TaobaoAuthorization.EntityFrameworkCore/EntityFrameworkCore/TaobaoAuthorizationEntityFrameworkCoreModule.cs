using Abp.EntityFrameworkCore;
using Abp.Modules;
using Abp.Reflection.Extensions;

namespace TaobaoAuthorization.EntityFrameworkCore
{
    [DependsOn(
        typeof(TaobaoAuthorizationCoreModule), 
        typeof(AbpEntityFrameworkCoreModule))]
    public class TaobaoAuthorizationEntityFrameworkCoreModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(TaobaoAuthorizationEntityFrameworkCoreModule).GetAssembly());
        }
    }
}
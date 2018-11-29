using Abp.AspNetCore;
using Abp.AspNetCore.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using TaobaoAuthorization.Configuration;
using TaobaoAuthorization.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace TaobaoAuthorization.Web.Startup
{
    [DependsOn(
        typeof(TaobaoAuthorizationApplicationModule), 
        typeof(TaobaoAuthorizationEntityFrameworkCoreModule), 
        typeof(AbpAspNetCoreModule))]
    public class TaobaoAuthorizationWebModule : AbpModule
    {
        private readonly IConfigurationRoot _appConfiguration;

        public TaobaoAuthorizationWebModule(IHostingEnvironment env)
        {
            _appConfiguration = AppConfigurations.Get(env.ContentRootPath, env.EnvironmentName);
        }

        public override void PreInitialize()
        {
            Configuration.DefaultNameOrConnectionString = _appConfiguration.GetConnectionString(TaobaoAuthorizationConsts.ConnectionStringName);

            Configuration.Navigation.Providers.Add<TaobaoAuthorizationNavigationProvider>();

            Configuration.Modules.AbpAspNetCore()
                .CreateControllersForAppServices(
                    typeof(TaobaoAuthorizationApplicationModule).GetAssembly()
                );
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(TaobaoAuthorizationWebModule).GetAssembly());
        }
    }
}
using Abp.Configuration.Startup;
using Abp.Domain.Uow;
using Abp.EntityFrameworkCore;
using Abp.EntityFrameworkCore.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;

namespace TaobaoAuthorization.EntityFrameworkCore
{
    [DependsOn(
        typeof(TaobaoAuthorizationCoreModule), 
        typeof(AbpEntityFrameworkCoreModule))]
    public class TaobaoAuthorizationEntityFrameworkCoreModule : AbpModule
    {
        public override void PreInitialize()
        {
            base.PreInitialize();
            Configuration.ReplaceService<IConnectionStringResolver, MyConnectionStringResolver>();
            this.AddDbContext<TaobaoAuthorizationDbContext>();
            this.AddDbContext<TaobaoAuthorizedDbContext>();
        }
        private void AddDbContext<TDbContext>()
            where TDbContext: AbpDbContext
        {
            Configuration.Modules.AbpEfCore().AddDbContext<TDbContext>(options =>
            {
                if (options.ExistingConnection != null)
                {
                    DbContextOptionsConfigurer.Configure(options.DbContextOptions, options.ExistingConnection);
                }
                else
                {
                    DbContextOptionsConfigurer.Configure(options.DbContextOptions, options.ConnectionString);
                }
            });
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(TaobaoAuthorizationEntityFrameworkCoreModule).GetAssembly());
        }
    }
}
using TaobaoAuthorization.Configuration;
using TaobaoAuthorization.Web;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace TaobaoAuthorization.EntityFrameworkCore
{
    /* This class is needed to run EF Core PMC commands. Not used anywhere else */
    public class TaobaoAuthorizationDbContextFactory : IDesignTimeDbContextFactory<TaobaoAuthorizationDbContext>
    {
        public TaobaoAuthorizationDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<TaobaoAuthorizationDbContext>();
            var configuration = AppConfigurations.Get(WebContentDirectoryFinder.CalculateContentRootFolder());

            DbContextOptionsConfigurer.Configure(
                builder,
                configuration.GetConnectionString(TaobaoAuthorizationConsts.ConnectionStringName)
            );

            return new TaobaoAuthorizationDbContext(builder.Options);
        }
    }
}
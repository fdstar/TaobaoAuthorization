using Microsoft.EntityFrameworkCore;

namespace TaobaoAuthorization.EntityFrameworkCore
{
    public static class DbContextOptionsConfigurer
    {
        public static void Configure(
            DbContextOptionsBuilder<TaobaoAuthorizationDbContext> dbContextOptions, 
            string connectionString
            )
        {
            /* This is the single point to configure DbContextOptions for TaobaoAuthorizationDbContext */
            //dbContextOptions.UseSqlServer(connectionString);
            dbContextOptions.UseMySql(connectionString);
        }
    }
}

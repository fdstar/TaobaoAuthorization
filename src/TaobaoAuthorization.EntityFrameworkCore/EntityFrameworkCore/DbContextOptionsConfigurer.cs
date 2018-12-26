using Abp.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace TaobaoAuthorization.EntityFrameworkCore
{
    public static class DbContextOptionsConfigurer
    {
        public static void Configure<T>(
            DbContextOptionsBuilder<T> dbContextOptions,
            string connectionString
            )
            where T : AbpDbContext
        {
            /* This is the single point to configure DbContextOptions for TaobaoAuthorizationDbContext */
            //dbContextOptions.UseSqlServer(connectionString);
            dbContextOptions.UseMySql(connectionString);
        }

        public static void Configure<T>(
            DbContextOptionsBuilder<T> dbContextOptions,
            DbConnection connection
            )
            where T : AbpDbContext
        {
            /* This is the single point to configure DbContextOptions for TaobaoAuthorizationDbContext */
            //dbContextOptions.UseSqlServer(connectionString);
            dbContextOptions.UseMySql(connection);
        }
    }
}

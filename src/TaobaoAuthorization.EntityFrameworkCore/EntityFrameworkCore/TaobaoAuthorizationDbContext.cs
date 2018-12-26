using Abp.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TaobaoAuthorization.Authorizations;
using TaobaoAuthorization.Partners;

namespace TaobaoAuthorization.EntityFrameworkCore
{
    public class TaobaoAuthorizationDbContext : AbpDbContext
    {
        //Add DbSet properties for your entities...
        public DbSet<AuthOrder> AuthOrders { get; set; }
        public DbSet<Partner> Partners { get; set; }

        public TaobaoAuthorizationDbContext(DbContextOptions<TaobaoAuthorizationDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Partner>().HasIndex("PartnerKey").IsUnique();
            modelBuilder.Entity<AuthOrder>().HasIndex("PartnerId", "AppKey", "AuthState").IsUnique();
        }
    }
    public class TaobaoAuthorizedDbContext : AbpDbContext
    {
        public DbSet<AuthorizedInfo> AuthorizedInfos { get; set; }

        public TaobaoAuthorizedDbContext(DbContextOptions<TaobaoAuthorizedDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<AuthorizedInfo>().HasIndex("AppKey", "AuthState").IsUnique();
        }
    }
}

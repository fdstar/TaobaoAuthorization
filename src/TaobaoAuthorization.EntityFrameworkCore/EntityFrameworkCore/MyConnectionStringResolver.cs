using Abp.Configuration.Startup;
using Abp.Domain.Uow;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using TaobaoAuthorization.Configuration;
using TaobaoAuthorization.Web;

namespace TaobaoAuthorization.EntityFrameworkCore
{
    public class MyConnectionStringResolver : DefaultConnectionStringResolver
    {
        public MyConnectionStringResolver(IAbpStartupConfiguration configuration)
            : base(configuration)
        {
        }

        public override string GetNameOrConnectionString(ConnectionStringResolveArgs args)
        {
            var connectStringName = this.GetConnectionStringName(args);
            if (connectStringName != null)
            {
                var configuration = AppConfigurations.Get(WebContentDirectoryFinder.CalculateContentRootFolder());
                return configuration.GetConnectionString(connectStringName);
            }
            return base.GetNameOrConnectionString(args);
        }
        private string GetConnectionStringName(ConnectionStringResolveArgs args)
        {
            var type = args["DbContextConcreteType"] as Type;
            if (type == typeof(TaobaoAuthorizedDbContext))
            {
                return TaobaoAuthorizationConsts.AuthorizedConnectionStringName;
            }
            return null;
        }
    }
}

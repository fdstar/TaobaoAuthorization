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
            if (args["DbContextConcreteType"] as Type == typeof(TaobaoAuthorizedDbContext))
            {
                var configuration = AppConfigurations.Get(WebContentDirectoryFinder.CalculateContentRootFolder());
                return configuration.GetConnectionString(TaobaoAuthorizationConsts.AuthorizedConnectionStringName);
            }

            return base.GetNameOrConnectionString(args);
        }
    }
}

using Abp.Application.Services;
using Abp.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TaobaoAuthorization.Authorizations.Dto;

namespace TaobaoAuthorization.Authorizations
{
    public class AuthorizedInfoAppService : AsyncCrudAppService<AuthorizedInfo, AuthorizedInfoDto, long, GetAuthorizedInfoFilterDto, CreateAuthorizedInfoDto, AuthorizedInfoDto>, IAuthorizedInfoAppService
    {
        public AuthorizedInfoAppService(IRepository<AuthorizedInfo, long> repository)
            : base(repository)
        {
            LocalizationSourceName = TaobaoAuthorizationConsts.LocalizationSourceName;
        }
        protected override IQueryable<AuthorizedInfo> CreateFilteredQuery(GetAuthorizedInfoFilterDto input)
        {
            var query = base.CreateFilteredQuery(input);
            if (input.AppKey.HasValue && input.AppKey > 0)
            {
                query = query.Where(o => o.AppKey == input.AppKey);
            }
            if (!string.IsNullOrWhiteSpace(input.AuthState))
            {
                query = query.Where(o => o.AuthState == input.AuthState);
            }
            return query;
        }
    }
}

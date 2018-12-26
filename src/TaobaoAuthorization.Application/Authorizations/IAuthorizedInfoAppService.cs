using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Text;
using TaobaoAuthorization.Authorizations.Dto;

namespace TaobaoAuthorization.Authorizations
{
    public interface IAuthorizedInfoAppService : IAsyncCrudAppService<AuthorizedInfoDto, long, GetAuthorizedInfoFilterDto, CreateAuthorizedInfoDto, AuthorizedInfoDto>
    {
    }
}

using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TaobaoAuthorization.Authorizations.Dto;

namespace TaobaoAuthorization.Authorizations
{
    public interface IAuthOrderAppService : IAsyncCrudAppService<AuthOrderDto, long, GetAuthOrderFilterDto, CreateAuthOrderInput, AuthOrderDto>
    {
        Task<AuthOrderDto> Get(GetAuthOrderInput input);
    }
}

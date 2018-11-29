using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.UI;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaobaoAuthorization.Authorizations.Dto;
using TaobaoAuthorization.Partners;

namespace TaobaoAuthorization.Authorizations
{
    [RemoteService(false)]
    public class AuthOrderAppService : AsyncCrudAppService<AuthOrder, AuthOrderDto, long, GetAuthOrderFilterDto, CreateAuthOrderInput, AuthOrderDto>, IAuthOrderAppService
    {
        private readonly IPartnerAppService _partnerAppService;
        public AuthOrderAppService(IRepository<AuthOrder, long> repository,
            IPartnerAppService partnerAppService)
            : base(repository)
        {
            this._partnerAppService = partnerAppService;
            LocalizationSourceName = TaobaoAuthorizationConsts.LocalizationSourceName;
        }

        public override async Task<AuthOrderDto> Create(CreateAuthOrderInput input)
        {
            Partner partner = null;
            if (await this._partnerAppService.Validate(input, (d, p) =>
            {
                partner = p;
                return null;
            }))
            {
                var order = await this.Repository.FirstOrDefaultAsync(o => o.PartnerId == partner.Id && o.AppKey == input.AppKey && o.AuthState == input.AuthState).ConfigureAwait(false);
                if (order == null)
                {
                    order = ObjectMapper.Map<AuthOrder>(input);
                    order.PartnerId = partner.Id;
                    order.AuthView = order.AuthView ?? string.Empty;
                    order.RedirectUri = order.RedirectUri ?? string.Empty;
                    var id = await this.Repository.InsertAndGetIdAsync(order);
                    order.Id = id;
                }
                else if (input.ForceReAuth.HasValue && input.ForceReAuth == 1)
                {
                    order.Error = string.Empty;
                    order.ErrorDescription = string.Empty;
                    order.TaobaoCode = string.Empty;
                    await this.Repository.UpdateAsync(order);
                }
                else if (!string.IsNullOrEmpty(order.TaobaoCode))
                {
                    throw new UserFriendlyException(L("AreadyAuthed"));
                }
                return ObjectMapper.Map<AuthOrderDto>(order);
            }
            throw new UserFriendlyException(L("VerifySignatureFail"));
        }

        public async Task<AuthOrderDto> Get(GetAuthOrderInput input)
        {
            Partner partner = null;
            if (await this._partnerAppService.Validate(input, (d, p) =>
            {
                partner = p;
                return null;
            }))
            {
                var order = await this.Repository.GetAll().AsNoTracking()
                    .FirstOrDefaultAsync(o => o.PartnerId == partner.Id && o.AppKey == input.AppKey && o.AuthState == input.AuthState).ConfigureAwait(false);
                if (order != null)
                {
                    return ObjectMapper.Map<AuthOrderDto>(order);
                }
                return null;
            }
            throw new ApplicationException(L("VerifySignatureFail"));
        }

        protected override IQueryable<AuthOrder> CreateFilteredQuery(GetAuthOrderFilterDto input)
        {
            var query = base.CreateFilteredQuery(input);
            query = query.Where(o => o.PartnerId == input.PartnerId);
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaobaoAuthorization.Partners.Dto;
using Abp.Domain.Repositories;
using Abp.Application.Services;
using System.Globalization;
using Abp.Authorization;
using Microsoft.Extensions.Options;
using Abp.UI;
using TaobaoAuthorization.Configuration;

namespace TaobaoAuthorization.Partners
{
    [RemoteService(false)]
    public class PartnerAppService : AsyncCrudAppService<Partner, PartnerDto, long, long, PartnerDto, PartnerDto>, IPartnerAppService
    {
        private readonly AppSettings _appSettings;
        private readonly IPartnerManager _partnerManager;
        public PartnerAppService(IPartnerManager partnerManager,
            IRepository<Partner, long> partnerRepository,
            IOptionsSnapshot<AppSettings> appSettings)
            : base(partnerRepository)
        {
            this._partnerManager = partnerManager;
            this._appSettings = appSettings.Value;
            LocalizationSourceName = TaobaoAuthorizationConsts.LocalizationSourceName;
        }
        [AbpAllowAnonymous]
        public async Task<bool> Validate<T>(T dto, Func<T, Partner, SortedDictionary<string, string>> func = null) where T : SignatureData
        {
            if (dto == null || !DateTime.TryParseExact(dto?.RequestTime, "yyyyMMddHHmmss", CultureInfo.CurrentCulture, DateTimeStyles.None, out DateTime requestTime))
            {
                throw new UserFriendlyException(L("ArgumentError"));
            }
#if !DEBUG
            var timeRange = _appSettings.SignTimeRange;
            if (requestTime < DateTime.Now.AddMinutes(-timeRange) || requestTime > DateTime.Now.AddMinutes(timeRange))
            {
                throw new UserFriendlyException(L("RequestTimeout"));
            }
#endif
            var partner = await this.Repository.FirstOrDefaultAsync(p => p.PartnerKey == dto.PartnerKey).ConfigureAwait(false);
            if (partner == null)
            {
                throw new UserFriendlyException(L("NoData"));
            }
            if (partner.IsDisabled)
            {
                throw new UserFriendlyException(L("PartnerDisabled"));
            }
            var dic = func?.Invoke(dto, partner);
            if (dic == null || dic.Count == 0)
            {
                dic = SignatureHelper.GetDictionary(dto);
            }
            return await this._partnerManager.VerifyData(partner, dic, dto.SignData, dto.SignType).ConfigureAwait(false);
        }
    }
}

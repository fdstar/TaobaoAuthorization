using Abp.Domain.Repositories;
using Abp.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaobaoAuthorization.Partners
{
    public class PartnerManager : DomainService, IPartnerManager
    {
        private readonly IRepository<Partner, long> _partnerRepository;
        public PartnerManager(IRepository<Partner, long> partnerRepository)
        {
            this._partnerRepository = partnerRepository;
            LocalizationSourceName = TaobaoAuthorizationConsts.LocalizationSourceName;
        }

        public async Task<string> GetSignData(string partnerKey, SortedDictionary<string, string> args, string algorithm = "SHA1", params string[] removeKeys)
        {
            var partner = await this.GetPartner(partnerKey).ConfigureAwait(false);
            return await this.GetSignData(partner, args, algorithm, removeKeys);
        }
        private async Task<Partner> GetPartner(string partnerKey)
        {
            var partner = await this._partnerRepository.FirstOrDefaultAsync(p => p.PartnerKey == partnerKey).ConfigureAwait(false);
            if (partner == null)
            {
                throw new ApplicationException(L("NoData"));
            }
            return partner;
        }

        public async Task<bool> VerifyData(string partnerId, SortedDictionary<string, string> args, string compareData, string algorithm = "SHA1", params string[] removeKeys)
        {
            var partner = await this.GetPartner(partnerId).ConfigureAwait(false);
            return await this.VerifyData(partner, args, compareData, algorithm, removeKeys).ConfigureAwait(false);
        }

        public Task<string> GetSignData(Partner partner, SortedDictionary<string, string> args, string algorithm = "SHA1", params string[] removeKeys)
        {
            return Task.FromResult(SignatureHelper.GetSignData(partner.SecretKey, args, algorithm, null, null, "UTF-8", removeKeys));
        }

        public Task<bool> VerifyData(Partner partner, SortedDictionary<string, string> args, string compareData, string algorithm = "SHA1", params string[] removeKeys)
        {
#if DEBUG
            return Task.FromResult(true);
#endif
            return Task.FromResult(SignatureHelper.VerifyData(partner.SecretKey, compareData, args, algorithm, null, null, "UTF-8", removeKeys));
        }
    }
}

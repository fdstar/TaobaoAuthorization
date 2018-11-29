using Abp.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaobaoAuthorization.Partners
{
    public interface IPartnerManager : IDomainService
    {
        Task<string> GetSignData(string partnerKey, SortedDictionary<string, string> args, string algorithm = "SHA1", params string[] removeKeys);
        Task<string> GetSignData(Partner partner, SortedDictionary<string, string> args, string algorithm = "SHA1", params string[] removeKeys);
        Task<bool> VerifyData(string partnerKey, SortedDictionary<string, string> args, string compareData, string algorithm = "SHA1", params string[] removeKeys);
        Task<bool> VerifyData(Partner partner, SortedDictionary<string, string> args, string compareData, string algorithm = "SHA1", params string[] removeKeys);
    }
}

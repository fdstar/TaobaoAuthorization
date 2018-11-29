using Abp.Application.Services;
using TaobaoAuthorization.Partners.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaobaoAuthorization.Partners
{
    public interface IPartnerAppService : IAsyncCrudAppService<PartnerDto, long, long, PartnerDto, PartnerDto>
    {
        /// <summary>
        /// 验证请求是否有效
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dto">请求参数</param>
        /// <param name="func">dto如何转化为Dictionary，如果不传则默认按属性获取，也可以通过该委托获取Partner但不返回dic，即返回null</param>
        /// <returns></returns>
        Task<bool> Validate<T>(T dto, Func<T, Partner, SortedDictionary<string, string>> func = null) where T : SignatureData;
    }
}

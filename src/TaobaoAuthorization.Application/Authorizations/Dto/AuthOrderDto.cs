using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaobaoAuthorization.Authorizations.Dto
{
    [AutoMapTo(typeof(AuthOrder))]
    public class AuthOrderDto : AuditedEntityDto<long>
    {
        /// <summary>
        /// Partner.Id
        /// </summary>
        [Required]
        public long PartnerId { get; set; }
        /// <summary>
        /// 要授权的Taobao应用Key
        /// </summary>
        [Required]
        public long AppKey { get; set; }
        /// <summary>
        /// 用于淘宝授权时传递的维持应用的状态值，可用于定位以及区分当前授权用户
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string AuthState { get; set; }
        /// <summary>
        /// Taobao授权页面样式，可选web、tmall或wap其中一种，默认为web
        /// </summary>
        [MaxLength(5)]
        public string AuthView { get; set; } = string.Empty;
        /// <summary>
        /// 淘宝授权成功后的跳转地址，如果不传则不跳转
        /// </summary>
        [MaxLength(150)]
        public string RedirectUri { get; set; } = string.Empty;
        /// <summary>
        /// 用户授权后淘宝返回的授权Code
        /// </summary>
        [MaxLength(100)]
        public string TaobaoCode { get; set; } = string.Empty;
        /// <summary>
        /// 错误码
        /// </summary>
        [MaxLength(20)]
        public string Error { get; set; } = string.Empty;
        /// <summary>
        /// 错误描述
        /// </summary>
        [MaxLength(200)]
        public string ErrorDescription { get; set; } = string.Empty;
    }
}

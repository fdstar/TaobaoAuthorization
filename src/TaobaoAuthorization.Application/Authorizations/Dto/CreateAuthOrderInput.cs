using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaobaoAuthorization.Partners.Dto;

namespace TaobaoAuthorization.Authorizations.Dto
{
    [AutoMapTo(typeof(AuthOrder))]
    public class CreateAuthOrderInput : SignatureData
    {
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
        public string AuthView { get; set; }
        /// <summary>
        /// 是否强制要求重新授权，1代表重新授权
        /// </summary>
        public byte? ForceReAuth { get; set; }
        /// <summary>
        /// 淘宝授权成功后的跳转地址，如果不传则不跳转
        /// </summary>
        [MaxLength(150)]
        public string RedirectUri { get; set; }
    }
}

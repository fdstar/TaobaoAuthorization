using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using TaobaoAuthorization.Partners.Dto;

namespace TaobaoAuthorization.Authorizations.Dto
{
    public class GetAuthOrderInput : SignatureData
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
    }
}

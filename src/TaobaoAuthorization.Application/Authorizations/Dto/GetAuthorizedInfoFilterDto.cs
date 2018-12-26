using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TaobaoAuthorization.Authorizations.Dto
{
    public class GetAuthorizedInfoFilterDto
    {
        /// <summary>
        /// 要授权的Taobao应用Key
        /// </summary>
        [Required]
        public long? AppKey { get; set; }
        /// <summary>
        /// 用于淘宝授权时传递的维持应用的状态值，可用于定位以及区分当前授权用户
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string AuthState { get; set; }
    }
}

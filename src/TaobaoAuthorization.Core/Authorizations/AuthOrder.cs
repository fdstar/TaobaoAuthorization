using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TaobaoAuthorization.Authorizations
{
    /// <summary>
    /// 授权请求信息
    /// </summary>
    public class AuthOrder : AuditedEntity<long>
    {
        /// <summary>
        /// Partner.Id
        /// </summary>
        [Required]
        [Column(Order = 2)]
        public long PartnerId { get; set; }
        /// <summary>
        /// 要授权的Taobao应用Key
        /// </summary>
        [Required]
        [Column(Order = 3)]
        public long AppKey { get; set; }
        /// <summary>
        /// 用于淘宝授权时传递的维持应用的状态值，可用于定位以及区分当前授权用户
        /// </summary>
        [Required]
        //[MaxLength(100)]
        [Column(Order = 4, TypeName = "varchar(100)")]
        public string AuthState { get; set; }
        /// <summary>
        /// Taobao授权页面样式，可选web、tmall或wap其中一种，默认为web
        /// </summary>
        //[MaxLength(5)]
        [Required]
        [Column(Order = 5, TypeName = "varchar(5)")]
        public string AuthView { get; set; } = string.Empty;
        /// <summary>
        /// 淘宝授权成功后的跳转地址，如果不传则不跳转
        /// </summary>
        [Required]
        [Column(Order = 6, TypeName = "varchar(150)")]
        public string RedirectUri { get; set; } = string.Empty;
        /// <summary>
        /// 用户授权后淘宝返回的授权Code
        /// </summary>
        //[MaxLength(100)]
        [Required]
        [Column(Order = 7, TypeName = "varchar(100)")]
        public string TaobaoCode { get; set; } = string.Empty;
        /// <summary>
        /// 错误码
        /// </summary>
        [Required]
        [MaxLength(20)]
        [Column(Order = 8)]
        public string Error { get; set; } = string.Empty;
        /// <summary>
        /// 错误描述
        /// </summary>
        [Required]
        [MaxLength(200)]
        [Column(Order = 9)]
        public string ErrorDescription { get; set; } = string.Empty;
    }
}

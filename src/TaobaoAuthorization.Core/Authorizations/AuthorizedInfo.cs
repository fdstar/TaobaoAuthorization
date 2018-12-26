using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TaobaoAuthorization.Authorizations
{
    /// <summary>
    /// 已授权信息
    /// </summary>
    public class AuthorizedInfo : AuditedEntity<long>
    {
        /// <summary>
        /// 淘宝应用Key
        /// </summary>
        [Required]
        [Column(Order = 3)]
        public long AppKey { get; set; }
        /// <summary>
        /// 用于淘宝授权时传递的维持应用的状态值，可用于定位以及区分当前授权用户
        /// </summary>
        [Required]
        [Column(Order = 4, TypeName = "varchar(100)")]
        public string AuthState { get; set; }
        /// <summary>
        /// 淘宝授权Key
        /// </summary>
        [Required]
        [Column(Order = 5, TypeName = "varchar(100)")]
        public string AccessToken { get; set; }
        /// <summary>
        /// 授权过期时间（本地时间）
        /// </summary>
        [Required]
        public DateTime ExpiresTime { get; set; }
    }
}

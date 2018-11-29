using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaobaoAuthorization.Partners
{
    /// <summary>
    /// 合作者信息
    /// </summary>
    public class Partner : AuditedEntity<long>
    {
        /// <summary>
        /// 合作者Key
        /// </summary>
        [Required]
        //[MaxLength(50)]
        [Column(Order =2, TypeName = "varchar(50)")]
        public string PartnerKey{ get; set; }
        /// <summary>
        /// 秘钥
        /// </summary>
        [Required]
        //[MaxLength(100)]
        [Column(Order = 3, TypeName = "varchar(100)")]
        public string SecretKey { get; set; }
        /// <summary>
        /// 合作者名称
        /// </summary>
        [Required]
        [MaxLength(100)]
        [Column(Order = 4)]
        public string PartnerName { get; set; }
        /// <summary>
        /// 是否禁用
        /// </summary>
        [Required]
        [Column(Order = 5)]
        public bool IsDisabled { get; set; }
    }
}

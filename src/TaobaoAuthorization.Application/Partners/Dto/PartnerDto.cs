using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaobaoAuthorization.Partners.Dto
{
    [AutoMapTo(typeof(Partner))]
    public class PartnerDto : EntityDto<long>
    {
        /// <summary>
        /// 合作者Key
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string PartnerKey { get; set; }
        /// <summary>
        /// 秘钥
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string SecretKey { get; set; }
        /// <summary>
        /// 合作者名称
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string PartnerName { get; set; }
        /// <summary>
        /// 是否禁用
        /// </summary>
        [Required]
        public bool IsDisabled { get; set; }
    }
}

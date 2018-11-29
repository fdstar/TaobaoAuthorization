using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaobaoAuthorization.Partners.Dto
{
    public class SignatureData
    {
        /// <summary>
        /// 合作者id
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string PartnerKey { get; set; }
        /// <summary>
        /// 请求时间,北京时间标准 yyyyMMddHHmmss格式
        /// </summary>
        [Required]
        public string RequestTime { get; set; }
        /// <summary>
        /// 签名方式
        /// </summary>
        public string SignType { get; set; } = "SHA1";
        /// <summary>
        /// 签名数据
        /// </summary>
        [Required]
        public string SignData { get; set; }
    }
}

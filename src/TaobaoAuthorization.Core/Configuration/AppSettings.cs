using System;
using System.Collections.Generic;
using System.Text;

namespace TaobaoAuthorization.Configuration
{
    public class AppSettings
    {
        /// <summary>
        /// 淘宝授权地址
        /// </summary>
        public string TaobaoOAuthUrl { get; set; }
        /// <summary>
        /// 淘宝RedirectUrl跳转的Host，测试期间需设置该值与淘宝应用上配置的域名一致
        /// </summary>
        public string RedirectHost { get; set; }
        /// <summary>
        /// 签名有效时间，单位分钟，默认5分钟
        /// </summary>
        public int SignTimeRange { get; set; } = 5;
        /// <summary>
        /// 对授权时传递给taobao的State值加密用的TripleDES秘钥
        /// </summary>
        public string TripleDESKeyForAuthState { get; set; }
        /// <summary>
        /// 获取到的淘宝Code过期时间，单位秒
        /// </summary>
        public int CodeExpiresIn { get; set; }
        /// <summary>
        /// 获取TripleDES秘钥对应的数组
        /// </summary>
        /// <returns></returns>
        public byte[] GetTripleDESKeyData()
        {
            return Convert.FromBase64String(this.TripleDESKeyForAuthState);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TaobaoAuthorization
{
    /// <summary>
    /// Hash签名辅助类
    /// </summary>
    public class HashSignatureHelper
    {
        /// <summary>
        /// 数据签名
        /// </summary>
        /// <param name="content">原始的字符串</param>
        /// <param name="algorithm">算法名称</param>
        /// <param name="encodingName">编码方式</param>
        /// <returns>base64格式字符串</returns>
        public static string SignData(string content, string algorithm = "SHA1", string encodingName = "UTF-8")
        {
            return Convert.ToBase64String(SignData(Encoding.GetEncoding(encodingName).GetBytes(content), algorithm));
        }
        /// <summary>
        /// 数据签名
        /// </summary>
        /// <param name="content">原始数据</param>
        /// <param name="algorithm">算法名称</param>
        /// <returns></returns>
        public static byte[] SignData(byte[] content, string algorithm = "SHA1")
        {
            return ComputeHash(content, algorithm);
        }
        private static byte[] ComputeHash(byte[] content, string algorithm)
        {
            switch (algorithm.ToUpper())
            {
                case "MD5":
                    return MD5Helper.ComputeHash(content);
                case "SHA256":
                    return SHA256Helper.ComputeHash(content);
                case "SHA384":
                    return SHA384Helper.ComputeHash(content);
                case "SHA512":
                    return SHA512Helper.ComputeHash(content);
                default:
                    return SHA1Helper.ComputeHash(content);
            }
        }
        /// <summary>
        /// 数据验签
        /// </summary>
        /// <param name="content">原始的字符串</param>
        /// <param name="signature">待校验的签名字符串 base64格式</param>
        /// <param name="algorithm">算法名称</param>
        /// <param name="encodingName">编码方式</param>
        /// <returns></returns>
        public static bool VerifyData(string content, string signature, string algorithm = "SHA1", string encodingName = "UTF-8")
        {
            return VerifyData(Encoding.GetEncoding(encodingName).GetBytes(content), signature, algorithm);
        }
        /// <summary>
        /// 数据验签
        /// </summary>
        /// <param name="content">原始数据</param>
        /// <param name="signature">待校验的签名数据</param>
        /// <param name="algorithm">算法名称</param>
        /// <returns></returns>
        public static bool VerifyData(byte[] content, byte[] signature, string algorithm = "SHA1")
        {
            return VerifyData(content, Convert.ToBase64String(signature), algorithm);
        }
        private static bool VerifyData(byte[] content, string signature, string algorithm)
        {
            return Convert.ToBase64String(ComputeHash(content, algorithm)) == signature;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace TaobaoAuthorization
{
    /// <summary>
    /// 哈希摘要算法帮助类
    /// </summary>
    /// <typeparam name="Algorithm"></typeparam>
    public class HashAlgorithmHelper<Algorithm>
        where Algorithm : HashAlgorithm, new()
    {
        /// <summary>
        /// Hash获取
        /// </summary>
        /// <param name="inputBuff"></param>
        /// <returns></returns>
        public static byte[] ComputeHash(byte[] inputBuff)
        {
            using (Algorithm alg = new Algorithm())
            {
                return alg.ComputeHash(inputBuff);
            }
        }
        /// <summary>
        /// 将输入的字符串以指定编码方式获取其字节数组，并将Hash后的数据以0X的方式返回
        /// </summary>
        /// <param name="inputString">要Hash的原始数据</param>
        /// <param name="encoding">inputString转化为byte所采用的编码方式，不传递则为utf-8</param>
        /// <param name="upper">是否返回大写，默认大写</param>
        /// <returns></returns>
        public static string HashOf(string inputString, Encoding encoding = null, bool upper = true)
        {
            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }
            var buff = ComputeHash(encoding.GetBytes(inputString));
            StringBuilder tmp = new StringBuilder();
            for (int i = 0; i < buff.Length; i++)
            {
                tmp.Append(buff[i].ToString(upper ? "X2" : "x2"));
            }
            return tmp.ToString();
        }
    }
    /// <summary>
    /// MD5Helper
    /// </summary>
    public class MD5Helper : HashAlgorithmHelper<MD5CryptoServiceProvider>
    {
    }
    /// <summary>
    /// SHA1Helper
    /// </summary>
    public class SHA1Helper : HashAlgorithmHelper<SHA1CryptoServiceProvider>
    {
    }
    /// <summary>
    /// SHA256Helper
    /// </summary>
    public class SHA256Helper : HashAlgorithmHelper<SHA256CryptoServiceProvider>
    {
    }
    /// <summary>
    /// SHA384Helper
    /// </summary>
    public class SHA384Helper : HashAlgorithmHelper<SHA384CryptoServiceProvider>
    {
    }
    /// <summary>
    /// SHA512Helper
    /// </summary>
    public class SHA512Helper : HashAlgorithmHelper<SHA512CryptoServiceProvider>
    {
    }
}

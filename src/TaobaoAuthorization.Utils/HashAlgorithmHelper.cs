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
            return ConvertToString(buff, upper);
        }
        /// <summary>
        /// 将byte数组转化为0X格式的字符串
        /// </summary>
        /// <param name="data"></param>
        /// <param name="upper"></param>
        /// <returns></returns>
        public static string ConvertToString(byte[] data, bool upper = true)
        {
            StringBuilder tmp = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                tmp.Append(data[i].ToString(upper ? "X2" : "x2"));
            }
            return tmp.ToString();
        }
        /// <summary>
        /// 将0x格式的字符串转化为byte数组
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static byte[] ConvertStringToByte(string str)
        {
            if (str == null || str.Length % 2 != 0)
            {
                throw new ArgumentException();
            }
            var data = new byte[str.Length / 2];
            for (var i = 0; i < data.Length; i++)
            {
                data[i] = Convert.ToByte(str.Substring(i * 2, 2), 16);
            }
            return data;
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

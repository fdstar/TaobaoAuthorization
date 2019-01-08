using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaobaoAuthorization
{
    /// <summary>
    /// 签名辅助类
    /// </summary>
    public class SignatureHelper
    {
        /// <summary>
        /// 默认签名时要移除的Key集合
        /// </summary>
        public static readonly string[] defaultRemoveKeysWhenSign = { "SignType", "SignData" };
        /// <summary>
        /// 将实体转换成字典（对于包含复杂对象的类不支持）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dto"></param>
        /// <returns></returns>
        public static SortedDictionary<string, string> GetDictionary<T>(T dto)
        {
            var props = dto.GetType().GetProperties();
            var dic = new SortedDictionary<string, string>();
            foreach (var p in props)
            {
                var value = p.GetValue(dto);
                if (value != null)
                {
                    dic.Add(p.Name, value.ToString());
                }
            }
            return dic;
        }
        /// <summary>
        /// 获取签名结果 base64格式的签名字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="secretKey">签名秘钥</param>
        /// <param name="dto">待签名的实体</param>
        /// <param name="dicFunc">将实体转化为字典的委托，如果不传递则默认使用<see cref="GetDictionary{T}(T)"/>方法进行转换</param>
        /// <param name="algorithm">算法名称，默认SHA1</param>
        /// <param name="valueFunc">签名时value部分如何转换委托，默认原样输出</param>
        /// <param name="signFunc">签名结果如何输出成字符串委托，默认转换成大写的16进制格式字符串</param>
        /// <param name="encodingName">字符串转换为byte[]时采用的编码，默认UTF-8</param>
        /// <param name="ignoreProps">签名时要忽略的属性名集合</param>
        /// <returns>返回结果由signFunc决定，默认转换成大写的16进制格式字符串</returns>
        public static string GetSignData<T>(string secretKey, T dto, Func<T, SortedDictionary<string, string>> dicFunc = null, string algorithm = "SHA1", Func<string, string> valueFunc = null, Func<byte[], string> signFunc = null, string encodingName = "UTF-8", params string[] ignoreProps)
        {
            var dic = dicFunc?.Invoke(dto);
            if (dic == null)
            {
                dic = GetDictionary(dto);
            }
            return GetSignData(secretKey, dic, algorithm, valueFunc, signFunc, encodingName, ignoreProps);
        }
        /// <summary>
        /// 获取签名结果 base64格式的签名字符串
        /// </summary>
        /// <param name="secretKey">签名秘钥</param>
        /// <param name="args">待签名的字典数据</param>
        /// <param name="algorithm">算法名称，默认SHA1</param>
        /// <param name="valueFunc">签名时value部分如何转换委托，默认原样输出</param>
        /// <param name="signFunc">签名结果如何输出成字符串委托，默认转换成大写的16进制格式字符串</param>
        /// <param name="encodingName">字符串转换为byte[]时采用的编码，默认UTF-8</param>
        /// <param name="removeKeys">不参与签名的Key集合，不传递则使用<see cref="defaultRemoveKeysWhenSign"/></param>
        /// <returns>返回结果由signFunc决定，默认转换成大写的16进制格式字符串</returns>
        public static string GetSignData(string secretKey, SortedDictionary<string, string> args, string algorithm = "SHA1", Func<string, string> valueFunc = null, Func<byte[], string> signFunc = null, string encodingName = "UTF-8", params string[] removeKeys)
        {
            StringBuilder tmp = new StringBuilder();
            if (removeKeys == null || removeKeys.Length == 0)
            {
                removeKeys = defaultRemoveKeysWhenSign;
            }
            foreach (var kv in args)
            {
                if (kv.Value == null || removeKeys.Any(k => k.Equals(kv.Key, StringComparison.OrdinalIgnoreCase)))
                {
                    continue;
                }
                tmp.Append('&');
                tmp.Append(kv.Key);
                tmp.Append('=');
                tmp.Append(valueFunc?.Invoke(kv.Value) ?? kv.Value);
            }
            if (tmp.Length > 0)
            {
                tmp = tmp.Remove(0, 1);
            }
            tmp.Append(secretKey);
            var data = HashSignatureHelper.SignData(Encoding.GetEncoding(encodingName).GetBytes(tmp.ToString()), algorithm);
            var retStr = signFunc?.Invoke(data);
            if (retStr == null)
            {
                retStr = SHA1Helper.ConvertToString(data);
            }
            return retStr;
        }
        /// <summary>
        /// 校验签名是否正确
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="secretKey">签名秘钥</param>
        /// <param name="compareData">要比较的签名字符串 base64格式</param>
        /// <param name="dto">待签名的实体</param>
        /// <param name="dicFunc">将实体转化为字典的委托，如果不传递则默认使用<see cref="GetDictionary{T}(T)"/>方法进行转换</param>
        /// <param name="algorithm">算法名称，默认SHA1</param>
        /// <param name="valueFunc">签名时value部分如何转换委托，默认原样输出</param>
        /// <param name="signFunc">签名结果如何输出成字符串委托，默认转换成大写的16进制格式字符串</param>
        /// <param name="encodingName">字符串转换为byte[]时采用的编码，默认UTF-8</param>
        /// <param name="ignoreProps">签名时要忽略的属性名集合</param>
        /// <returns></returns>
        public static bool VerifyData<T>(string secretKey, string compareData, T dto, Func<T, SortedDictionary<string, string>> dicFunc = null, string algorithm = "SHA1", Func<string, string> valueFunc = null, Func<byte[], string> signFunc = null, string encodingName = "UTF-8", params string[] ignoreProps)
        {
            return GetSignData(secretKey, dto, dicFunc, algorithm, valueFunc, signFunc, encodingName, ignoreProps).Equals(compareData, StringComparison.OrdinalIgnoreCase);
        }
        /// <summary>
        /// 校验签名是否正确
        /// </summary>
        /// <param name="secretKey">签名秘钥</param>
        /// <param name="compareData">要比较的签名字符串 base64格式</param>
        /// <param name="args">待签名的字典数据</param>
        /// <param name="algorithm">算法名称，默认SHA1</param>
        /// <param name="valueFunc">签名时value部分如何转换委托，默认原样输出</param>
        /// <param name="signFunc">签名结果如何输出成字符串委托，默认转换成大写的16进制格式字符串</param>
        /// <param name="encodingName">字符串转换为byte[]时采用的编码，默认UTF-8</param>
        /// <param name="removeKeys">不参与签名的Key集合，不传递则使用<see cref="defaultRemoveKeysWhenSign"/></param>
        /// <returns></returns>
        public static bool VerifyData(string secretKey, string compareData, SortedDictionary<string, string> args, string algorithm = "SHA1", Func<string, string> valueFunc = null, Func<byte[], string> signFunc = null, string encodingName = "UTF-8", params string[] removeKeys)
        {
            return GetSignData(secretKey, args, algorithm, valueFunc, signFunc, encodingName, removeKeys).Equals(compareData, StringComparison.OrdinalIgnoreCase);
        }
    }
}

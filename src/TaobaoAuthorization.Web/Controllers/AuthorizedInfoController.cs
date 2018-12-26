using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TaobaoAuthorization.Authorizations;
using TaobaoAuthorization.Authorizations.Dto;

namespace TaobaoAuthorization.Web.Controllers
{
    [Route("Authorized")]
    public class AuthorizedInfoController : TaobaoAuthorizationControllerBase
    {

        private string _url = "https://eco.taobao.com/router/rest";//淘宝授权地址
        private string _comAuthorizationUrl = "/Auth/Authorization";//统一授权地址,这里因为是例子放在了一起做测试，所以域名部分没加上
        private long _appKey = 25297011;//淘宝应用appKey
        private string _appSecret = "填入实际的秘钥";//淘宝应用秘钥
        private string _authState = "demoAuthState";//维持应用的状态值

        private string _partnerKey = "Partner001";//合作号Id
        private string _partnerSecret = "1234567890";//合作者秘钥
        private int _signTimeRange = 2;//签名允许时间差值

        const string taobaoAuthorizedRedirectUrlSessionKey = "taobaoAuthorizedRedirectUrlSessionKey";
        const int reduceSeconds = 60;//为保证授权时间有一定的缓存时间，在返回的有效时间范围上减去该部分值

        private readonly IAuthorizedInfoAppService _authorizedInfoAppService;
        public AuthorizedInfoController(IAuthorizedInfoAppService authorizedInfoAppService)
        {
            this._authorizedInfoAppService = authorizedInfoAppService;
        }
        /// <summary>
        /// 淘宝授权首页，如果淘宝授权已过期，那么会先跳转淘宝进行授权，授权通过后再跳转实际业务页面
        /// 如果不传递redirectUri，那么则仅仅是单独的授权
        /// </summary>
        /// <param name="redirectUri">实际业务需要跳转的页面</param>
        /// <param name="force">是否强制更新授权，非0代表强制授权</param>
        /// <returns></returns>
        [HttpGet("Index")]
        public async Task<IActionResult> Index(string redirectUri, byte force = 0)
        {
            var needAuth = force != 0;
            if (!needAuth)
            {
                var info = await this.GetAuthorizedInfo().ConfigureAwait(false);
                if (info == null || info.ExpiresTime <= DateTime.Now)
                {
                    needAuth = true;
                }
                ViewBag.ExpiresTime = info?.ExpiresTime.ToString("yyyy-MM-dd HH:mm:ss");
            }
            if (needAuth)
            {
                if (!string.IsNullOrWhiteSpace(redirectUri))
                {
                    HttpContext.Session.SetString(taobaoAuthorizedRedirectUrlSessionKey, redirectUri);
                }
                return Redirect(this.GetAuthorizationUri());
            }
            if (!string.IsNullOrWhiteSpace(redirectUri))
            {
                return Redirect(redirectUri);
            }
            return View();
        }
        private async Task<AuthorizedInfoDto> GetAuthorizedInfo()
        {
            var infos = await this._authorizedInfoAppService.GetAll(new GetAuthorizedInfoFilterDto
            {
                AppKey = this._appKey,
                AuthState = this._authState,
            });
            if (infos.TotalCount != 0 && infos.Items != null && infos.Items.Count > 0)
            {
                return infos.Items[0];
            }
            return null;
        }
        private string GetAuthorizationUri()
        {
            var uriHost = $"{Request.Scheme}://{Request.Host}/{Request.PathBase}".TrimEnd('/');
            var dic = new SortedDictionary<string, string>
            {
                { "PartnerKey",this._partnerKey},
                { "RequestTime",DateTime.Now.ToString("yyyyMMddHHmmss")},
                { "SignType","SHA1"},
                { "AppKey",this._appKey.ToString()},
                { "AuthState",this._authState},
                { "ForceReAuth","1"},
                { "RedirectUri",$"{uriHost}/Authorized/Authorized"}
            };
            var signData = SignatureHelper.GetSignData(this._partnerSecret, dic, dic["SignType"]);
            dic.Add("SignData", signData);
            var tmp = new StringBuilder();
            tmp.Append(this._comAuthorizationUrl);
            tmp.Append('?');
            foreach (var kv in dic)
            {
                tmp.Append(kv.Key);
                tmp.Append('=');
                tmp.Append(Uri.EscapeDataString(kv.Value));
                tmp.Append('&');
            }
            tmp.Remove(tmp.Length - 1, 1);
            return tmp.ToString();
        }
        /// <summary>
        /// 授权成功后的回调页面
        /// </summary>
        /// <returns></returns>
        [HttpGet("Authorized")]
        public async Task<IActionResult> Authorized()
        {
            var msg = "授权失败";
            if (this.ValidSignature())
            {
                this.GetTaobaoAccessToken(out string accessToken, out DateTime expiresTime);
                if (accessToken != null)
                {
                    var info = await this.GetAuthorizedInfo().ConfigureAwait(false);
                    if (info == null)
                    {//新增
                        var dto = new CreateAuthorizedInfoDto
                        {
                            AccessToken = accessToken,
                            AppKey = this._appKey,
                            AuthState = this._authState,
                            ExpiresTime = expiresTime
                        };
                        await this._authorizedInfoAppService.Create(dto);
                    }
                    else
                    {
                        info.AccessToken = accessToken;
                        info.ExpiresTime = expiresTime;
                        await this._authorizedInfoAppService.Update(info);
                    }
                    msg = "授权成功";
                    var redirectUri = HttpContext.Session.GetString(taobaoAuthorizedRedirectUrlSessionKey);
                    if (!string.IsNullOrWhiteSpace(redirectUri))
                    {
                        return Redirect(redirectUri);
                    }
                }
            }
            ViewBag.Message = msg;
            return View();
        }
        /// <summary>
        /// 实际获取淘宝授权AccessToken
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="expiresTime"></param>
        private void GetTaobaoAccessToken(out string accessToken, out DateTime expiresTime)
        {
            accessToken = null;
            expiresTime = DateTime.Now.AddYears(-1);
#warning 以下部分需下载淘宝的TopSdk后才能进行授权
            //以下部分需下载淘宝的TopSdk后才能进行授权
            //var tbClient = new DefaultTopClient(_url, this._appKey.ToString(), this._appSecret);
            //var req = new TopAuthTokenCreateRequest()
            //{
            //    Code = Request.Query["TaobaoCode"]
            //};
            //var response = tbClient.Execute(req);
            //if (!response.IsError)
            //{
            //    var obj = (JObject)JsonConvert.DeserializeObject(response.TokenResult);
            //    accessToken = obj.Value<string>("access_token");
            //    var expiresIn = obj.Value<long>("expires_in");
            //    expiresTime = DateTime.Now.AddSeconds(expiresIn - reduceSeconds);
            //}
            //else
            //{
            //    this.Logger.Error(response.ErrMsg);
            //}
        }
        /// <summary>
        /// 验证签名
        /// </summary>
        /// <returns></returns>
        private bool ValidSignature()
        {
            if (this.ValidSignatureTime())
            {
                var dic = new SortedDictionary<string, string>();
                foreach (var key in Request.Query.Keys)
                {
                    dic.Add(key, Request.Query[key]);
                }
                long appKey;
                return long.TryParse(Request.Query["AppKey"], out appKey) && appKey == this._appKey
                    && this._authState.Equals(Request.Query["AuthState"], StringComparison.OrdinalIgnoreCase)
                    && !string.IsNullOrWhiteSpace(Request.Query["TaobaoCode"])
                    && SignatureHelper.VerifyData(this._partnerSecret, Request.Query["SignData"], dic, Request.Query["SignType"]);
            }
            return false;
        }
        private bool ValidSignatureTime()
        {
            DateTime time;
            return DateTime.TryParseExact(Request.Query["RequestTime"], "yyyyMMddHHmmss", CultureInfo.InvariantCulture, DateTimeStyles.None, out time)
                && time >= DateTime.Now.AddMinutes(-this._signTimeRange)
                && time <= DateTime.Now.AddMinutes(this._signTimeRange);
        }
    }
}
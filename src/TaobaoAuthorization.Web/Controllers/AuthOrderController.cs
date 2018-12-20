using Abp.Application.Services.Dto;
using Abp.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TaobaoAuthorization.Authorizations;
using TaobaoAuthorization.Authorizations.Dto;
using TaobaoAuthorization.Configuration;
using TaobaoAuthorization.Partners;

namespace TaobaoAuthorization.Web.Controllers
{
    [Route("Auth")]
    public class AuthOrderController : TaobaoAuthorizationControllerBase
    {
        //淘宝授权说明页面 http://open.taobao.com/doc.htm?docId=102635&docType=1
        private readonly AppSettings _appSettings;
        private readonly IAuthOrderAppService _authOrderAppService;
        private readonly IPartnerAppService _partnerAppService;
        public AuthOrderController(IAuthOrderAppService authOrderAppService,
            IPartnerAppService partnerAppService,
            IOptionsSnapshot<AppSettings> appSettings)
        {
            this._authOrderAppService = authOrderAppService;
            this._appSettings = appSettings.Value;
            this._partnerAppService = partnerAppService;
        }
        [HttpGet("Authorization")]
        public async Task<ActionResult> Authorization([FromQuery]CreateAuthOrderInput input)
        {
            var order = await this._authOrderAppService.Create(input);
            var redirectUriHost = this._appSettings.RedirectUriHost;
            if (string.IsNullOrWhiteSpace(redirectUriHost))
            {
                redirectUriHost = $"{Request.Scheme}://{Request.Host}/{Request.PathBase}";
            }
            var state = this.EncryptAuthState(order, input);
            return Redirect($"{this._appSettings.TaobaoOAuthUrl}?response_type=code&client_id={input.AppKey}&redirect_uri={redirectUriHost.TrimEnd('/')}/Auth/Callback&state={state}&view={input.AuthView}");
        }
        private string EncryptAuthState(AuthOrderDto order, CreateAuthOrderInput input)
        {
            var state = $"{order.Id}_{input.AuthState}_{DateTime.Now.ToString("fff")}";
            var encryptData = TripleDESHelper.Encrypt(Encoding.UTF8.GetBytes(state), this._appSettings.GetTripleDESKeyData(), null, CipherMode.ECB, PaddingMode.PKCS7);
            return MD5Helper.ConvertToString(encryptData);
        }
        private bool ValidAuthState(string state, out long id, out string authState)
        {
            id = 0;
            authState = null;
            try
            {
                var data = MD5Helper.ConvertStringToByte(state);
                data = TripleDESHelper.Decrypt(data, this._appSettings.GetTripleDESKeyData(), null, CipherMode.ECB, PaddingMode.PKCS7);
                var origStr = Encoding.UTF8.GetString(data);
                if (!string.IsNullOrWhiteSpace(origStr) && Regex.IsMatch(origStr, @"^\d+_"))
                {
                    var idxS = origStr.IndexOf('_');
                    var idxE = origStr.LastIndexOf('_');
                    id = long.Parse(origStr.Substring(0, idxS));
                    authState = origStr.Substring(idxS + 1, idxE - idxS - 1);
                    return true;
                }
            }
            catch { }
            return false;
        }
        [HttpGet("Callback")]
        public async Task<ActionResult> Callback(string code, string state, string error, string error_description)
        {
            var authed = false;
            if (this.ValidAuthState(state, out long id, out string authState))
            {
                var order = await this._authOrderAppService.Get(new EntityDto<long>(id));
                if (order != null && order.AuthState == authState && string.IsNullOrWhiteSpace(order.TaobaoCode))
                {
                    if (!string.IsNullOrWhiteSpace(code))
                    {
                        authed = true;
                        order.TaobaoCode = code;
                        order.Error = string.Empty;
                        order.ErrorDescription = string.Empty;
                    }
                    else if (!string.IsNullOrWhiteSpace(error))
                    {
                        order.Error = error;
                        order.ErrorDescription = error_description;
                    }
                    await this._authOrderAppService.Update(order);
                    if (authed && !string.IsNullOrWhiteSpace(order.RedirectUri))
                    {
                        var partner = await this._partnerAppService.Get(new EntityDto<long>(order.PartnerId)).ConfigureAwait(false);
                        var dic = new SortedDictionary<string, string>()
                        {
                            { "PartnerKey",partner.PartnerKey},
                            { "AuthState",order.AuthState},
                            { "AppKey",order.AppKey.ToString()},
                            { "TaobaoCode",order.TaobaoCode},
                            { "RequestTime",DateTime.Now.ToString("yyyyMMddHHmmss")},
                            { "SignType","SHA1"},
                        };
                        var signData = SignatureHelper.GetSignData(partner.SecretKey, dic, "SHA1");
                        dic.Add("SignData", signData);
                        var queryString = QueryString.Create(dic);
                        return Redirect($"{order.RedirectUri}{queryString}");
                        //回调页面可以直接在后端通过TopSdk中的TopAuthTokenCreateRequest来获取AccessToken
                        //code只能被使用一次，且时限较短
                    }
                }
            }
            return new ContentResult()
            {
                ContentType = "text/plain",
                Content = authed ? L("AreadyAuthed") : L("AuthRefused")
            };
        }
        [HttpGet("GetCode")]
        [DontWrapResult]
        public async Task<string> GetTaobaoCode([FromQuery]GetAuthOrderInput input)
        {
            var order = await this._authOrderAppService.Get(input);
            if (order != null
                && !string.IsNullOrWhiteSpace(order.TaobaoCode)
                && order.LastModificationTime > DateTime.Now.AddSeconds(-this._appSettings.CodeExpiresIn))
            {
                //获取到的Code有时效性，虽然暂时还不清楚这时效性是多长，只能说比较短
                return order.TaobaoCode;
            }
            return string.Empty;//统一200返回，客户端根据是否为空来判断Code是否获取成功
        }
    }
}

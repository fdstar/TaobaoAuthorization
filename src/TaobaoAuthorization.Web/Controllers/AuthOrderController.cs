using Abp.Application.Services.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
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
            var redirectHost = this._appSettings.RedirectHost;
            if (string.IsNullOrWhiteSpace(redirectHost))
            {
                redirectHost = $"{Request.Scheme}://{Request.Host}/{Request.PathBase}";
            }
            return Redirect($"{this._appSettings.TaobaoOAuthUrl}?response_type=code&client_id={input.AppKey}&redirect_uri={redirectHost.TrimEnd('/')}/Auth/Callback&state={order.Id}_{input.AuthState}&view={input.AuthView}");
        }
        [HttpGet("Callback")]
        public async Task<ActionResult> Callback(string code, string state, string error, string error_description)
        {
            var authed = false;
            if (!string.IsNullOrWhiteSpace(state) && Regex.IsMatch(state, @"^\d+_"))
            {
                var idx = state.IndexOf('_');
                var id = long.Parse(state.Substring(0, idx));
                var authState = state.Substring(idx + 1);
                var order = await this._authOrderAppService.Get(new EntityDto<long>(id));
                if (order != null && order.AuthState == authState && string.IsNullOrWhiteSpace(order.TaobaoCode))
                {
                    if (!string.IsNullOrWhiteSpace(code))
                    {
                        authed = true;
                        order.TaobaoCode = code;
                    }
                    else if (string.IsNullOrWhiteSpace(error))
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
        public async Task<string> GetTaobaoCode([FromQuery]GetAuthOrderInput input)
        {
            var order = await this._authOrderAppService.Get(input);
            if (order != null)
            {
                return order.TaobaoCode;
            }
            return null;
        }
    }
}

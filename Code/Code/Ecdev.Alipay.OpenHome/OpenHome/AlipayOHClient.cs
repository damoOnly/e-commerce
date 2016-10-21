using Aop.Api;
using Aop.Api.Request;
using Aop.Api.Response;
using Aop.Api.Util;
using Ecdev.Alipay.OpenHome.AlipayOHException;
using Ecdev.Alipay.OpenHome.Handle;
using Ecdev.Alipay.OpenHome.Model;
using Ecdev.Alipay.OpenHome.Request;
using Ecdev.Alipay.OpenHome.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web;
namespace Ecdev.Alipay.OpenHome
{
	public class AlipayOHClient
	{
		private class EventType
		{
			public const string Verifygw = "verifygw";
			public const string Follow = "follow";
		}
		private const string APP_ID = "app_id";
		private const string FORMAT = "format";
		private const string METHOD = "method";
		private const string TIMESTAMP = "timestamp";
		private const string VERSION = "version";
		private const string SIGN_TYPE = "sign_type";
		private const string ACCESS_TOKEN = "auth_token";
		private const string SIGN = "sign";
		private const string TERMINAL_TYPE = "terminal_type";
		private const string TERMINAL_INFO = "terminal_info";
		private const string PROD_CODE = "prod_code";
		private const string BIZ_CONTENT = "biz_content";
		private const string SING = "sign";
		private const string CONTENT = "biz_content";
		private const string SING_TYPE = "sign_type";
		private const string SERVICE = "service";
		private const string CHARSET = "charset";
		private string version;
		private string format;
		private string signType = "RSA";
		private string dateTimeFormat = "yyyy-MM-dd HH:mm:ss";
		private string serverUrl;
		private string appId;
		private string privateKey;
		private string pubKey;
		private string aliPubKey;
		private string charset;
		public AliRequest request;
		private WebUtils webUtils = new WebUtils();
		private HttpContext context;
		public event OnUserFollow OnUserFollow;
		public AlipayOHClient(string url, string appId, string aliPubKey, string priKey, string pubKey, string charset = "UTF-8")
		{
			this.serverUrl = url;
			this.appId = appId;
			this.privateKey = priKey;
			this.charset = charset;
			this.pubKey = pubKey;
			this.aliPubKey = aliPubKey;
		}
		public AlipayOHClient(string aliPubKey, string priKey, string pubKey, string charset = "UTF-8")
		{
			this.privateKey = priKey;
			this.charset = charset;
			this.pubKey = pubKey;
			this.aliPubKey = aliPubKey;
		}
		internal string FireUserFollowEvent()
		{
			return this.OnUserFollow(this, new System.EventArgs());
		}
		public void HandleAliOHResponse(HttpContext context)
		{
			this.context = context;
			string text = context.Request["sign"];
			string text2 = context.Request["biz_content"];
			string text3 = context.Request["sign_type"];
			string text4 = context.Request["service"];
			this.request = XmlSerialiseHelper.Deserialize<AliRequest>(text2);
			IHandle handle = null;
			string eventType = this.request.EventType;
			if (eventType != null)
			{
				if (!(eventType == "verifygw"))
				{
					if (eventType == "follow")
					{
						handle = new UserFollowHandle();
					}
				}
				else
				{
					handle = new VerifyGateWayHandle();
				}
			}
			if (handle != null)
			{
				handle.client = this;
				handle.LocalRsaPriKey = this.privateKey;
				handle.LocalRsaPubKey = this.pubKey;
				handle.AliRsaPubKey = this.aliPubKey;
				string s = handle.Handle(text2);
				context.Response.AddHeader("Content-Type", "application/xml");
				context.Response.Write(s);
			}
		}
		public T Execute<T>(IRequest request) where T : AliResponse, IAliResponseStatus
		{
			System.Collections.Generic.Dictionary<string, string> dictionary = new System.Collections.Generic.Dictionary<string, string>();
			dictionary.Add("method", request.GetMethodName());
			dictionary.Add("app_id", this.appId);
			dictionary.Add("timestamp", System.DateTime.Now.ToString(this.dateTimeFormat));
			dictionary.Add("sign_type", this.signType);
			dictionary.Add("charset", this.charset);
			dictionary.Add("biz_content", request.GetBizContent());
			dictionary.Add("sign", AlipaySignature.RSASign(dictionary, this.privateKey, this.charset));
			string value = this.webUtils.DoPost(this.serverUrl, dictionary, this.charset);
			T result = JsonConvert.DeserializeObject<T>(value);
			if (result.error_response != null && result.error_response.IsError)
			{
				throw new AliResponseException(result.error_response.code, result.error_response.sub_msg);
			}
			if (result.Code != "200")
			{
				throw new AliResponseException(result.Code, result.Message);
			}
			return result;
		}
		public AlipaySystemOauthTokenResponse OauthTokenRequest(string authCode)
		{
			AlipaySystemOauthTokenRequest alipaySystemOauthTokenRequest = new AlipaySystemOauthTokenRequest();
			alipaySystemOauthTokenRequest.GrantType = AlipaySystemOauthTokenRequest.AllGrantType.authorization_code;
			alipaySystemOauthTokenRequest.Code = authCode;
			AlipaySystemOauthTokenResponse result = null;
			try
			{
				IAopClient aopClient = new DefaultAopClient(this.serverUrl, this.appId, this.privateKey);
				result = aopClient.Execute<AlipaySystemOauthTokenResponse>(alipaySystemOauthTokenRequest);
			}
			catch (AopException var_3_3D)
			{
			}
			return result;
		}
		public AlipayUserUserinfoShareResponse GetAliUserInfo(string accessToken)
		{
			AlipayUserUserinfoShareRequest alipayUserUserinfoShareRequest = new AlipayUserUserinfoShareRequest();
			alipayUserUserinfoShareRequest.AuthToken = accessToken;
			IAopClient aopClient = new DefaultAopClient(this.serverUrl, this.appId, this.privateKey);
			return aopClient.Execute<AlipayUserUserinfoShareResponse>(alipayUserUserinfoShareRequest);
		}
	}
}

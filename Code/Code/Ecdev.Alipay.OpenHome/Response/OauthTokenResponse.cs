using Ecdev.Alipay.OpenHome.Model;
using System;
namespace Ecdev.Alipay.OpenHome.Response
{
	public class OauthTokenResponse : AliResponse, IAliResponseStatus
	{
		public AliUserTokenInfo alipay_system_oauth_token_response
		{
			get;
			set;
		}
		public string Code
		{
			get
			{
				return (this.alipay_system_oauth_token_response != null) ? "200" : "0";
			}
		}
		public string Message
		{
			get
			{
				return (this.Code != "200") ? "未知错误" : "";
			}
		}
	}
}

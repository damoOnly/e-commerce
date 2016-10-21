using System;
namespace Ecdev.Weixin.Pay
{
	public class PayAccount
	{
		public string AppId
		{
			get;
			set;
		}
		public string AppSecret
		{
			get;
			set;
		}
		public string PartnerId
		{
			get;
			set;
		}
		public string PartnerKey
		{
			get;
			set;
		}
		public string PaySignKey
		{
			get;
			set;
		}
		public PayAccount()
		{
		}
		public PayAccount(string appId, string appSecret, string partnerId, string partnerKey, string paySignKey)
		{
			this.AppId = appId;
			this.AppSecret = appSecret;
			this.PartnerId = partnerId;
			this.PartnerKey = partnerKey;
			this.PaySignKey = paySignKey;
		}
	}
}

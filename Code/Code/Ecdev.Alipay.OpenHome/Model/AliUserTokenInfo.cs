using System;
namespace Ecdev.Alipay.OpenHome.Model
{
	[System.Serializable]
	public class AliUserTokenInfo
	{
		public string alipay_user_id
		{
			get;
			set;
		}
		public string access_token
		{
			get;
			set;
		}
	}
}

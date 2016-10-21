using Ecdev.Alipay.OpenHome.Model;
using System;
namespace Ecdev.Alipay.OpenHome.Response
{
	[System.Serializable]
	public class MenuAddResponse : AliResponse, IAliResponseStatus
	{
		public AliResponseMessage alipay_mobile_public_menu_add_response
		{
			get;
			set;
		}
		public string Code
		{
			get
			{
				return this.alipay_mobile_public_menu_add_response.code;
			}
		}
		public string Message
		{
			get
			{
				return this.alipay_mobile_public_menu_add_response.msg;
			}
		}
	}
}

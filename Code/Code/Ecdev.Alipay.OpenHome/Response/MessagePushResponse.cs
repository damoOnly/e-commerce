using Ecdev.Alipay.OpenHome.Model;
using System;
namespace Ecdev.Alipay.OpenHome.Response
{
	[System.Serializable]
	public class MessagePushResponse : AliResponse, IAliResponseStatus
	{
		public AliResponseMessage alipay_mobile_public_message_push_response
		{
			get;
			set;
		}
		public new string sign
		{
			get;
			set;
		}
		public string Code
		{
			get;
			set;
		}
		public string Message
		{
			get;
			set;
		}
	}
}

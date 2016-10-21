using Ecdev.Alipay.OpenHome.Model;
using Ecdev.Alipay.OpenHome.Utility;
using System;
namespace Ecdev.Alipay.OpenHome.Request
{
	public class MessagePushRequest : IRequest
	{
		private Message message;
		public string GetMethodName()
		{
			return "alipay.mobile.public.message.push";
		}
		public MessagePushRequest(string appid, string toUserId, Articles articles, int articleCount, string agreementId = null, string msgType = "image-text")
		{
			Message message = new Message
			{
				AgreementId = agreementId,
				AppId = appid,
				Articles = articles,
				ArticleCount = articleCount,
				ToUserId = toUserId,
				CreateTime = TimeHelper.TransferToMilStartWith1970(System.DateTime.Now).ToString("F0"),
				MsgType = msgType
			};
			this.message = message;
		}
		public string GetBizContent()
		{
			return XmlSerialiseHelper.Serialise<Message>(this.message);
		}
	}
}

using Ecdev.Weixin.MP.Request;
using Ecdev.Weixin.MP.Request.Event;
using System;
using System.Xml.Linq;
namespace Ecdev.Weixin.MP.Util
{
	public static class RequestMessageFactory
	{
		public static AbstractRequest GetRequestEntity(XDocument doc)
		{
			AbstractRequest abstractRequest;
			switch (MsgTypeHelper.GetMsgType(doc))
			{
			case RequestMsgType.Text:
				abstractRequest = new TextRequest();
				break;
			case RequestMsgType.Image:
				abstractRequest = new ImageRequest();
				break;
			case RequestMsgType.Voice:
				abstractRequest = new VoiceRequest();
				break;
			case RequestMsgType.Video:
				abstractRequest = new VideoRequest();
				break;
			case RequestMsgType.Location:
				abstractRequest = new LocationRequest();
				break;
			case RequestMsgType.Link:
				abstractRequest = new LinkRequest();
				break;
			case RequestMsgType.Event:
				switch (EventTypeHelper.GetEventType(doc))
				{
				case RequestEventType.Subscribe:
					abstractRequest = new SubscribeEventRequest();
					break;
				case RequestEventType.UnSubscribe:
					abstractRequest = new UnSubscribeEventRequest();
					break;
				case RequestEventType.Scan:
					abstractRequest = new ScanEventRequest();
					break;
				case RequestEventType.Location:
					abstractRequest = new LocationEventRequest();
					break;
				case RequestEventType.Click:
					abstractRequest = new ClickEventRequest();
					break;
				default:
					throw new ArgumentOutOfRangeException();
				}
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
			EntityHelper.FillEntityWithXml<AbstractRequest>(abstractRequest, doc);
			return abstractRequest;
		}
	}
}

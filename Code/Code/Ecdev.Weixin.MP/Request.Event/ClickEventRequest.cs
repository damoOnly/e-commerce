using System;
namespace Ecdev.Weixin.MP.Request.Event
{
	public class ClickEventRequest : EventRequest
	{
		public string EventKey
		{
			get;
			set;
		}
		public override RequestEventType Event
		{
			get
			{
				return RequestEventType.Click;
			}
			set
			{
			}
		}
	}
}

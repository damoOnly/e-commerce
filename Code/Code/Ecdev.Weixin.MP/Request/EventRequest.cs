using System;
namespace Ecdev.Weixin.MP.Request
{
	public abstract class EventRequest : AbstractRequest
	{
		public virtual RequestEventType Event
		{
			get;
			set;
		}
	}
}

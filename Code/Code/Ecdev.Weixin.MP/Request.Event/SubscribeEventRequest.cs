using System;
namespace Ecdev.Weixin.MP.Request.Event
{
	public class SubscribeEventRequest : EventRequest
	{
		public override RequestEventType Event
		{
			get
			{
				return RequestEventType.Subscribe;
			}
			set
			{
			}
		}

        /// <summary>
        /// 事件KEY值，是一个32位无符号整数，即创建二维码时的二维码scene_id
        /// </summary>
        public string EventKey { get; set; }

        /// <summary>
        /// 二维码的ticket，可用来换取二维码图片
        /// </summary>
        public string Ticket { get; set; }
	}
}

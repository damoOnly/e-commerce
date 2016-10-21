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
        /// �¼�KEYֵ����һ��32λ�޷�����������������ά��ʱ�Ķ�ά��scene_id
        /// </summary>
        public string EventKey { get; set; }

        /// <summary>
        /// ��ά���ticket����������ȡ��ά��ͼƬ
        /// </summary>
        public string Ticket { get; set; }
	}
}

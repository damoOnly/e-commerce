using System;
namespace Ecdev.Weixin.MP.Domain
{
	public class QrcodeTicket
	{
        public string ticket
		{
			get;
			set;
		}
        public int expire_seconds
		{
			get;
			set;
		}
        public string url
		{
			get;
			set;
		}

	}
}

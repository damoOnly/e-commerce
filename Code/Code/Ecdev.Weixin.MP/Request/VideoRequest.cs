using System;
namespace Ecdev.Weixin.MP.Request
{
	public class VideoRequest : AbstractRequest
	{
		public int MediaId
		{
			get;
			set;
		}
		public int ThumbMediaId
		{
			get;
			set;
		}
	}
}

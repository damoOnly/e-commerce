using System;
namespace Ecdev.Weixin.MP.Request
{
	public class ImageRequest : AbstractRequest
	{
		public string PicUrl
		{
			get;
			set;
		}
		public int MediaId
		{
			get;
			set;
		}
	}
}

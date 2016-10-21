using Ecdev.Weixin.MP.Domain;
using System;
namespace Ecdev.Weixin.MP.Response
{
	public class ImageResponse : AbstractResponse
	{
		public Image Image
		{
			get;
			set;
		}
		public override ResponseMsgType MsgType
		{
			get
			{
				return ResponseMsgType.Image;
			}
		}
	}
}

using Ecdev.Weixin.MP.Domain;
using System;
namespace Ecdev.Weixin.MP.Response
{
	public class VideoResponse : AbstractResponse
	{
		public Video Video
		{
			get;
			set;
		}
		public override ResponseMsgType MsgType
		{
			get
			{
				return ResponseMsgType.Video;
			}
		}
	}
}

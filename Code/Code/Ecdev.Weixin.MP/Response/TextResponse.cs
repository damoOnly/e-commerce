using System;
namespace Ecdev.Weixin.MP.Response
{
	public class TextResponse : AbstractResponse
	{
		public string Content
		{
			get;
			set;
		}
		public override ResponseMsgType MsgType
		{
			get
			{
				return ResponseMsgType.Text;
			}
		}
	}
}

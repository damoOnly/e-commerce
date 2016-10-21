using Ecdev.Weixin.MP.Domain;
using System;
namespace Ecdev.Weixin.MP.Response
{
	public class VoiceResponse : AbstractResponse
	{
		public Voice Voice
		{
			get;
			set;
		}
		public override ResponseMsgType MsgType
		{
			get
			{
				return ResponseMsgType.Voice;
			}
		}
	}
}

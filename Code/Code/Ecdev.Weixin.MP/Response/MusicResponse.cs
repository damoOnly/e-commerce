using Ecdev.Weixin.MP.Domain;
using System;
namespace Ecdev.Weixin.MP.Response
{
	public class MusicResponse : AbstractResponse
	{
		public Music Music
		{
			get;
			set;
		}
		public override ResponseMsgType MsgType
		{
			get
			{
				return ResponseMsgType.Music;
			}
		}
	}
}

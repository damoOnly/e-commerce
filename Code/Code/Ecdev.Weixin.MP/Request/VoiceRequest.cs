using System;
namespace Ecdev.Weixin.MP.Request
{
	public class VoiceRequest : AbstractRequest
	{
		public int MediaId
		{
			get;
			set;
		}
		public string Format
		{
			get;
			set;
		}
	}
}

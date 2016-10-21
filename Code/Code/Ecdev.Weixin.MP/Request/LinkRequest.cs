using System;
namespace Ecdev.Weixin.MP.Request
{
	public class LinkRequest : AbstractRequest
	{
		public int Title
		{
			get;
			set;
		}
		public string Description
		{
			get;
			set;
		}
		public string Url
		{
			get;
			set;
		}
	}
}

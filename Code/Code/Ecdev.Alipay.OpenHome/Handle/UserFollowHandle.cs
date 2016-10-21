using Aop.Api.Util;
using System;
namespace Ecdev.Alipay.OpenHome.Handle
{
	internal class UserFollowHandle : IHandle
	{
		public string LocalRsaPriKey
		{
			get;
			set;
		}
		public string LocalRsaPubKey
		{
			get;
			set;
		}
		public string AliRsaPubKey
		{
			get;
			set;
		}
		public AlipayOHClient client
		{
			get;
			set;
		}
		public string Handle(string requestContent)
		{
			string bizContent = this.client.FireUserFollowEvent();
			return AlipaySignature.encryptAndSign(bizContent, this.AliRsaPubKey, this.LocalRsaPriKey, "UTF-8", false, true, true);
		}
	}
}

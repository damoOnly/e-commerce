using Aop.Api.Util;
using Ecdev.Alipay.OpenHome.Utility;
using System;
namespace Ecdev.Alipay.OpenHome.Handle
{
	internal class VerifyGateWayHandle : IHandle
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
			string bizContent = string.Format("<success>true</success><biz_content>{0}</biz_content>", RsaFileHelper.GetRSAKeyContent(this.LocalRsaPubKey, true));
			return AlipaySignature.encryptAndSign(bizContent, this.AliRsaPubKey, this.LocalRsaPriKey, "UTF-8", false, true, true);
		}
	}
}

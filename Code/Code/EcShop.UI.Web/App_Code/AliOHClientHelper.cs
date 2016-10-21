using EcShop.Membership.Context;
using Ecdev.Alipay.OpenHome;
using System;
namespace EcShop.UI.Web.App_Code
{
	public class AliOHClientHelper
	{
		public static AlipayOHClient Instance(string serverRootPath)
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
			string aliOHServerUrl = masterSettings.AliOHServerUrl;
			string aliOHAppId = masterSettings.AliOHAppId;
			string priKey = serverRootPath + "/config/rsa_private_key.pem";
			string aliPubKey = serverRootPath + "/config/alipay_pubKey.pem";
			string pubKey = serverRootPath + "/config/rsa_public_key.pem";
			return new AlipayOHClient(aliOHServerUrl, aliOHAppId, aliPubKey, priKey, pubKey, "UTF-8");
		}
	}
}

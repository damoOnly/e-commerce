using EcShop.Membership.Context;
using Ecdev.Alipay.OpenHome;
using Ecdev.Alipay.OpenHome.Model;
using Ecdev.Alipay.OpenHome.Request;
using Ecdev.Alipay.OpenHome.Response;
using System;
using System.Web;
namespace EcShop.UI.Web.API
{
	public class alipay : System.Web.IHttpHandler
	{
		private string priKeyFile;
		private string alipayPubKeyFile;
		private string pubKeyFile;
		private string logfile;
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}
		public void ProcessRequest(System.Web.HttpContext context)
		{
			string arg_10_0 = context.Request["EventType"];
			string arg_16_0 = string.Empty;
			this.priKeyFile = context.Server.MapPath("~/config/rsa_private_key.pem");
			this.alipayPubKeyFile = context.Server.MapPath("~/config/alipay_pubKey.pem");
			this.pubKeyFile = context.Server.MapPath("~/config/rsa_public_key.pem");
			this.logfile = context.Server.MapPath("~/a.log");
			AlipayOHClient alipayOHClient = new AlipayOHClient(this.alipayPubKeyFile, this.priKeyFile, this.pubKeyFile, "UTF-8");
			alipayOHClient.OnUserFollow += new OnUserFollow(this.client_OnUserFollow);
			alipayOHClient.HandleAliOHResponse(context);
		}
		private string client_OnUserFollow(object sender, System.EventArgs e)
		{
			try
			{
				AlipayOHClient alipayOHClient = (AlipayOHClient)sender;
				SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
				Articles articles = new Articles
				{
					Item = new Item
					{
						Description = masterSettings.AliOHFollowRelay,
						Title = string.IsNullOrWhiteSpace(masterSettings.AliOHFollowRelayTitle) ? "欢迎您的关注！" : masterSettings.AliOHFollowRelayTitle
					}
				};
				IRequest request = new MessagePushRequest(alipayOHClient.request.AppId, alipayOHClient.request.FromUserId, articles, 1, null, "image-text");
				AlipayOHClient alipayOHClient2 = new AlipayOHClient(masterSettings.AliOHServerUrl, alipayOHClient.request.AppId, this.alipayPubKeyFile, this.priKeyFile, this.pubKeyFile, "UTF-8");
				alipayOHClient2.Execute<MessagePushResponse>(request);
			}
			catch (System.Exception)
			{
			}
			return "";
		}
	}
}

using EcShop.ControlPanel.Store;
using EcShop.Entities.Store;
using EcShop.Membership.Context;
using EcShop.UI.ControlPanel.Utility;
using EcShop.UI.Web.App_Code;
using System;
using System.IO;
using System.Web.UI.WebControls;
namespace EcShop.UI.Web.Admin.AliOH
{
	[PrivilegeCheck(Privilege.AliohMobileAlipaySet)]
	public class WebForm1 : AdminPage
	{
		protected System.Web.UI.WebControls.TextBox txtAppId;
		protected System.Web.UI.WebControls.TextBox txtAppWelcome;
		protected System.Web.UI.WebControls.Literal txtUrl;
		protected System.Web.UI.WebControls.TextBox txtPubKey;
		protected System.Web.UI.WebControls.Button btnAdd;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!this.Page.IsPostBack)
			{
				SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
				string path = base.Server.MapPath("~/config/rsa_public_key.pem");
				string text;
				if (!System.IO.File.Exists(path))
				{
					text = this.CreateRsaKey();
					SettingsManager.Save(masterSettings);
				}
				else
				{
					text = RsaKeyHelper.GetRSAKeyContent(path, true);
				}
				this.txtAppId.Text = masterSettings.AliOHAppId;
				this.txtAppWelcome.Text = masterSettings.AliOHFollowRelay;
				this.txtUrl.Text = string.Format("http://{0}/api/alipay.ashx", base.Request.Url.Host);
				this.txtPubKey.Text = text;
			}
		}
		private string CreateRsaKey()
		{
			string keyDirectory = base.Server.MapPath("~/config");
			string generatorPath = base.Server.MapPath("~/config/RSAGenerator/Rsa.exe");
			return RsaKeyHelper.CreateRSAKeyFile(generatorPath, keyDirectory);
		}
		protected void btnOK_Click(object sender, System.EventArgs e)
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
			masterSettings.AliOHAppId = this.txtAppId.Text;
			masterSettings.AliOHFollowRelay = this.txtAppWelcome.Text;
			SettingsManager.Save(masterSettings);
			this.ShowMsg("修改成功", true);
		}
	}
}

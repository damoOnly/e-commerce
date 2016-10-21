using EcShop.Membership.Context;
using EcShop.UI.Common.Controls;
using EcShop.UI.ControlPanel.Utility;
using System;
using System.Web.UI.WebControls;
namespace EcShop.UI.Web.Admin.WAPShop
{
	public class PayConfig : AdminPage
	{
		protected System.Web.UI.WebControls.TextBox txtAppId;
		protected System.Web.UI.WebControls.TextBox txtAppSecret;
		protected System.Web.UI.WebControls.TextBox txtPartnerID;
		protected System.Web.UI.WebControls.TextBox txtPartnerKey;
		protected System.Web.UI.WebControls.TextBox txtPaySignKey;
		protected YesNoRadioButtonList radEnableHtmRewrite;
		protected System.Web.UI.WebControls.Button btnOK;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			if (!base.IsPostBack)
			{
				SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
				this.txtAppId.Text = masterSettings.WeixinAppId;
				this.txtAppSecret.Text = masterSettings.WeixinAppSecret;
				this.txtPartnerID.Text = masterSettings.WeixinPartnerID;
				this.txtPartnerKey.Text = masterSettings.WeixinPartnerKey;
				this.txtPaySignKey.Text = masterSettings.WeixinPaySignKey;
				this.radEnableHtmRewrite.SelectedValue = masterSettings.EnableWeiXinRequest;
			}
		}
		protected void btnOK_Click(object sender, System.EventArgs e)
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
			masterSettings.WeixinAppId = this.txtAppId.Text;
			masterSettings.WeixinAppSecret = this.txtAppSecret.Text;
			masterSettings.WeixinPartnerID = this.txtPartnerID.Text;
			masterSettings.WeixinPartnerKey = this.txtPartnerKey.Text;
			masterSettings.WeixinPaySignKey = this.txtPaySignKey.Text;
			masterSettings.EnableWeiXinRequest = this.radEnableHtmRewrite.SelectedValue;
			SettingsManager.Save(masterSettings);
			this.ShowMsg("设置成功", true);
		}
	}
}

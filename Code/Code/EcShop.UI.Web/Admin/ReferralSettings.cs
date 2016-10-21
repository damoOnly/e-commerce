using EcShop.ControlPanel.Store;
using EcShop.Membership.Context;
using EcShop.UI.ControlPanel.Utility;
using kindeditor.Net;
using System;
using System.Web.UI.WebControls;
namespace EcShop.UI.Web.Admin
{
	[AdministerCheck(true)]
	public class ReferralSettings : AdminPage
	{
		protected KindeditorControl fckReferralIntroduction;
		protected System.Web.UI.WebControls.CheckBox chkIsAudit;
		protected System.Web.UI.WebControls.Button btnOK;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			if (!this.Page.IsPostBack)
			{
				SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
				this.fckReferralIntroduction.Text = masterSettings.ReferralIntroduction;
				this.chkIsAudit.Checked = masterSettings.IsAuditReferral;
			}
		}
		private void btnOK_Click(object sender, System.EventArgs e)
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
			masterSettings.ReferralIntroduction = this.fckReferralIntroduction.Text;
			masterSettings.IsAuditReferral = this.chkIsAudit.Checked;
			SettingsManager.Save(masterSettings);
			this.ShowMsg("设置成功", true);
		}
	}
}

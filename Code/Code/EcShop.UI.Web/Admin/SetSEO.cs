using EcShop.ControlPanel.Store;
using EcShop.Core;
using EcShop.Membership.Context;
using EcShop.UI.ControlPanel.Utility;
using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace EcShop.UI.Web.Admin
{
    //[AdministerCheck(true)]
	public class SetSEO : AdminPage
	{
		protected System.Web.UI.WebControls.TextBox txtSiteDescription;
		protected System.Web.UI.HtmlControls.HtmlGenericControl txtSiteDescriptionTip;
		protected System.Web.UI.WebControls.TextBox txtSearchMetaDescription;
		protected System.Web.UI.HtmlControls.HtmlGenericControl txtSearchMetaDescriptionTip;
		protected System.Web.UI.WebControls.TextBox txtSearchMetaKeywords;
		protected System.Web.UI.HtmlControls.HtmlGenericControl txtSearchMetaKeywordsTip;
		protected System.Web.UI.WebControls.Button btnOK;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!this.Page.IsPostBack)
			{
				SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
				this.txtSiteDescription.Text = masterSettings.SiteDescription;
				this.txtSearchMetaDescription.Text = masterSettings.SearchMetaDescription;
				this.txtSearchMetaKeywords.Text = masterSettings.SearchMetaKeywords;
			}
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
		}
		protected void btnOK_Click(object sender, System.EventArgs e)
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
			masterSettings.SiteDescription = this.txtSiteDescription.Text.Trim();
			masterSettings.SearchMetaDescription = this.txtSearchMetaDescription.Text.Trim();
			masterSettings.SearchMetaKeywords = this.txtSearchMetaKeywords.Text.Trim();
			Globals.EntityCoding(masterSettings, true);
			SettingsManager.Save(masterSettings);
			this.ShowMsg("保存成功", true);
		}
	}
}

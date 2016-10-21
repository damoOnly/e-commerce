using EcShop.ControlPanel.Store;
using EcShop.Entities.Store;
using EcShop.Membership.Context;
using EcShop.UI.Common.Controls;
using EcShop.UI.ControlPanel.Utility;
using kindeditor.Net;
using System;
using System.Web.UI.WebControls;
namespace EcShop.UI.Web.Admin.WAPShop
{
	[PrivilegeCheck(Privilege.WapOfflinePaySet)]
	public class OffLinePay : AdminPage
	{
		protected KindeditorControl fkContent;
		protected YesNoRadioButtonList radEnableOffLinePay;
		protected YesNoRadioButtonList radEnablePro;
		protected System.Web.UI.WebControls.Button btnAdd;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!this.Page.IsPostBack)
			{
				SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
				this.fkContent.Text = masterSettings.OffLinePayContent;
				this.radEnableOffLinePay.SelectedValue = masterSettings.EnableWapOffLinePay;
				this.radEnablePro.SelectedValue = masterSettings.EnableWapPodPay;
			}
		}
		protected void btnOK_Click(object sender, System.EventArgs e)
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
			masterSettings.OffLinePayContent = this.fkContent.Text;
			masterSettings.EnableWapOffLinePay = this.radEnableOffLinePay.SelectedValue;
			masterSettings.EnableWapPodPay = this.radEnablePro.SelectedValue;
			SettingsManager.Save(masterSettings);
			this.ShowMsg("修改成功", true);
		}
	}
}

using EcShop.ControlPanel.Store;
using EcShop.Core;
using EcShop.Entities.Store;
using EcShop.Membership.Context;
using EcShop.UI.Common.Controls;
using EcShop.UI.ControlPanel.Utility;
using Ecdev.Plugins;
using Ionic.Zlib;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;
using System.Xml;
namespace EcShop.UI.Web.Admin
{
    [PrivilegeCheck(Privilege.SendOrderSettings)]
    public class SendOrderSettings : AdminPage
	{
        protected System.Web.UI.WebControls.TextBox txtStartTime;
        protected System.Web.UI.WebControls.TextBox txtEndTime;
        protected System.Web.UI.WebControls.TextBox txtDay;
        protected System.Web.UI.WebControls.TextBox txtEmail;
        protected System.Web.UI.WebControls.TextBox txtRuntimes;
        protected System.Web.UI.WebControls.RadioButtonList RbIsRun;
        protected System.Web.UI.WebControls.Button btnSave;
		protected override void OnInitComplete(System.EventArgs e)
		{
			base.OnInitComplete(e);
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
		}
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!this.Page.IsPostBack)
			{
				SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
                this.txtStartTime.Text = masterSettings.SendOrderStartTime;
                this.txtEndTime.Text = masterSettings.SendOrderEndTime;
                this.txtDay.Text = masterSettings.SendOrderDay;
                this.txtEmail.Text = masterSettings.SendOrderEmail;
                this.txtRuntimes.Text = masterSettings.IsRunTimes;
                this.RbIsRun.SelectedValue = masterSettings.IsSendOrderOpen;
			}
		}
		private ConfigData LoadConfig(out string selectedName)
		{
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
            selectedName = masterSettings.SMSSender;
			if (string.IsNullOrEmpty(selectedName) || selectedName.Length == 0)
			{
				return null;
			}
			ConfigablePlugin configablePlugin = SMSSender.CreateInstance(selectedName);
			if (configablePlugin == null)
			{
				return null;
			}
			ConfigData configData = configablePlugin.GetConfigData(base.Request.Form);
         
			return configData;
		}
		private void btnSave_Click(object sender, System.EventArgs e)
		{
			string text;
			ConfigData configData = this.LoadConfig(out text);
			SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
			if (string.IsNullOrEmpty(text) || configData == null)
			{
				masterSettings.SendOrderStartTime = string.Empty;
				masterSettings.SendOrderEndTime = string.Empty;
                masterSettings.SendOrderDay = string.Empty;
                masterSettings.SendOrderEmail = string.Empty;
                masterSettings.IsSendOrderOpen = string.Empty;
                masterSettings.IsRunTimes = string.Empty;
			}
			else
			{
                masterSettings.SendOrderStartTime = this.txtStartTime.Text.Trim();
                masterSettings.SendOrderEndTime = this.txtEndTime.Text.Trim() ;
                masterSettings.SendOrderDay =this.txtDay.Text.Trim();
                masterSettings.SendOrderEmail = this.txtEmail.Text.Trim();
                masterSettings.IsSendOrderOpen = this.RbIsRun.SelectedValue.Trim();
                masterSettings.IsRunTimes = this.txtRuntimes.Text.Trim();
			}
			SettingsManager.Save(masterSettings);
            this.ShowMsg("保存完成！", true);
         
		}

	}
}

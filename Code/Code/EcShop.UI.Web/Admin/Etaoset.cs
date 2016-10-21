using EcShop.ControlPanel.Store;
using EcShop.Core;
using EcShop.Membership.Context;
using EcShop.UI.Common.Controls;
using EcShop.UI.ControlPanel.Utility;
using Ecdev.Components.Validation;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace EcShop.UI.Web.Admin
{
	[AdministerCheck(true)]
	public class Etaoset : AdminPage
	{
		protected System.Web.UI.WebControls.FileUpload fudVerifyFile;
		protected System.Web.UI.WebControls.Button btnUpoad;
		protected System.Web.UI.WebControls.TextBox txtEtaoID;
		protected YesNoRadioButtonList rdobltIsCreateFeed;
		protected System.Web.UI.HtmlControls.HtmlGenericControl incDir;
		protected System.Web.UI.WebControls.Label lblEtaoFeedInc;
		protected System.Web.UI.HtmlControls.HtmlGenericControl fulDir;
		protected System.Web.UI.WebControls.Label lblEtaoFeedFull;
		protected System.Web.UI.WebControls.Button btnChangeEmailSettings;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!this.Page.IsPostBack)
			{
				this.LoadEtaoOpen();
			}
		}
		protected void LoadEtaoOpen()
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
			this.txtEtaoID.Text = masterSettings.EtaoID;
			this.rdobltIsCreateFeed.SelectedValue = masterSettings.IsCreateFeed;
			if (!masterSettings.IsCreateFeed)
			{
				this.incDir.Visible = false;
				this.fulDir.Visible = false;
				return;
			}
			if (System.IO.File.Exists(this.Page.Server.MapPath(Globals.ApplicationPath + "/Storage/Root/IncrementIndex.xml")))
			{
				this.lblEtaoFeedInc.Text = HiContext.Current.HostPath + Globals.ApplicationPath + "/Storage/Root/IncrementIndex.xml";
				this.incDir.Visible = true;
			}
			else
			{
				this.incDir.Visible = false;
			}
			if (System.IO.File.Exists(this.Page.Server.MapPath(Globals.ApplicationPath + "/Storage/Root/FullIndex.xml")))
			{
				this.lblEtaoFeedFull.Text = HiContext.Current.HostPath + Globals.ApplicationPath + "/Storage/Root/FullIndex.xml";
				this.fulDir.Visible = true;
				return;
			}
			this.fulDir.Visible = false;
		}
		protected void btnUpoad_Click(object sender, System.EventArgs e)
		{
			if (!this.fudVerifyFile.HasFile)
			{
				this.ShowMsg("需要选择验证文件再点击上传。", false);
				return;
			}
			if (this.fudVerifyFile.PostedFile.ContentType.ToLower(System.Globalization.CultureInfo.InvariantCulture) != "text/plain")
			{
				this.ShowMsg("只能上传TXT文本文件", false);
				return;
			}
			if (!this.fudVerifyFile.FileName.ToLower(System.Globalization.CultureInfo.InvariantCulture).EndsWith(".txt") || this.fudVerifyFile.FileName.IndexOf('.') != this.fudVerifyFile.FileName.Length - 4)
			{
				this.ShowMsg("文件名只能有一个.号", false);
				return;
			}
			if (this.fudVerifyFile.FileName.ToLower() != "etao_domain_verify.txt")
			{
				this.ShowMsg("你上传的不是一淘的验证文件!", false);
				return;
			}
			string text = "etao_domain_verify.txt";
			string str = string.Empty;
			string text2 = string.Empty;
			if (!string.IsNullOrEmpty(Globals.ApplicationPath))
			{
				if (Globals.ApplicationPath.EndsWith("\\"))
				{
					str = Globals.ApplicationPath;
				}
				else
				{
					str = Globals.ApplicationPath + "\\";
				}
				text2 = HiContext.Current.Context.Request.MapPath(str + text);
			}
			else
			{
				text2 = HiContext.Current.Context.Request.MapPath("/");
				if (text2.EndsWith("\\"))
				{
					text2 += text;
				}
				else
				{
					text2 = text2 + "\\" + text;
				}
			}
			this.fudVerifyFile.SaveAs(text2);
			this.ShowMsg("上传成功。", true);
		}
		protected void btnChangeEmailSettings_Click(object sender, System.EventArgs e)
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
			masterSettings.EtaoID = this.txtEtaoID.Text;
			masterSettings.IsCreateFeed = this.rdobltIsCreateFeed.SelectedValue;
			if (!this.ValidationSettings(masterSettings))
			{
				return;
			}
			SettingsManager.Save(masterSettings);
			this.LoadEtaoOpen();
			this.ShowMsg("保存成功。", true);
		}
		private bool ValidationSettings(SiteSettings setting)
		{
			ValidationResults validationResults = Validation.Validate<SiteSettings>(setting, new string[]
			{
				"ValMasterSettings"
			});
			string text = string.Empty;
			if (!validationResults.IsValid)
			{
				foreach (ValidationResult current in (System.Collections.Generic.IEnumerable<ValidationResult>)validationResults)
				{
					text += Formatter.FormatErrorMessage(current.Message);
				}
				this.ShowMsg(text, false);
			}
			return validationResults.IsValid;
		}
	}
}

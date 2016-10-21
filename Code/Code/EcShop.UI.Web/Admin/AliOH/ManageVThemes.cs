using EcShop.ControlPanel.Store;
using EcShop.Core;
using EcShop.Core.Configuration;
using EcShop.Entities.Store;
using EcShop.Membership.Context;
using EcShop.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Web;
using System.Web.UI.WebControls;
using System.Xml;
namespace EcShop.UI.Web.Admin.AliOH
{
	[PrivilegeCheck(Privilege.AliohManageThemes)]
	public class ManageVThemes : AdminPage
	{
		protected System.Web.UI.WebControls.Literal litThemeName;
		protected System.Web.UI.WebControls.Literal lblThemeCount;
		protected System.Web.UI.WebControls.DataList dtManageThemes;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
			this.litThemeName.Text = masterSettings.AliOHTheme;
			this.dtManageThemes.ItemCommand += new System.Web.UI.WebControls.DataListCommandEventHandler(this.dtManageThemes_ItemCommand);
			if (!this.Page.IsPostBack)
			{
				this.BindData(masterSettings);
			}
		}
		private void BindData(SiteSettings siteSettings)
		{
			System.Collections.Generic.IList<ManageThemeInfo> list = this.LoadThemes(siteSettings.AliOHTheme);
			this.dtManageThemes.DataSource = list;
			this.dtManageThemes.DataBind();
			this.lblThemeCount.Text = list.Count.ToString();
		}
		private void dtManageThemes_ItemCommand(object sender, System.Web.UI.WebControls.DataListCommandEventArgs e)
		{
			if (e.Item.ItemType == System.Web.UI.WebControls.ListItemType.Item || e.Item.ItemType == System.Web.UI.WebControls.ListItemType.AlternatingItem)
			{
				string name = this.dtManageThemes.DataKeys[e.Item.ItemIndex].ToString();
				if (e.CommandName == "btnUse")
				{
					this.UserThems(name);
					this.ShowMsg("成功修改了支付宝服务窗模板", true);
				}
			}
		}
		protected void UserThems(string name)
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
			masterSettings.AliOHTheme = name;
			SettingsManager.Save(masterSettings);
			HiCache.Remove("TemplateFileCache");
			this.BindData(masterSettings);
		}
		protected System.Collections.Generic.IList<ManageThemeInfo> LoadThemes(string currentThemeName)
		{
			System.Xml.XmlDocument xmlDocument = new System.Xml.XmlDocument();
			System.Collections.Generic.IList<ManageThemeInfo> list = new System.Collections.Generic.List<ManageThemeInfo>();
			string path = System.Web.HttpContext.Current.Request.PhysicalApplicationPath + HiConfiguration.GetConfig().FilesPath + "\\Templates\\AliOHshop";
			string[] array = System.IO.Directory.Exists(path) ? System.IO.Directory.GetDirectories(path) : null;
			string[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				string path2 = array2[i];
				System.IO.DirectoryInfo directoryInfo = new System.IO.DirectoryInfo(path2);
				string text = directoryInfo.Name.ToLower(System.Globalization.CultureInfo.InvariantCulture);
				if (text.Length > 0 && !text.StartsWith("_"))
				{
					System.IO.FileInfo[] files = directoryInfo.GetFiles("template.xml");
					System.IO.FileInfo[] array3 = files;
					for (int j = 0; j < array3.Length; j++)
					{
						System.IO.FileInfo fileInfo = array3[j];
						ManageThemeInfo manageThemeInfo = new ManageThemeInfo();
						System.IO.FileStream fileStream = fileInfo.OpenRead();
						xmlDocument.Load(fileStream);
						fileStream.Close();
						manageThemeInfo.Name = xmlDocument.SelectSingleNode("root/Name").InnerText;
						manageThemeInfo.ThemeImgUrl = string.Concat(new string[]
						{
							Globals.ApplicationPath,
							"/Templates/AliOHshop/",
							text,
							"/",
							xmlDocument.SelectSingleNode("root/ImageUrl").InnerText
						});
						manageThemeInfo.ThemeName = text;
						if (string.Compare(manageThemeInfo.ThemeName, currentThemeName) == 0)
						{
							this.litThemeName.Text = manageThemeInfo.ThemeName;
						}
						list.Add(manageThemeInfo);
					}
				}
			}
			return list;
		}
	}
}

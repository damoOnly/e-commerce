using EcShop.ControlPanel.Commodities;
using EcShop.Core;
using EcShop.Membership.Context;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI;
namespace EcShop.UI.Common.Controls
{
	[ParseChildren(true), PersistChildren(false)]
	public abstract class WAPTemplatedWebControl : TemplatedWebControl
	{
		private string skinName;
		protected virtual string SkinPath
		{
			get
			{
				string wapshopSkinPath = HiContext.Current.GetWapshopSkinPath(null);
				if (this.SkinName.StartsWith(wapshopSkinPath))
				{
					return this.SkinName;
				}
				if (this.SkinName.StartsWith("/"))
				{
					return wapshopSkinPath + this.SkinName;
				}
				return wapshopSkinPath + "/" + this.SkinName;
			}
		}
		public virtual string SkinName
		{
			get
			{
				return this.skinName;
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					return;
				}
				value = value.ToLower(CultureInfo.InvariantCulture);
				if (!value.EndsWith(".html"))
				{
					return;
				}
				this.skinName = value;
			}
		}
		private bool SkinFileExists
		{
			get
			{
				return !string.IsNullOrEmpty(this.SkinName);
			}
		}
		protected override void OnInit(EventArgs e)
		{
			this.CheckAuth();
			base.OnInit(e);
		}
		protected void CheckAuth()
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
			
			if (masterSettings.OpenWap != 1 && !Globals.IsTestDomain)
			{
				this.Page.Response.Redirect("/Default.aspx");
			}
		}
		protected override void CreateChildControls()
		{
			this.Controls.Clear();
			if (this.LoadHtmlThemedControl())
			{
				this.AttachChildControls();
				return;
			}
			throw new SkinNotFoundException(this.SkinPath);
		}
		protected bool LoadHtmlThemedControl()
		{
			string text = this.ControlText();
			if (!string.IsNullOrEmpty(text))
			{
				Control control = this.Page.ParseControl(text);
				control.ID = "_";
				this.Controls.Add(control);
				return true;
			}
			return false;
		}
		private string ControlText()
		{
			if (!this.SkinFileExists)
			{
				return null;
			}
			StringBuilder stringBuilder = new StringBuilder(File.ReadAllText(this.Page.Request.MapPath(this.SkinPath), Encoding.UTF8));
			if (stringBuilder.Length == 0)
			{
				return null;
			}
			stringBuilder.Replace("<%", "").Replace("%>", "");
			string wapshopSkinPath = HiContext.Current.GetWapshopSkinPath(null);
			stringBuilder.Replace("/images/", wapshopSkinPath + "/images/");
			stringBuilder.Replace("/script/", wapshopSkinPath + "/script/");
			stringBuilder.Replace("/style/", wapshopSkinPath + "/style/");
			stringBuilder.Replace("/utility/", Globals.ApplicationPath + "/utility/");
			stringBuilder.Insert(0, "<%@ Register TagPrefix=\"UI\" Namespace=\"ASPNET.WebControls\" Assembly=\"ASPNET.WebControls\" %>" + Environment.NewLine);
			stringBuilder.Insert(0, "<%@ Register TagPrefix=\"Kindeditor\" Namespace=\"kindeditor.Net\" Assembly=\"kindeditor.Net\" %>" + Environment.NewLine);
			stringBuilder.Insert(0, "<%@ Register TagPrefix=\"Hi\" Namespace=\"EcShop.UI.Common.Validator\" Assembly=\"EcShop.UI.Common.Validator\" %>" + Environment.NewLine);
			stringBuilder.Insert(0, "<%@ Register TagPrefix=\"Hi\" Namespace=\"EcShop.UI.Common.Controls\" Assembly=\"EcShop.UI.Common.Controls\" %>" + Environment.NewLine);
			stringBuilder.Insert(0, "<%@ Register TagPrefix=\"Hi\" Namespace=\"EcShop.UI.SaleSystem.Tags\" Assembly=\"EcShop.UI.SaleSystem.Tags\" %>" + Environment.NewLine);
			stringBuilder.Insert(0, "<%@ Control Language=\"C#\" %>" + Environment.NewLine);
			MatchCollection matchCollection = Regex.Matches(stringBuilder.ToString(), "href(\\s+)?=(\\s+)?\"url:(?<UrlName>.*?)(\\((?<Param>.*?)\\))?\"", RegexOptions.IgnoreCase | RegexOptions.Multiline);
			for (int i = matchCollection.Count - 1; i >= 0; i--)
			{
				int num = matchCollection[i].Groups["UrlName"].Index - 4;
				int num2 = matchCollection[i].Groups["UrlName"].Length + 4;
				if (matchCollection[i].Groups["Param"].Length > 0)
				{
					num2 += matchCollection[i].Groups["Param"].Length + 2;
				}
				stringBuilder.Remove(num, num2);
				stringBuilder.Insert(num, Globals.GetSiteUrls().UrlData.FormatUrl(matchCollection[i].Groups["UrlName"].Value.Trim(), new object[]
				{
					matchCollection[i].Groups["Param"].Value
				}));
			}
			return stringBuilder.ToString();
		}
		public void ReloadPage(NameValueCollection queryStrings)
		{
			this.Page.Response.Redirect(this.GenericReloadUrl(queryStrings));
		}
		public void ReloadPage(NameValueCollection queryStrings, bool endResponse)
		{
			this.Page.Response.Redirect(this.GenericReloadUrl(queryStrings), endResponse);
		}
		private string GenericReloadUrl(NameValueCollection queryStrings)
		{
			if (queryStrings == null || queryStrings.Count == 0)
			{
				return this.Page.Request.Url.AbsolutePath;
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(this.Page.Request.Url.AbsolutePath).Append("?");
			foreach (string text in queryStrings.Keys)
			{
				if (queryStrings[text] != null)
				{
					string text2 = queryStrings[text].Trim();
					if (!string.IsNullOrEmpty(text2) && text2.Length > 0)
					{
						stringBuilder.Append(text).Append("=").Append(this.Page.Server.UrlEncode(text2)).Append("&");
					}
				}
			}
			queryStrings.Clear();
			stringBuilder.Remove(stringBuilder.Length - 1, 1);
			return stringBuilder.ToString();
		}
		protected void GotoResourceNotFound(string errorMsg = "")
		{
			this.Page.Response.Redirect(Globals.ApplicationPath + "/ResourceNotFound_Mobile.aspx?errorMsg=" + errorMsg);
		}

        public string RegisterSitesScript()//ע��վ���б�
        {
            DataTable dtSites = SitesManagementHelper.GetSites();
            string strJson = Newtonsoft.Json.JsonConvert.SerializeObject(dtSites);
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("<script language=\"javascript\" type=\"text/javascript\">");
            stringBuilder.AppendFormat("var sitesList={0}", strJson);
            stringBuilder.AppendLine("</script>");
            return stringBuilder.ToString();
        }
	}
}

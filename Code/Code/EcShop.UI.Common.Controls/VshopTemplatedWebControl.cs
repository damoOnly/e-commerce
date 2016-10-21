using Ecdev.Weixin.MP.Api;
using EcShop.ControlPanel.Commodities;
using EcShop.Core;
using EcShop.Core.ErrorLog;
using EcShop.Membership.Context;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Globalization;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
namespace EcShop.UI.Common.Controls
{
	[ParseChildren(true), PersistChildren(false)]
	public abstract class VshopTemplatedWebControl : TemplatedWebControl
	{
     
		private string skinName;
		protected virtual string SkinPath
		{
			get
			{
				string vshopSkinPath = HiContext.Current.GetVshopSkinPath(null);
				if (this.SkinName.StartsWith(vshopSkinPath))
				{
					return this.SkinName;
				}
				if (this.SkinName.StartsWith("/"))
				{
					return vshopSkinPath + this.SkinName;
				}
				return vshopSkinPath + "/" + this.SkinName;
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
                if (!value.EndsWith(".html") )
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
            //if (masterSettings.OpenVstore != 1 && !Globals.IsTestDomain)
            //{
            //    this.Page.Response.Redirect("/ResourceNotFound_Mobile.aspx?errormsg=抱歉，您暂未开通此服务！");
            //}
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
			string vshopSkinPath = HiContext.Current.GetVshopSkinPath(null);
			stringBuilder.Replace("/images/", vshopSkinPath + "/images/");
			stringBuilder.Replace("/script/", vshopSkinPath + "/script/");
			stringBuilder.Replace("/style/", vshopSkinPath + "/style/");
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
        public void RegisterJSApiScript()
        {
        }
        public string GetJSApiScript()
        {
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
            string weixinAppId = masterSettings.WeixinAppId;

            TimeSpan ts = DateTime.Now.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            string timestamp = Convert.ToInt64(ts.TotalSeconds).ToString();
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("<script>");
            stringBuilder.AppendLine("wx.config({");
            stringBuilder.AppendLine("debug: false, ");
            stringBuilder.AppendLine(string.Format("appId: '{0}', ", weixinAppId));
            stringBuilder.AppendLine(string.Format("timestamp:{0},", timestamp));
            stringBuilder.AppendLine("nonceStr: 'ecshop',");
            string url = HttpContext.Current.Request.Url.AbsoluteUri;
            if (url.ToLower().Contains("vshop/default.aspx"))//为了兼容自定义首页
            {
                url = url.Replace("default.aspx","").Replace("Default.aspx","");
            }
            stringBuilder.AppendLine(string.Format("signature: '{0}',", TicketApi.GetTicketSignature("ecshop", timestamp, url, weixinAppId, masterSettings.WeixinAppSecret)));
            stringBuilder.AppendLine(" jsApiList: [\"scanQRCode\",\"onMenuShareAppMessage\",\"onMenuShareTimeline\",\"onMenuShareQQ\",\"onMenuShareWeibo\",\"getLocation\",\"openLocation\"]");
            stringBuilder.AppendLine("});");
             
            stringBuilder.AppendLine("</script>");
           // ErrorLog.Write(string.Format("注册微信jsapi，{0}", stringBuilder.ToString()));
           //HiCache.Insert("jsapi-" + HttpContext.Current.Request.Url.ToString(), stringBuilder.ToString(), 6900);
            return stringBuilder.ToString();
        }
        public string RegisterSitesScript()//注册站点列表
        {
            DataTable dtSites = SitesManagementHelper.GetSites();
            string strJson = Newtonsoft.Json.JsonConvert.SerializeObject(dtSites);
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("<script language=\"javascript\" type=\"text/javascript\">");
            stringBuilder.AppendFormat("var sitesList={0}", strJson);
            stringBuilder.AppendLine("</script>");
            return stringBuilder.ToString();
        }
		public void RegisterShareScript(string ImgUrl, string lineLink, string descContent, string shareTitle)
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
			string weixinAppId = masterSettings.WeixinAppId;
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("<script language=\"javascript\" type=\"text/javascript\">");
			stringBuilder.AppendLine("var wxShareImgUrl = '" + ImgUrl + "';");
			stringBuilder.AppendLine("var wxSharelineLink = '" + lineLink + "';");
			stringBuilder.AppendLine("var wxSharedescContent = '" + (string.IsNullOrEmpty(descContent) ? "" : descContent.Replace("'", "")) + "';");
			stringBuilder.AppendLine("var wxShareshareTitle = '" + shareTitle + "';");
			stringBuilder.AppendLine("var wxappid = '" + weixinAppId + "';");
			stringBuilder.AppendLine("var wxShareImgWidth = '220';");
			stringBuilder.AppendLine("var wxShareImgHeight = '220';");
			stringBuilder.AppendLine("function weixinShareTimeline(){");
			stringBuilder.AppendLine("WeixinJSBridge.invoke('shareTimeline',{");
			stringBuilder.AppendLine("\"img_url\":wxShareImgUrl,");
			stringBuilder.AppendLine("\"img_width\":wxShareImgWidth,");
			stringBuilder.AppendLine("\"img_height\": wxShareImgHeight,");
			stringBuilder.AppendLine("\"link\": wxSharelineLink,");
			stringBuilder.AppendLine("\"desc\": wxSharedescContent,");
			stringBuilder.AppendLine("\"title\": wxShareshareTitle");
			stringBuilder.AppendLine("});");
			stringBuilder.AppendLine("}");
			stringBuilder.AppendLine("function wxshareFriend() {");
			stringBuilder.AppendLine("WeixinJSBridge.invoke('sendAppMessage', {");
			stringBuilder.AppendLine("\"appid\": wxappid,");
			stringBuilder.AppendLine("\"img_url\": wxShareImgUrl,");
			stringBuilder.AppendLine("\"img_width\": wxShareImgWidth,");
			stringBuilder.AppendLine("\"img_height\": wxShareImgHeight,");
			stringBuilder.AppendLine("\"link\": wxSharelineLink,");
			stringBuilder.AppendLine("\"desc\": wxSharedescContent,");
			stringBuilder.AppendLine("\"title\": wxShareshareTitle");
			stringBuilder.AppendLine("}, function (res) {");
			stringBuilder.AppendLine("})");
			stringBuilder.AppendLine("}");
			stringBuilder.AppendLine("document.addEventListener('WeixinJSBridgeReady', function onBridgeReady() {");
			stringBuilder.AppendLine("WeixinJSBridge.on('menu:share:appmessage', function (argv) {");
			stringBuilder.AppendLine("wxshareFriend();");
			stringBuilder.AppendLine("});");
			stringBuilder.AppendLine("WeixinJSBridge.on('menu:share:timeline', function (argv) {");
			stringBuilder.AppendLine("weixinShareTimeline();");
			stringBuilder.AppendLine("});");
			stringBuilder.AppendLine("},false);");
			stringBuilder.AppendLine("</script>");
			HttpContext.Current.Response.Write(stringBuilder.ToString());
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
	}
}

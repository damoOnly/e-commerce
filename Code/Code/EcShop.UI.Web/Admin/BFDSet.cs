using EcShop.ControlPanel.Store;
using EcShop.Membership.Context;
using EcShop.UI.ControlPanel.Utility;
using System;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace EcShop.UI.Web.Admin
{
	[AdministerCheck(true)]
	public class BFDSet : AdminPage
	{
		private string sign;
		private string mykey = "123456";
		protected System.Web.UI.HtmlControls.HtmlGenericControl div_pan1;
		protected System.Web.UI.WebControls.LinkButton hlinkCreate;
		protected System.Web.UI.HtmlControls.HtmlGenericControl div_pan2;
		protected System.Web.UI.WebControls.LinkButton hplinkSet;
		protected System.Web.UI.HtmlControls.HtmlGenericControl div_total;
		protected System.Web.UI.WebControls.LinkButton LkClose;
		protected System.Web.UI.WebControls.HyperLink Lktoal;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.hlinkCreate.Click += new System.EventHandler(this.hlinkCreate_Click);
			this.hplinkSet.Click += new System.EventHandler(this.hplinkSet_Click);
			if (!base.IsPostBack)
			{
				SiteSettings siteSettings = HiContext.Current.SiteSettings;
				if (string.IsNullOrEmpty(siteSettings.BFDUserName))
				{
					this.div_pan1.Visible = true;
					this.div_pan2.Visible = false;
					this.div_total.Visible = false;
					return;
				}
				this.div_pan1.Visible = false;
				this.div_pan2.Visible = true;
				if (siteSettings.EnabledBFD)
				{
					this.div_pan2.Visible = false;
					this.div_total.Visible = true;
					return;
				}
				this.div_total.Visible = false;
			}
		}
		protected void hlinkCreate_Click(object sender, System.EventArgs e)
		{
			SiteSettings siteSettings = HiContext.Current.SiteSettings;
			string host = this.Page.Request.Url.Host;
			string str = "123456";
			string arg = "hishop";
			string text = System.Guid.NewGuid().ToString().Replace('-', '_');
			string arg2 = host;
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			stringBuilder.Append("{");
			stringBuilder.AppendFormat("\"code\":\"{0}\",", arg);
			stringBuilder.AppendFormat("\"userName\":\"{0}\",", text);
			stringBuilder.AppendFormat("\"password\":\"{0}\",", arg2);
			stringBuilder.AppendFormat("\"realName\":\"{0}\",", "海商");
			stringBuilder.AppendFormat("\"linkName\":\"{0}\",", "长沙海商");
			stringBuilder.AppendFormat("\"phone\":\"{0}\",", "13088888888");
			stringBuilder.AppendFormat("\"email\":\"{0}\",", "admin@Ecdev.com.cn");
			stringBuilder.AppendFormat("\"comName\":\"{0}\",", siteSettings.SiteName);
			stringBuilder.AppendFormat("\"comDomain\":\"{0}\",", host);
			stringBuilder.AppendFormat("\"comAddress\":\"{0}\",", "湖南长沙");
			stringBuilder.AppendFormat("\"bussiness\":\"{0}\",", "1,2");
			stringBuilder.AppendFormat("\"scope\":\"{0}\",", "6,7");
			stringBuilder.AppendFormat("\"pid\":\"{0}\"", "8");
			stringBuilder.Append("}");
			string str2 = APIHelper.Sign("data=" + stringBuilder.ToString() + "&mkey=" + str, "md5", "utf-8");
			string postData = "data=" + stringBuilder.ToString() + "&sign=" + str2;
			string url = "http://passport.baifendian.com/bbgweb/callback/service.do";
			string a = APIHelper.PostData(url, postData);
			if (a == "1000")
			{
				siteSettings.EnabledBFD = false;
				siteSettings.BFDUserName = text;
				this.div_pan1.Visible = false;
				this.div_pan2.Visible = true;
				this.div_total.Visible = false;
				this.hplinkSet.Text = "开启分析引擎";
				SettingsManager.Save(siteSettings);
				this.ShowMsg("创建账号成功", true);
				return;
			}
			this.ShowMsg("创建账号失败", false);
		}
		protected void hplinkSet_Click(object sender, System.EventArgs e)
		{
			SiteSettings siteSettings = HiContext.Current.SiteSettings;
			this.div_pan1.Visible = false;
			this.div_pan2.Visible = false;
			this.div_total.Visible = true;
			siteSettings.EnabledBFD = true;
			SettingsManager.Save(siteSettings);
			this.ShowMsg("开启分析引擎功能成功", true);
		}
		protected void LkClose_Click(object sender, System.EventArgs e)
		{
			SiteSettings siteSettings = HiContext.Current.SiteSettings;
			if (siteSettings.EnabledBFD)
			{
				siteSettings.EnabledBFD = false;
				SettingsManager.Save(siteSettings);
				this.div_pan1.Visible = false;
				this.div_pan2.Visible = true;
				this.div_total.Visible = false;
				this.ShowMsg("关闭分析引擎功能成功", true);
			}
		}
	}
}

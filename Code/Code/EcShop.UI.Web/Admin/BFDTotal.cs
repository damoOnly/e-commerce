using EcShop.ControlPanel.Store;
using EcShop.Membership.Context;
using EcShop.UI.Common.Controls;
using EcShop.UI.ControlPanel.Utility;
using System;
using System.Web.UI.HtmlControls;
namespace EcShop.UI.Web.Admin
{
	public class BFDTotal : AdminPage
	{
		protected HeadContainer HeadContainer1;
		protected PageTitle PageTitle1;
		protected System.Web.UI.HtmlControls.HtmlForm form1;
		[AdministerCheck(true)]
		protected void Page_Load(object sender, System.EventArgs e)
		{
			string str = "123456";
			SiteSettings siteSettings = HiContext.Current.SiteSettings;
			if (!string.IsNullOrEmpty(siteSettings.BFDUserName) && siteSettings.EnabledBFD)
			{
				string str2 = APIHelper.Sign("code=hishop&username=" + siteSettings.BFDUserName + "&mkey=" + str, "md5", "utf-8");
				string url = "http://passport.baifendian.com/baeweb/main.html?code=hishop&username=" + siteSettings.BFDUserName + "&sign=" + str2;
				base.Response.Redirect(url);
				return;
			}
			this.Page.Response.Redirect("BFDset.aspx");
		}
	}
}

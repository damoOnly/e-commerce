using EcShop.ControlPanel.Store;
using EcShop.Membership.Context;
using EcShop.UI.ControlPanel.Utility;
using System;
using System.Web.UI.HtmlControls;
namespace EcShop.UI.Web.Admin
{
	[AdministerCheck(true)]
	public class CnzzStatisticTotal : AdminPage
	{
		protected System.Web.UI.HtmlControls.HtmlGenericControl framcnz;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			SiteSettings siteSettings = HiContext.Current.SiteSettings;
			if (!string.IsNullOrEmpty(siteSettings.CnzzPassword) && !string.IsNullOrEmpty(siteSettings.CnzzUsername))
			{
				this.framcnz.Attributes["src"] = "http://wss.cnzz.com/user/companion/92hi_login.php?site_id=" + siteSettings.CnzzUsername + "&password=" + siteSettings.CnzzPassword;
				return;
			}
			this.Page.Response.Redirect("cnzzstatisticsset.aspx");
		}
	}
}

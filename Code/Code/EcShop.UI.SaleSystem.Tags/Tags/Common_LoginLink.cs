using EcShop.Core;
using EcShop.Membership.Context;
using EcShop.Membership.Core.Enums;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace EcShop.UI.SaleSystem.Tags
{
	public class Common_LoginLink : HyperLink
	{
		protected override void Render(HtmlTextWriter writer)
		{
			if (HiContext.Current.User.UserRole == UserRole.Member)
			{
				base.Text = "退出";
				base.NavigateUrl = Globals.GetSiteUrls().UrlData.FormatUrl("logout");
			}
			else
			{
				base.Text = "登录";
				base.NavigateUrl = Globals.GetSiteUrls().UrlData.FormatUrl("login_clean");
			}
			base.Render(writer);
		}
	}
}

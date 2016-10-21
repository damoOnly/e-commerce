using EcShop.Core;
using EcShop.Membership.Context;
using EcShop.Membership.Core.Enums;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace EcShop.UI.SaleSystem.Tags
{
	public class Common_MyAccountLink : HyperLink
	{
		protected override void Render(HtmlTextWriter writer)
		{
			if (HiContext.Current.User.UserRole == UserRole.Member)
			{
                base.Text = HiContext.Current.User.Username;
				base.NavigateUrl = Globals.GetSiteUrls().UrlData.FormatUrl("user_UserDefault");
			}
			else
			{
				base.Text = "注册";
				base.NavigateUrl = Globals.GetSiteUrls().UrlData.FormatUrl("register");
			}
			base.Render(writer);
		}
	}
}

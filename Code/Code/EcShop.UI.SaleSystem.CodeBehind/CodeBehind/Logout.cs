using EcShop.Core;
using EcShop.Membership.Context;
using EcShop.Membership.Core;
using System;
using System.Web;
using System.Web.Security;
using System.Web.UI;
namespace EcShop.UI.SaleSystem.CodeBehind
{
	public class Logout : System.Web.UI.Page
	{
		protected override void OnLoad(System.EventArgs e)
		{
			base.OnLoad(e);
			System.Web.HttpCookie httpCookie = HiContext.Current.Context.Request.Cookies["Token_" + HiContext.Current.User.UserId.ToString()];
			if (httpCookie != null && !string.IsNullOrEmpty(httpCookie.Value))
			{
				httpCookie.Expires = System.DateTime.Now;
				System.Web.HttpContext.Current.Response.Cookies.Add(httpCookie);
			}
			System.Web.HttpCookie httpCookie2 = HiContext.Current.Context.Request.Cookies["Vshop-Member"];
			if (httpCookie2 != null && !string.IsNullOrEmpty(httpCookie2.Value))
			{
				httpCookie2.Expires = System.DateTime.Now;
				System.Web.HttpContext.Current.Response.Cookies.Add(httpCookie2);
			}
			if (this.Context.Request.IsAuthenticated)
			{
				System.Web.Security.FormsAuthentication.SignOut();
				System.Web.HttpCookie authCookie = System.Web.Security.FormsAuthentication.GetAuthCookie(HiContext.Current.User.Username, true);
				IUserCookie userCookie = HiContext.Current.User.GetUserCookie();
				if (userCookie != null)
				{
					userCookie.DeleteCookie(authCookie);
				}
				RoleHelper.SignOut(HiContext.Current.User.Username);
				this.Context.Response.Cookies["hishopLoginStatus"].Value = "";
			}

            HiCache.Remove("DataCache-UserLookuptable");
			this.Context.Response.Redirect(Globals.GetSiteUrls().Home, true);
		}
	}
}

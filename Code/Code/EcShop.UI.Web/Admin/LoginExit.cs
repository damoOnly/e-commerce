using EcShop.Membership.Context;
using EcShop.Membership.Core;
using System;
using System.Web;
using System.Web.Security;
using System.Web.UI;
namespace EcShop.UI.Web.Admin
{
	public class LoginExit : System.Web.UI.Page
	{
		protected void Page_Load(object sender, System.EventArgs e)
		{
			System.Web.Security.FormsAuthentication.SignOut();
			System.Web.HttpCookie authCookie = System.Web.Security.FormsAuthentication.GetAuthCookie(HiContext.Current.User.Username, true);
			IUserCookie userCookie = HiContext.Current.User.GetUserCookie();
			if (userCookie != null)
			{
				userCookie.DeleteCookie(authCookie);
			}
			RoleHelper.SignOut(HiContext.Current.User.Username);
			base.Response.Redirect("Login.aspx", true);
		}
	}
}

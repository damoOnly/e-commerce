using EcShop.Core;
using EcShop.Membership.Context;
using EcShop.Membership.Core;
using System;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
namespace EcShop.UI.Web.AppShop
{
	public class AppLogin : System.Web.UI.Page
	{
		private string sessionId;
		protected System.Web.UI.HtmlControls.HtmlForm form1;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.sessionId = this.Page.Request.QueryString["sessionId"];
			if (string.IsNullOrEmpty(this.sessionId))
			{
				System.Web.HttpCookie httpCookie = HiContext.Current.Context.Request.Cookies["Vshop-Member"];
				if (httpCookie != null && !string.IsNullOrEmpty(httpCookie.Value))
				{
					httpCookie.Expires = System.DateTime.Now;
					System.Web.HttpContext.Current.Response.Cookies.Add(httpCookie);
				}
				return;
			}
			Member member = Users.GetUserBySessionId(this.sessionId) as Member;
			if (member == null || member.IsAnonymous)
			{
				return;
			}

            string name = "Vshop-Member";
            HttpCookie httpCookie2 = new HttpCookie("Vshop-Member");
            httpCookie2.Value = Globals.UrlEncode(member.Username);
            httpCookie2.Expires = System.DateTime.Now.AddDays(7);
            httpCookie2.Domain = HttpContext.Current.Request.Url.Host;
            if (HttpContext.Current.Response.Cookies[name] != null)
            {
                HttpContext.Current.Response.Cookies.Remove(name);
            }
            HttpContext.Current.Response.Cookies.Add(httpCookie2);


			HiContext.Current.User = member;
			member.OnLogin();
		}
		public void ClearLoginStatus()
		{
			try
			{
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
				if (System.Web.HttpContext.Current.Request.IsAuthenticated)
				{
					System.Web.Security.FormsAuthentication.SignOut();
					System.Web.HttpCookie authCookie = System.Web.Security.FormsAuthentication.GetAuthCookie(HiContext.Current.User.Username, true);
					IUserCookie userCookie = HiContext.Current.User.GetUserCookie();
					if (userCookie != null)
					{
						userCookie.DeleteCookie(authCookie);
					}
					RoleHelper.SignOut(HiContext.Current.User.Username);
					System.Web.HttpContext.Current.Response.Cookies["hishopLoginStatus"].Value = "";
				}
			}
			catch
			{
			}
		}
	}
}

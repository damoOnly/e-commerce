using System;
using System.Web;
using System.Web.Security;
namespace EcShop.UI.Web
{
	public class Global : System.Web.HttpApplication
	{
		protected void Application_Start(object sender, System.EventArgs e)
		{
		}
		protected void Session_Start(object sender, System.EventArgs e)
		{
		}
		protected void Application_BeginRequest(object sender, System.EventArgs e)
		{
			try
			{
				string name = "AUTHID";
				string formsCookieName = System.Web.Security.FormsAuthentication.FormsCookieName;
				if (System.Web.HttpContext.Current.Request.Form[name] != null)
				{
					this.UpdateCookie(formsCookieName, System.Web.HttpContext.Current.Request.Form[name]);
				}
				else
				{
					if (System.Web.HttpContext.Current.Request.QueryString[name] != null)
					{
						this.UpdateCookie(formsCookieName, System.Web.HttpContext.Current.Request.QueryString[name]);
					}
				}
			}
			catch (System.Exception)
			{
			}
		}
		private void UpdateCookie(string cookie_name, string cookie_value)
		{
			System.Web.HttpCookie httpCookie = System.Web.HttpContext.Current.Request.Cookies.Get(cookie_name);
			if (httpCookie == null)
			{
				System.Web.HttpCookie cookie = new System.Web.HttpCookie(cookie_name, cookie_value);
				base.Response.Cookies.Add(cookie);
				return;
			}
			httpCookie.Value = cookie_value;
			System.Web.HttpContext.Current.Request.Cookies.Set(httpCookie);
		}
		protected void Application_AuthenticateRequest(object sender, System.EventArgs e)
		{
		}
		protected void Application_Error(object sender, System.EventArgs e)
		{
		}
		protected void Session_End(object sender, System.EventArgs e)
		{
		}
		protected void Application_End(object sender, System.EventArgs e)
		{
		}
	}
}

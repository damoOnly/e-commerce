using EcShop.Membership.Core;
using System;
using System.Text.RegularExpressions;
using System.Web;
namespace EcShop.Membership.Context
{
	public class UserCookie : IUserCookie
	{
		private readonly HiContext hiContext = HiContext.Current;
		private readonly HttpContext context = null;
		public UserCookie(IUser user)
		{
			if (user != null && !user.IsAnonymous)
			{
				this.context = this.hiContext.Context;
			}
		}
		public void WriteCookie(HttpCookie cookie, int days, bool autoLogin)
		{
			if (cookie != null && this.context != null)
			{
				this.SetCookieDomain(cookie);
				if (autoLogin)
				{
					cookie.Expires = System.DateTime.Now.AddDays((double)days);
				}
				this.context.Response.Cookies.Add(cookie);
			}
		}
		public void DeleteCookie(HttpCookie cookie)
		{
			if (cookie != null && this.context != null)
			{
				this.SetCookieDomain(cookie);
				cookie.Expires = new System.DateTime(1911, 10, 12);
				this.context.Response.Cookies.Add(cookie);
			}
		}
		private void SetCookieDomain(HttpCookie cookie)
		{
			Regex regex = new Regex("[_a-zA-Z0-9-]+(\\.[_a-zA-Z0-9-]+)+$", RegexOptions.IgnoreCase | RegexOptions.Compiled);
			if (regex.IsMatch(this.context.Request.Url.Host))
			{
				if (this.context.Request.Url.Host.ToLower().EndsWith(this.hiContext.SiteSettings.SiteUrl.ToLower()))
				{
					cookie.Path = "/";
					cookie.Domain = this.hiContext.SiteSettings.SiteUrl;
				}
			}
		}
	}
}

using EcShop.Membership.Context;
using System;
using System.Web;
using System.Web.UI;
namespace EcShop.UI.Common.Controls
{
	[ParseChildren(true), PersistChildren(false)]
	public abstract class WAPMemberTemplatedWebControl : WAPTemplatedWebControl
	{
		protected WAPMemberTemplatedWebControl()
		{
			if (!(HiContext.Current.User is Member))
			{
				this.Page.Response.Redirect("Login.aspx?returnUrl=" + HttpUtility.UrlEncode(HttpContext.Current.Request.Url.ToString()));
			}
		}
	}
}

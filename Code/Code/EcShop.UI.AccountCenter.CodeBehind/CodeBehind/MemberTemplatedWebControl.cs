using EcShop.Core;
using EcShop.Membership.Context;
using EcShop.Membership.Core.Enums;
using EcShop.UI.Common.Controls;
using System;
using System.Web.UI;
namespace EcShop.UI.AccountCenter.CodeBehind
{
	[System.Web.UI.ParseChildren(true), System.Web.UI.PersistChildren(false)]
	public abstract class MemberTemplatedWebControl : HtmlTemplatedWebControl
	{
		protected MemberTemplatedWebControl()
		{
			if (HiContext.Current.User.UserRole != UserRole.Member)
			{
				this.Page.Response.Redirect(Globals.GetSiteUrls().UrlData.FormatUrl("login", new object[]
				{
					this.Page.Request.RawUrl
				}), true);
			}
		}
	}
}

using Ecdev.Weixin.MP.Api;
using Ecdev.Weixin.MP.Domain;
using EcShop.ControlPanel.Promotions;
using EcShop.Core;
using EcShop.Core.ErrorLog;
using EcShop.Entities.Promotions;
using EcShop.Membership.Context;
using EcShop.Membership.Core;
using EcShop.Membership.Core.Enums;
using EcShop.SaleSystem.Vshop;
using EcShop.UI.Common.Controls;
using System;
using System.Configuration;
using System.Web;
using System.Web.UI;
namespace EcShop.UI.SaleSystem.CodeBehind
{
	[System.Web.UI.ParseChildren(true)]
	public class VLogin : VshopTemplatedWebControl
	{
		private string openId;
		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "skin-VLogin.html";
			}
			base.OnInit(e);
		}
		protected override void AttachChildControls()
		{
			this.openId = this.Page.Request.QueryString["sessionId"];
			Member member = null;
			if (!string.IsNullOrEmpty(this.openId))
			{
				member = (Users.GetUserByOpenId(this.openId) as Member);
			}
			Member member2 = HiContext.Current.User as Member;
			if (member2 != null)
			{
				if (string.IsNullOrEmpty(member2.OpenId) && member == null)
				{
					member2.OpenId = this.openId;
					Users.UpdateUser(member2);
				}
				member = member2;
			}
            if (member != null)
            {
                string name = "Vshop-Member";
                HttpCookie httpCookie = new HttpCookie("Vshop-Member");
                httpCookie.Value = Globals.UrlEncode(member.Username);
                httpCookie.Expires = System.DateTime.Now.AddDays(7);
                httpCookie.Domain = HttpContext.Current.Request.Url.Host;
                if (HttpContext.Current.Response.Cookies[name] != null)
                {
                    HttpContext.Current.Response.Cookies.Remove(name);
                }
                HttpContext.Current.Response.Cookies.Add(httpCookie);
                this.Page.Response.Redirect("/Vshop/MemberCenter.aspx");
            }
			PageTitle.AddSiteNameTitle("登录");
		}
	}
}

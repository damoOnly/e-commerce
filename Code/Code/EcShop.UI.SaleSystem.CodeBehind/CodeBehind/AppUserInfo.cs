using EcShop.Core;
using EcShop.Membership.Context;
using EcShop.UI.Common.Controls;
using System;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace EcShop.UI.SaleSystem.CodeBehind
{
	[System.Web.UI.ParseChildren(true)]
	public class AppUserInfo : AppshopTemplatedWebControl
	{
		private System.Web.UI.WebControls.Literal litUserLink;
		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "skin-VUserInfo.html";
			}
			base.OnInit(e);
		}
		protected override void AttachChildControls()
		{
			this.litUserLink = (System.Web.UI.WebControls.Literal)this.FindControl("litUserLink");
			if (this.litUserLink != null)
			{
				System.Uri url = System.Web.HttpContext.Current.Request.Url;
				string text = (url.Port == 80) ? string.Empty : (":" + url.Port.ToString(System.Globalization.CultureInfo.InvariantCulture));
				this.litUserLink.Text = string.Concat(new object[]
				{
					string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0}://{1}{2}", new object[]
					{
						url.Scheme,
						url.Host,
						text
					}),
					Globals.ApplicationPath,
					"/?ReferralUserId=",
					HiContext.Current.User.UserId
				});
			}
			Member member = HiContext.Current.User as Member;
			if (member != null)
			{
				System.Web.UI.HtmlControls.HtmlInputText control = (System.Web.UI.HtmlControls.HtmlInputText)this.FindControl("txtUserName");
				System.Web.UI.HtmlControls.HtmlInputText control2 = (System.Web.UI.HtmlControls.HtmlInputText)this.FindControl("txtRealName");
				System.Web.UI.HtmlControls.HtmlInputText control3 = (System.Web.UI.HtmlControls.HtmlInputText)this.FindControl("txtPhone");
				System.Web.UI.HtmlControls.HtmlInputText control4 = (System.Web.UI.HtmlControls.HtmlInputText)this.FindControl("txtEmail");
				control.SetWhenIsNotNull(member.Username);
				control2.SetWhenIsNotNull(member.RealName);
				control3.SetWhenIsNotNull(member.CellPhone);
				control4.SetWhenIsNotNull(member.QQ);
			}
			PageTitle.AddSiteNameTitle("修改用户信息");
		}
	}
}

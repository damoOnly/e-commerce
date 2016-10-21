using EcShop.Core;
using EcShop.Membership.Context;
using EcShop.UI.Common.Controls;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace EcShop.UI.AccountCenter.CodeBehind
{
	[System.Web.UI.ParseChildren(true)]
	public class ReferralRegisterresults : MemberTemplatedWebControl
	{
		private System.Web.UI.WebControls.Literal litReferralRegisterresults;
		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "User/Skin-ReferralRegisterresults.html";
			}
			base.OnInit(e);
		}
		protected override void AttachChildControls()
		{
			this.litReferralRegisterresults = (System.Web.UI.WebControls.Literal)this.FindControl("litReferralRegisterresults");
			PageTitle.AddSiteNameTitle("我要成为推广员");
			Member member = Users.GetUser(HiContext.Current.User.UserId, false) as Member;
			if (member.ReferralStatus == 2)
			{
				this.Page.Response.Redirect(Globals.ApplicationPath + "/user/PopularizeGift.aspx");
				return;
			}
			if (member.ReferralStatus == 1)
			{
				this.litReferralRegisterresults.Text = "您提交的申请正在审核中...";
				return;
			}
			if (member.ReferralStatus == 3)
			{
				this.litReferralRegisterresults.Text = "审核被拒绝，理由：" + member.RefusalReason;
			}
		}
	}
}

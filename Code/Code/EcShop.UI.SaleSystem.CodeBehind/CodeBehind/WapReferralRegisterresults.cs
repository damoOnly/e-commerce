using EcShop.Core;
using EcShop.Membership.Context;
using EcShop.UI.Common.Controls;
using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace EcShop.UI.SaleSystem.CodeBehind
{
	[System.Web.UI.ParseChildren(true)]
	public class WapReferralRegisterresults : WAPMemberTemplatedWebControl
	{
		private System.Web.UI.WebControls.Literal litReferralRegisterresults;
		private System.Web.UI.WebControls.Literal litRefuseReasons;
		private System.Web.UI.HtmlControls.HtmlGenericControl divRefuseReasons;
		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-ReferralRegisterresults.html";
			}
			base.OnInit(e);
		}
		protected override void AttachChildControls()
		{
			this.litReferralRegisterresults = (System.Web.UI.WebControls.Literal)this.FindControl("litReferralRegisterresults");
			this.litRefuseReasons = (System.Web.UI.WebControls.Literal)this.FindControl("litRefuseReasons");
			this.divRefuseReasons = (System.Web.UI.HtmlControls.HtmlGenericControl)this.FindControl("divRefuseReasons");
			PageTitle.AddSiteNameTitle("我要成为推广员");
			Member member = Users.GetUser(HiContext.Current.User.UserId, false) as Member;
			if (member.ReferralStatus == 2)
			{
				this.Page.Response.Redirect(Globals.ApplicationPath + "/WapShop/Referral.aspx");
				return;
			}
			if (member.ReferralStatus == 1)
			{
				this.litReferralRegisterresults.Text = "您提交的申请正在审核中...";
				if (this.divRefuseReasons != null)
				{
					this.divRefuseReasons.Visible = false;
					return;
				}
			}
			else
			{
				if (member.ReferralStatus == 3)
				{
					this.litReferralRegisterresults.Text = "您提交的申请被拒绝了...";
					this.litRefuseReasons.Text = "拒绝理由：" + member.RefusalReason;
				}
			}
		}
	}
}

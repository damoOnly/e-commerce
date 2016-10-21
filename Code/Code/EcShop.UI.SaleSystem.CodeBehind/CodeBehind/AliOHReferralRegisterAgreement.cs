using EcShop.Core;
using EcShop.Membership.Context;
using EcShop.UI.Common.Controls;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace EcShop.UI.SaleSystem.CodeBehind
{
	[System.Web.UI.ParseChildren(true)]
	public class AliOHReferralRegisterAgreement : AliOHMemberTemplatedWebControl
	{
		private System.Web.UI.WebControls.Literal litReferralRegisterAgreement;
		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-ReferralRegisterAgreement.html";
			}
			base.OnInit(e);
		}
		protected override void AttachChildControls()
		{
			this.litReferralRegisterAgreement = (System.Web.UI.WebControls.Literal)this.FindControl("litReferralRegisterAgreement");
			Member member = Users.GetUser(HiContext.Current.User.UserId) as Member;
			if (member.ReferralStatus != 0)
			{
				this.Page.Response.Redirect(Globals.ApplicationPath + "/AliOH/ReferralRegisterresults.aspx");
			}
			PageTitle.AddSiteNameTitle("我要成为推广员");
			this.litReferralRegisterAgreement.Text = HiContext.Current.SiteSettings.ReferralIntroduction;
		}
	}
}

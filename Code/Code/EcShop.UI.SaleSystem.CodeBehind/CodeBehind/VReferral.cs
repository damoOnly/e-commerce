using EcShop.Entities.Members;
using EcShop.Membership.Context;
using EcShop.SaleSystem.Member;
using EcShop.UI.Common.Controls;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace EcShop.UI.SaleSystem.CodeBehind
{
	[System.Web.UI.ParseChildren(true)]
	public class VReferral : VMemberTemplatedWebControl
	{
		private System.Web.UI.WebControls.Literal litUserName;
		private System.Web.UI.WebControls.Literal litExpenditure;
		private System.Web.UI.WebControls.Literal litPoints;
		private System.Web.UI.WebControls.Literal litMemberGrade;
		private System.Web.UI.WebControls.Literal litWaitForRecieveCount;
		private System.Web.UI.WebControls.Literal litWaitForPayCount;
		private System.Web.UI.WebControls.Literal litPaymentBalance;
		private System.Web.UI.WebControls.Literal litUserLink;
		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-Referral.html";
			}
			base.OnInit(e);
		}
		protected override void AttachChildControls()
		{
			PageTitle.AddSiteNameTitle("推广员");
			Member member = HiContext.Current.User as Member;
			if (member == null)
			{
				return;
			}
			this.litUserLink = (System.Web.UI.WebControls.Literal)this.FindControl("litUserLink");
			this.litUserName = (System.Web.UI.WebControls.Literal)this.FindControl("litUserName");
			this.litPaymentBalance = (System.Web.UI.WebControls.Literal)this.FindControl("litPaymentBalance");
			this.litExpenditure = (System.Web.UI.WebControls.Literal)this.FindControl("litExpenditure");
			this.litExpenditure.SetWhenIsNotNull(member.Expenditure.ToString("F2"));
			this.litPaymentBalance.SetWhenIsNotNull(member.Balance.ToString("F2"));
			this.litPoints = (System.Web.UI.WebControls.Literal)this.FindControl("litPoints");
			this.litPoints.SetWhenIsNotNull(member.Points.ToString("F2"));
			this.litMemberGrade = (System.Web.UI.WebControls.Literal)this.FindControl("litMemberGrade");
			MemberGradeInfo memberGrade = MemberProcessor.GetMemberGrade(member.GradeId);
			if (memberGrade != null)
			{
				this.litMemberGrade.SetWhenIsNotNull(memberGrade.Name);
			}
			this.litUserName.Text = (string.IsNullOrEmpty(member.RealName) ? member.Username : member.RealName);
		}
	}
}

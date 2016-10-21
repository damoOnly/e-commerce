using EcShop.Core;
using EcShop.Entities.Members;
using EcShop.Entities.Orders;
using EcShop.Membership.Context;
using EcShop.SaleSystem.Member;
using EcShop.UI.Common.Controls;
using System;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace EcShop.UI.SaleSystem.CodeBehind
{
	[System.Web.UI.ParseChildren(true)]
	public class AliOHMemberCenter : AliOHMemberTemplatedWebControl
	{
		private System.Web.UI.WebControls.Literal litUserLink;
		private System.Web.UI.WebControls.Literal litUserName;
		private System.Web.UI.WebControls.Literal litExpenditure;
		private System.Web.UI.WebControls.Literal litPoints;
		private System.Web.UI.WebControls.Literal litMemberGrade;
		private System.Web.UI.WebControls.Literal litWaitForRecieveCount;
		private System.Web.UI.WebControls.Literal litWaitForPayCount;
		private System.Web.UI.WebControls.Literal litPaymentBalance;
		private System.Web.UI.WebControls.HyperLink referralLink;
		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-VMemberCenter.html";
			}
			base.OnInit(e);
		}
		protected override void AttachChildControls()
		{
			PageTitle.AddSiteNameTitle("会员中心");
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
			this.litPoints = (System.Web.UI.WebControls.Literal)this.FindControl("litPoints");
			if (this.litPoints != null)
			{
				this.litPoints.SetWhenIsNotNull(member.Points.ToString("F2"));
			}
			this.referralLink = (System.Web.UI.WebControls.HyperLink)this.FindControl("referralLink");
			this.litPaymentBalance.SetWhenIsNotNull(member.Balance.ToString("F2"));
			this.litMemberGrade = (System.Web.UI.WebControls.Literal)this.FindControl("litMemberGrade");
			MemberGradeInfo memberGrade = MemberProcessor.GetMemberGrade(member.GradeId);
			if (memberGrade != null)
			{
				this.litMemberGrade.SetWhenIsNotNull(memberGrade.Name);
			}
			this.litUserName.Text = (string.IsNullOrEmpty(member.RealName) ? member.Username : member.RealName);
			this.litWaitForRecieveCount = (System.Web.UI.WebControls.Literal)this.FindControl("litWaitForRecieveCount");
			this.litWaitForPayCount = (System.Web.UI.WebControls.Literal)this.FindControl("litWaitForPayCount");
			OrderQuery orderQuery = new OrderQuery();
			orderQuery.Status = OrderStatus.WaitBuyerPay;
			int userOrderCount = MemberProcessor.GetUserOrderCount(HiContext.Current.User.UserId, orderQuery);
			this.litWaitForPayCount.SetWhenIsNotNull(userOrderCount.ToString());
			orderQuery.Status = OrderStatus.SellerAlreadySent;
			userOrderCount = MemberProcessor.GetUserOrderCount(HiContext.Current.User.UserId, orderQuery);
			this.litWaitForRecieveCount.SetWhenIsNotNull(userOrderCount.ToString());
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
					"/AliOH/?ReferralUserId=",
					HiContext.Current.User.UserId
				});
			}
			if (this.referralLink != null)
			{
				this.referralLink.CssClass = "list-group-item";
				if (member.ReferralStatus == 0 || member.ReferralStatus == 1 || member.ReferralStatus == 3)
				{
					this.referralLink.Text = "申请成为推广员";
					if (member.ReferralStatus == 1 || member.ReferralStatus == 3)
					{
						this.referralLink.NavigateUrl = "/AliOH/ReferralRegisterresults.aspx";
					}
					else
					{
						this.referralLink.NavigateUrl = "/AliOH/ReferralRegisterAgreement.aspx";
					}
				}
				if (member.ReferralStatus == 2)
				{
					this.referralLink.Text = "推广员";
					this.referralLink.NavigateUrl = "/AliOH/Referral.aspx";
				}
			}
		}
	}
}

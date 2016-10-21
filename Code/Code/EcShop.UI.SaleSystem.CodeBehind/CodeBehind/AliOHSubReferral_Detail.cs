using EcShop.Entities.Members;
using EcShop.SaleSystem.Member;
using EcShop.UI.Common.Controls;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace EcShop.UI.SaleSystem.CodeBehind
{
	[System.Web.UI.ParseChildren(true)]
	public class AliOHSubReferral_Detail : AliOHMemberTemplatedWebControl
	{
		private System.Web.UI.WebControls.Literal litUsername;
		private System.Web.UI.WebControls.Literal litTrueName;
		private System.Web.UI.WebControls.Literal litTelphone;
		private System.Web.UI.WebControls.Literal litOrderCount;
		private System.Web.UI.WebControls.Literal litAuditTime;
		private System.Web.UI.WebControls.Literal litLastOrderTime;
		private FormatedMoneyLabel litAmount;
		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-SubReferral_Detail.html";
			}
			base.OnInit(e);
		}
		protected override void AttachChildControls()
		{
			this.litAmount = (FormatedMoneyLabel)this.FindControl("litAmount");
			this.litUsername = (System.Web.UI.WebControls.Literal)this.FindControl("litUsername");
			this.litTrueName = (System.Web.UI.WebControls.Literal)this.FindControl("litTrueName");
			this.litOrderCount = (System.Web.UI.WebControls.Literal)this.FindControl("litOrderCount");
			this.litTelphone = (System.Web.UI.WebControls.Literal)this.FindControl("litTelphone");
			this.litAuditTime = (System.Web.UI.WebControls.Literal)this.FindControl("litAuditTime");
			this.litLastOrderTime = (System.Web.UI.WebControls.Literal)this.FindControl("litLastOrderTime");
			PageTitle.AddSiteNameTitle("下级推广员详情");
			int userId = 0;
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["UserID"]))
			{
				int.TryParse(this.Page.Request.QueryString["UserID"], out userId);
			}
			SubReferralUser myReferralSubUser = MemberProcessor.GetMyReferralSubUser(userId);
			if (myReferralSubUser == null)
			{
				this.ShowMessage("错误的推广员ID", false);
			}
			this.litAmount.Money = myReferralSubUser.SubReferralSplittin;
			this.litUsername.Text = myReferralSubUser.UserName;
			this.litTrueName.Text = myReferralSubUser.RealName;
			this.litOrderCount.Text = myReferralSubUser.ReferralOrderNumber.ToString();
			this.litTelphone.Text = myReferralSubUser.CellPhone;
			this.litAuditTime.Text = (myReferralSubUser.ReferralAuditDate.HasValue ? myReferralSubUser.ReferralAuditDate.Value.ToString("yyyy-MM-dd hh:mm:ss") : "");
			this.litLastOrderTime.Text = (myReferralSubUser.LastReferralDate.HasValue ? myReferralSubUser.LastReferralDate.Value.ToString("yyyy-MM-dd hh:mm:ss") : "");
		}
	}
}

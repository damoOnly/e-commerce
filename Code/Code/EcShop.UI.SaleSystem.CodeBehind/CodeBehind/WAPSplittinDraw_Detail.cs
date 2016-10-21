using EcShop.Entities.Members;
using EcShop.SaleSystem.Member;
using EcShop.UI.Common.Controls;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace EcShop.UI.SaleSystem.CodeBehind
{
	[System.Web.UI.ParseChildren(true)]
	public class WAPSplittinDraw_Detail : WAPMemberTemplatedWebControl
	{
		private System.Web.UI.WebControls.Literal litRequestDate;
		private System.Web.UI.WebControls.Literal litAccount;
		private System.Web.UI.WebControls.Literal litAccountDate;
		private System.Web.UI.WebControls.Literal litStatus;
		private System.Web.UI.WebControls.Literal litMark;
		private FormatedMoneyLabel litAmount;
		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-SplitinDraw_Detail.html";
			}
			base.OnInit(e);
		}
		protected override void AttachChildControls()
		{
			this.litAmount = (FormatedMoneyLabel)this.FindControl("litAmount");
			this.litRequestDate = (System.Web.UI.WebControls.Literal)this.FindControl("litRequestDate");
			this.litAccount = (System.Web.UI.WebControls.Literal)this.FindControl("litAccount");
			this.litAccountDate = (System.Web.UI.WebControls.Literal)this.FindControl("litAccountDate");
			this.litStatus = (System.Web.UI.WebControls.Literal)this.FindControl("litStatus");
			this.litMark = (System.Web.UI.WebControls.Literal)this.FindControl("litMark");
			PageTitle.AddSiteNameTitle("提现详情");
			long journalNumber = 0L;
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["id"]))
			{
				long.TryParse(this.Page.Request.QueryString["id"], out journalNumber);
			}
			SplittinDrawInfo splittinDraw = MemberProcessor.GetSplittinDraw(journalNumber);
			if (splittinDraw == null)
			{
				this.ShowMessage("错误的提现记录ID", false);
			}
			this.litAmount.Money = splittinDraw.Amount;
			this.litRequestDate.Text = splittinDraw.RequestDate.ToString("yyyy-MM-dd hh:mm:ss");
			this.litMark.Text = splittinDraw.ManagerRemark;
			this.litAccount.Text = splittinDraw.Account;
			if (splittinDraw.AuditStatus != 1 && splittinDraw.AccountDate.HasValue)
			{
				this.litAccountDate.Text = splittinDraw.AccountDate.Value.ToString("yyyy-MM-dd hh:mm:ss");
			}
			this.litStatus.Text = ((splittinDraw.AuditStatus == 1) ? "未审核" : "已审核");
		}
	}
}

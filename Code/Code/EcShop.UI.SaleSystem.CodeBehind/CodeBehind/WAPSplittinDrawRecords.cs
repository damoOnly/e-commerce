using EcShop.Core.Entities;
using EcShop.Entities.Members;
using EcShop.Membership.Context;
using EcShop.SaleSystem.Member;
using EcShop.UI.Common.Controls;
using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
namespace EcShop.UI.SaleSystem.CodeBehind
{
	[System.Web.UI.ParseChildren(true)]
	public class WAPSplittinDrawRecords : WAPMemberTemplatedWebControl
	{
        private System.Web.UI.WebControls.Literal litPaymentBalance;
        private System.Web.UI.WebControls.Literal litBalanceDrawRequestAmount;
		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-SplittinDrawRecords.html";
			}
			base.OnInit(e);
		}
		protected override void AttachChildControls()
		{
            Member member = HiContext.Current.User as Member;
            if (member == null)
            {
                this.Page.Response.Redirect("/WapShop/Login.aspx");
            }
            this.litPaymentBalance = (System.Web.UI.WebControls.Literal)this.FindControl("litPaymentBalance");
            this.litBalanceDrawRequestAmount = (System.Web.UI.WebControls.Literal)this.FindControl("litBalanceDrawRequestAmount");
            if (litPaymentBalance != null)
            {
                this.litPaymentBalance.SetWhenIsNotNull(member.Balance.ToString("F2"));
            }
			PageTitle.AddSiteNameTitle("提现记录");
            BindDrawRecords();
		}
		private void BindDrawRecords()
		{
            decimal amount = MemberProcessor.GetBalanceAmountByUserId(HiContext.Current.User.UserId, "Expenses");
            litBalanceDrawRequestAmount.SetWhenIsNotNull(amount.ToString("F2"));
		}
	}
}

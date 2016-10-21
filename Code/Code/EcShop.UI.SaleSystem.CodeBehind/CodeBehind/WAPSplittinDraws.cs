using EcShop.Core.Entities;
using EcShop.Entities.Members;
using EcShop.Membership.Context;
using EcShop.SaleSystem.Member;
using EcShop.UI.Common.Controls;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace EcShop.UI.SaleSystem.CodeBehind
{
	[System.Web.UI.ParseChildren(true)]
	public class WAPSplittinDraws : WAPMemberTemplatedWebControl
	{
		private System.Web.UI.WebControls.Literal lblBanlance;
		private System.Web.UI.WebControls.Literal lblLastDrawTime;
		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-SplittinDraws.html";
			}
			base.OnInit(e);
		}
		protected override void AttachChildControls()
		{
			PageTitle.AddSiteNameTitle("申请提现");
            Member member = HiContext.Current.User as Member;
			this.lblBanlance = (System.Web.UI.WebControls.Literal)this.FindControl("lblBanlance");
			this.lblLastDrawTime = (System.Web.UI.WebControls.Literal)this.FindControl("lblLastDrawTime");
			if (!this.Page.IsPostBack)
			{
				if (this.lblBanlance != null)
				{
                    this.lblBanlance.Text = member.Balance.ToString("F2");
				}
				if (this.lblLastDrawTime != null)
				{
					DbQueryResult mySplittinDraws = MemberProcessor.GetMySplittinDraws(new BalanceDrawRequestQuery
					{
						PageIndex = 1,
						PageSize = 1,
						UserId = new int?(HiContext.Current.User.UserId)
					}, new int?(1));
					if (mySplittinDraws.TotalRecords > 0)
					{
						DataTable dataTable = (DataTable)mySplittinDraws.Data;
						this.lblLastDrawTime.Text = dataTable.Rows[0]["RequestDate"].ToString();
						return;
					}
					this.lblLastDrawTime.Text = "您还没有提现记录";
				}
			}
		}
	}
}

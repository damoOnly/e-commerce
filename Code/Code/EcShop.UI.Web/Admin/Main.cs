using EcShop.ControlPanel.Sales;
using EcShop.ControlPanel.Store;
using EcShop.Core;
using EcShop.Entities.Sales;
using EcShop.Entities.Store;
using EcShop.Membership.Context;
using EcShop.UI.Common.Controls;
using EcShop.UI.ControlPanel.Utility;
using System;
using System.Web.UI.WebControls;
namespace EcShop.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.Summary)]
	public class Main : AdminPage
	{
		protected System.Web.UI.WebControls.HyperLink ltrWaitSendOrdersNumber;
		protected ClassShowOnDataLitl lblMemberBlancedrawRequest;
		protected System.Web.UI.WebControls.HyperLink hpkMessages;
		protected System.Web.UI.WebControls.HyperLink hpkLiuYan;
		protected System.Web.UI.WebControls.HyperLink hpkZiXun;
		protected ClassShowOnDataLitl lblTodayFinishOrder;
		protected ClassShowOnDataLitl lblYesterdayFinishOrder;
		protected ClassShowOnDataLitl lblTodayOrderAmout;
		protected ClassShowOnDataLitl lblOrderPriceYesterDay;
		protected ClassShowOnDataLitl ltrTodayAddMemberNumber;
		protected ClassShowOnDataLitl lblUserNewAddYesterToday;
		protected ClassShowOnDataLitl lblTotalMembers;
		protected ClassShowOnDataLitl lblTotalProducts;
		protected ClassShowOnDataLitl lblMembersBalanceTotal;
		protected ClassShowOnDataLitl lblOrderPriceMonth;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!base.IsPostBack)
			{
				AdminStatisticsInfo statistics = SalesHelper.GetStatistics();
				this.BindStatistics(statistics);
			}
		}
		private void BindStatistics(AdminStatisticsInfo statistics)
		{
			Users.GetUser(HiContext.Current.User.UserId);
			if (statistics.OrderNumbWaitConsignment > 0)
			{
				this.ltrWaitSendOrdersNumber.NavigateUrl = "javascript:ShowSecondMenuLeft('订 单','sales/manageorder.aspx','" + Globals.ApplicationPath + "/Admin/sales/ManageOrder.aspx?orderStatus=2')";
			}
			this.ltrWaitSendOrdersNumber.Text = ((statistics.OrderNumbWaitConsignment > 0) ? (statistics.OrderNumbWaitConsignment.ToString() + "条") : "<font style=\"color:#2d2d2d\">0条</font>");
			this.hpkLiuYan.Text = ((statistics.LeaveComments > 0) ? (statistics.LeaveComments.ToString() + "条") : "<font style=\"color:#2d2d2d\">0条</font>");
			this.hpkZiXun.Text = ((statistics.ProductConsultations > 0) ? (statistics.ProductConsultations.ToString() + "条") : "<font style=\"color:#2d2d2d\">0条</font>");
			this.hpkMessages.Text = ((statistics.Messages > 0) ? (statistics.Messages.ToString() + "条") : "<font style=\"color:#2d2d2d\">0条</font>");
			this.hpkLiuYan.NavigateUrl = "javascript:ShowSecondMenuLeft('CRM管理','comment/manageleavecomments.aspx','" + Globals.ApplicationPath + "/Admin/comment/ManageLeaveComments.aspx?MessageStatus=3')";
			this.hpkZiXun.NavigateUrl = "javascript:ShowSecondMenuLeft('CRM管理','comment/productconsultations.aspx',null)";
			this.hpkMessages.NavigateUrl = "javascript:ShowSecondMenuLeft('CRM管理','comment/receivedmessages.aspx','" + Globals.ApplicationPath + "/Admin/comment/ReceivedMessages.aspx?IsRead=0')";
			this.lblTodayOrderAmout.Text = ((statistics.OrderPriceToday > 0m) ? ("￥" + Globals.FormatMoney(statistics.OrderPriceToday)) : string.Empty);
			this.ltrTodayAddMemberNumber.Text = ((statistics.UserNewAddToday > 0) ? statistics.UserNewAddToday.ToString() : string.Empty);
			this.lblMembersBalanceTotal.Text = "￥" + Globals.FormatMoney(statistics.MembersBalance);
			this.lblMemberBlancedrawRequest.Text = ((statistics.MemberBlancedrawRequest > 0) ? ("<a href=\"javascript:ShowSecondMenuLeft('财务管理','member/balancedrawrequest.aspx','member/balancedrawrequest.aspx')\">" + statistics.MemberBlancedrawRequest.ToString() + "条</a>") : string.Empty);
			this.lblTodayFinishOrder.Text = ((statistics.TodayFinishOrder > 0) ? statistics.TodayFinishOrder.ToString() : string.Empty);
			this.lblYesterdayFinishOrder.Text = ((statistics.YesterdayFinishOrder > 0) ? statistics.YesterdayFinishOrder.ToString() : string.Empty);
			this.lblOrderPriceYesterDay.Text = ((statistics.OrderPriceYesterDay > 0m) ? ("￥" + statistics.OrderPriceYesterDay.ToString("F2")) : string.Empty);
			this.lblUserNewAddYesterToday.Text = ((statistics.UserNewAddYesterToday > 0) ? (statistics.UserNewAddYesterToday.ToString() + "位") : string.Empty);
			this.lblTotalMembers.Text = ((statistics.TotalMembers > 0) ? (statistics.TotalMembers.ToString() + "位") : string.Empty);
			this.lblTotalProducts.Text = ((statistics.TotalProducts > 0) ? (statistics.TotalProducts.ToString() + "条") : string.Empty);
			this.lblOrderPriceMonth.Text = ((statistics.OrderPriceMonth > 0m) ? ("￥" + statistics.OrderPriceMonth.ToString("F2")) : string.Empty);
		}
	}
}

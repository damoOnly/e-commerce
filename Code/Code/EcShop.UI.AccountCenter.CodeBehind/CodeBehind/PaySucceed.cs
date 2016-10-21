using EcShop.Core;
using EcShop.Entities.Orders;
using EcShop.SaleSystem.Member;
using System;
using System.Web.UI.WebControls;
namespace EcShop.UI.AccountCenter.CodeBehind
{
	public class PaySucceed : MemberTemplatedWebControl
	{
		private System.Web.UI.WebControls.HyperLink hlkDetails;
		private System.Web.UI.WebControls.Label lblPaystatus;
		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "User/Skin-PaySucceed.html";
			}
			base.OnInit(e);
		}
		protected override void AttachChildControls()
		{
			this.hlkDetails = (System.Web.UI.WebControls.HyperLink)this.FindControl("hlkDetails");
			this.lblPaystatus = (System.Web.UI.WebControls.Label)this.FindControl("lblPayStatus");
			if (!this.Page.IsPostBack)
			{
				if (string.IsNullOrEmpty(this.Page.Request.QueryString["orderId"]))
				{
					this.lblPaystatus.Text = "无效访问";
					this.hlkDetails.Visible = false;
					return;
				}
				OrderInfo orderInfo = TradeHelper.GetOrderInfo(this.Page.Request.QueryString["orderId"]);
				if (orderInfo == null || orderInfo.OrderStatus != OrderStatus.BuyerAlreadyPaid)
				{
					this.lblPaystatus.Text = "订单不存在或订单状态不是已付款";
					this.hlkDetails.Visible = false;
					return;
				}
				this.hlkDetails.NavigateUrl = Globals.ApplicationPath + "/user/UserOrders.aspx?orderStatus=" + 2;
			}
		}
	}
}

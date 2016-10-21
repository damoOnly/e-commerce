using EcShop.Entities.Orders;
using EcShop.Entities.Sales;
using EcShop.SaleSystem.Member;
using EcShop.UI.Common.Controls;
using System;
using System.Web.UI.WebControls;
namespace EcShop.UI.AccountCenter.CodeBehind
{
	public class Bank : MemberTemplatedWebControl
	{
		private System.Web.UI.WebControls.Label lblPaymentName;
		private System.Web.UI.WebControls.Label lblDescription;
		private string orderId;
		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "User/Skin-Bank.html";
			}
			base.OnInit(e);
		}
		protected override void AttachChildControls()
		{
			this.lblPaymentName = (System.Web.UI.WebControls.Label)this.FindControl("lblPaymentName");
			this.lblDescription = (System.Web.UI.WebControls.Label)this.FindControl("lblDescription");
			this.orderId = this.Page.Request.QueryString["orderId"];
			PageTitle.AddSiteNameTitle("订单线下支付");
			if (string.IsNullOrEmpty(this.orderId))
			{
				base.GotoResourceNotFound();
			}
			if (!this.Page.IsPostBack)
			{
				OrderInfo orderInfo = TradeHelper.GetOrderInfo(this.orderId);
				PaymentModeInfo paymentMode = TradeHelper.GetPaymentMode(orderInfo.PaymentTypeId);
				if (paymentMode != null)
				{
					this.lblPaymentName.Text = paymentMode.Name;
					this.lblDescription.Text = paymentMode.Description;
				}
			}
		}
	}
}

using EcShop.Core;
using EcShop.Entities.Orders;
using EcShop.Membership.Context;
using EcShop.SaleSystem.Member;
using EcShop.UI.Common.Controls;
using System;
using System.Data;
using System.Web.UI.WebControls;
namespace EcShop.UI.AccountCenter.CodeBehind
{
	public class UserRefundApplyDetails : MemberTemplatedWebControl
	{
		private int refundId;
		private string orderId;
		private System.Web.UI.WebControls.Literal txtOrderId;
		private System.Web.UI.WebControls.Literal handleStatus;
		private FormatedTimeLabel litAddDate;
		private Common_OrderManage_OrderItems RefundDetails;
		private System.Web.UI.WebControls.Literal litRemark;
		private System.Web.UI.WebControls.Literal litWeight;
		private System.Web.UI.WebControls.Literal litAdminRemark;
		private System.Web.UI.WebControls.Literal litType;
		private FormatedMoneyLabel litRefundTotal;
		private FormatedMoneyLabel litTotalPrice;
		private FormatedMoneyLabel litOrderTotal;
		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "User/Skin-UserRefundApplyDetails.html";
			}
			base.OnInit(e);
		}
		protected override void AttachChildControls()
		{
			this.refundId = System.Convert.ToInt32(this.Page.Request.QueryString["RefundId"]);
			this.RefundDetails = (Common_OrderManage_OrderItems)this.FindControl("Common_OrderManage_OrderItems");
			this.txtOrderId = (System.Web.UI.WebControls.Literal)this.FindControl("txtOrderId");
			this.handleStatus = (System.Web.UI.WebControls.Literal)this.FindControl("handleStatus");
			this.litAddDate = (FormatedTimeLabel)this.FindControl("litAddDate");
			this.litRemark = (System.Web.UI.WebControls.Literal)this.FindControl("litRemark");
			this.litAdminRemark = (System.Web.UI.WebControls.Literal)this.FindControl("litAdminRemark");
			this.litWeight = (System.Web.UI.WebControls.Literal)this.FindControl("litWeight");
			this.litType = (System.Web.UI.WebControls.Literal)this.FindControl("litType");
			this.litTotalPrice = (FormatedMoneyLabel)this.FindControl("litTotalPrice");
			this.litRefundTotal = (FormatedMoneyLabel)this.FindControl("litRefundTotal");
			this.litOrderTotal = (FormatedMoneyLabel)this.FindControl("litOrderTotal");
			if (!this.Page.IsPostBack)
			{
				this.BindOrderRefund(this.refundId);
				OrderInfo orderInfo = TradeHelper.GetOrderInfo(this.orderId);
				if (orderInfo == null || orderInfo.UserId != HiContext.Current.User.UserId)
				{
					this.Page.Response.Redirect(Globals.ApplicationPath + "/ResourceNotFound.aspx?errorMsg=" + Globals.UrlEncode("该订单不存在或者不属于当前用户的订单"));
					return;
				}
				this.BindOrderItems(orderInfo);
				this.BindRefunds(orderInfo);
			}
		}
		private void BindRefunds(OrderInfo order)
		{
			this.RefundDetails.DataSource = order.LineItems.Values;
			this.RefundDetails.DataBind();
		}
		private void BindOrderItems(OrderInfo order)
		{
			if (order.OrderStatus == OrderStatus.Refunded && this.handleStatus.Text == "已处理")
			{
				this.litRefundTotal.Money = order.GetTotal();
			}
			this.litTotalPrice.Money = order.GetTotal();
			this.litOrderTotal.Money = order.GetTotal();
			this.litWeight.Text = order.Weight.ToString("F2");
			this.txtOrderId.Text = order.OrderId;
		}
		private void BindOrderRefund(int refundId)
		{
			DataTable refundApplysTable = TradeHelper.GetRefundApplysTable(refundId);
			if (refundApplysTable.Rows[0]["HandleStatus"] != System.DBNull.Value)
			{
				if ((int)refundApplysTable.Rows[0]["HandleStatus"] == 0)
				{
					this.handleStatus.Text = "未处理";
				}
				else
				{
					if ((int)refundApplysTable.Rows[0]["HandleStatus"] == 1)
					{
						this.handleStatus.Text = "已处理";
					}
					else
					{
						this.handleStatus.Text = "已拒绝";
					}
				}
			}
			if (refundApplysTable.Rows[0]["ApplyForTime"] != System.DBNull.Value)
			{
				this.litAddDate.Time = (System.DateTime)refundApplysTable.Rows[0]["ApplyForTime"];
			}
			if (refundApplysTable.Rows[0]["RefundRemark"] != System.DBNull.Value)
			{
				this.litRemark.Text = refundApplysTable.Rows[0]["RefundRemark"].ToString();
			}
			if (refundApplysTable.Rows[0]["AdminRemark"] != System.DBNull.Value)
			{
				this.litAdminRemark.Text = refundApplysTable.Rows[0]["AdminRemark"].ToString();
			}
			if (refundApplysTable.Rows[0]["RefundType"] != System.DBNull.Value)
			{
				this.litType.Text = (refundApplysTable.Rows[0]["RefundType"].ToString().Equals("1") ? "预付款" : "银行转账");
			}
			if (refundApplysTable.Rows[0]["OrderId"] != System.DBNull.Value)
			{
				this.orderId = refundApplysTable.Rows[0]["OrderId"].ToString();
			}
		}
	}
}

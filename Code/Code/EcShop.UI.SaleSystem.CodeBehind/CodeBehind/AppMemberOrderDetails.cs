using EcShop.Entities.Orders;
using EcShop.Entities.Promotions;
using EcShop.SaleSystem.Shopping;
using EcShop.UI.Common.Controls;
using EcShop.UI.SaleSystem.CodeBehind.Common;
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace EcShop.UI.SaleSystem.CodeBehind
{
	[System.Web.UI.ParseChildren(true)]
	public class AppMemberOrderDetails : AppshopTemplatedWebControl
	{
		private string orderId;
		private System.Web.UI.WebControls.Literal litShipTo;
		private System.Web.UI.WebControls.Literal litPhone;
		private System.Web.UI.WebControls.Literal litAddress;
		private System.Web.UI.WebControls.Literal litShipToDate;
		private System.Web.UI.WebControls.Literal litShippingCost;
		private System.Web.UI.WebControls.Literal litCounponPrice;
		private System.Web.UI.WebControls.Literal litBuildPrice;
		private System.Web.UI.WebControls.Literal litDisCountPrice;
		private System.Web.UI.WebControls.Literal litTax;
		private System.Web.UI.WebControls.Literal litPayCharge;
		private System.Web.UI.WebControls.Literal litOrderId;
		private System.Web.UI.WebControls.Literal litActualPrice;
		private System.Web.UI.WebControls.Literal litOrderDate;
		private System.Web.UI.WebControls.Literal litRemark;
		private OrderStatusLabel litOrderStatus;
		private System.Web.UI.WebControls.Literal litTotalPrice;
		private System.Web.UI.WebControls.Literal litPayTime;
		private System.Web.UI.HtmlControls.HtmlInputHidden orderStatus;
		private System.Web.UI.HtmlControls.HtmlInputHidden txtOrderId;
		private AppshopTemplatedRepeater rptOrderProducts;
		private AppshopTemplatedRepeater rptPromotions;
		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-VMemberOrderDetails.html";
			}
			base.OnInit(e);
		}
		protected override void AttachChildControls()
		{
			this.orderId = this.Page.Request.QueryString["orderId"];
			this.litShipTo = (System.Web.UI.WebControls.Literal)this.FindControl("litShipTo");
			this.litPhone = (System.Web.UI.WebControls.Literal)this.FindControl("litPhone");
			this.litAddress = (System.Web.UI.WebControls.Literal)this.FindControl("litAddress");
			this.litOrderId = (System.Web.UI.WebControls.Literal)this.FindControl("litOrderId");
			this.litOrderDate = (System.Web.UI.WebControls.Literal)this.FindControl("litOrderDate");
			this.litOrderStatus = (OrderStatusLabel)this.FindControl("litOrderStatus");
			this.rptOrderProducts = (AppshopTemplatedRepeater)this.FindControl("rptOrderProducts");
			this.litTotalPrice = (System.Web.UI.WebControls.Literal)this.FindControl("litTotalPrice");
			this.litPayTime = (System.Web.UI.WebControls.Literal)this.FindControl("litPayTime");
			this.orderStatus = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("orderStatus");
			this.txtOrderId = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("txtOrderId");
			this.litRemark = (System.Web.UI.WebControls.Literal)this.FindControl("litRemark");
			this.litShipToDate = (System.Web.UI.WebControls.Literal)this.FindControl("litShipToDate");
			this.litShippingCost = (System.Web.UI.WebControls.Literal)this.FindControl("litShippingCost");
			this.litCounponPrice = (System.Web.UI.WebControls.Literal)this.FindControl("litCounponPrice");
			this.litBuildPrice = (System.Web.UI.WebControls.Literal)this.FindControl("litBuildPrice");
			this.litDisCountPrice = (System.Web.UI.WebControls.Literal)this.FindControl("litDisCountPrice");
			this.litActualPrice = (System.Web.UI.WebControls.Literal)this.FindControl("litActualPrice");
			this.rptPromotions = (AppshopTemplatedRepeater)this.FindControl("rptPromotions");
			this.litTax = (System.Web.UI.WebControls.Literal)this.FindControl("litTax");
			this.litPayCharge = (System.Web.UI.WebControls.Literal)this.FindControl("litPayCharge");
			OrderInfo orderInfo = ShoppingProcessor.GetOrderInfo(this.orderId);
			if (orderInfo == null)
			{
				base.GotoResourceNotFound("此订单已不存在");
			}
			this.litShipTo.Text = orderInfo.ShipTo;
			this.litPhone.Text = orderInfo.CellPhone;
			this.litAddress.Text = orderInfo.ShippingRegion + orderInfo.Address;
			this.litOrderId.Text = this.orderId;
			this.litOrderDate.Text = orderInfo.OrderDate.ToString();
			this.litTotalPrice.SetWhenIsNotNull(orderInfo.GetAmount().ToString("F2"));
			this.litOrderStatus.OrderStatusCode = orderInfo.OrderStatus;
			this.litPayTime.SetWhenIsNotNull((orderInfo.PayDate != System.DateTime.MinValue) ? orderInfo.PayDate.ToString("yyyy-MM-dd HH:mm:ss") : "");
			this.orderStatus.SetWhenIsNotNull(((int)orderInfo.OrderStatus).ToString());
			this.txtOrderId.SetWhenIsNotNull(this.orderId.ToString());
			this.litCounponPrice.SetWhenIsNotNull(orderInfo.CouponValue.ToString("F2"));
			this.litShippingCost.SetWhenIsNotNull(orderInfo.AdjustedFreight.ToString("F2"));
			this.litShipToDate.SetWhenIsNotNull(orderInfo.ShipToDate);
			this.litBuildPrice.SetWhenIsNotNull(orderInfo.GetAmount().ToString("F2"));
			this.litDisCountPrice.SetWhenIsNotNull(orderInfo.AdjustedDiscount.ToString("F2"));
			this.litActualPrice.SetWhenIsNotNull(orderInfo.GetTotal().ToString("F2"));
			this.litRemark.SetWhenIsNotNull(orderInfo.Remark);
			this.litTax.SetWhenIsNotNull(orderInfo.Tax.ToString("F2"));
			this.litPayCharge.SetWhenIsNotNull(orderInfo.PayCharge.ToString("F2"));
			this.rptOrderProducts.DataSource = orderInfo.LineItems.Values;
			this.rptOrderProducts.DataBind();
			System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<string, string>> list = new System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<string, string>>();
			if (orderInfo.IsReduced)
			{
				list.Add(new System.Collections.Generic.KeyValuePair<string, string>(PromotionHelper.GetShortName(PromoteType.Reduced), orderInfo.ReducedPromotionName + string.Format(" 优惠：{0}", orderInfo.ReducedPromotionAmount.ToString("F2"))));
			}
			if (orderInfo.IsFreightFree)
			{
				list.Add(new System.Collections.Generic.KeyValuePair<string, string>(PromotionHelper.GetShortName(PromoteType.FullAmountSentFreight), string.Format("{0}", orderInfo.FreightFreePromotionName)));
			}
			if (orderInfo.IsSendTimesPoint)
			{
				list.Add(new System.Collections.Generic.KeyValuePair<string, string>(PromotionHelper.GetShortName(PromoteType.FullAmountSentTimesPoint), string.Format("{0}：送{1}倍", orderInfo.SentTimesPointPromotionName, orderInfo.TimesPoint.ToString("F2"))));
			}
			this.rptPromotions.DataSource = list;
			this.rptPromotions.DataBind();
			PageTitle.AddSiteNameTitle("订单详情");
		}
	}
}

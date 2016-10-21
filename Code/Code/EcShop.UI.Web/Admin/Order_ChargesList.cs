using EcShop.Core;
using EcShop.Entities.Orders;
using System;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace EcShop.UI.Web.Admin
{
	public class Order_ChargesList : System.Web.UI.UserControl
	{
		private OrderInfo order;
		protected System.Web.UI.WebControls.Literal litFreight;
		protected System.Web.UI.WebControls.Literal litShippingMode;
		protected System.Web.UI.WebControls.HyperLink hlkFreightFreePromotion;
		protected System.Web.UI.WebControls.Label lkBtnEditshipingMode;
		protected System.Web.UI.WebControls.Literal litPayCharge;
		protected System.Web.UI.WebControls.Literal litPayMode;
		protected System.Web.UI.WebControls.LinkButton lkBtnEditPayMode;
		protected System.Web.UI.WebControls.Literal litCouponValue;
		protected System.Web.UI.WebControls.Literal litCoupon;
        protected System.Web.UI.WebControls.Literal litVoucherValue;
        protected System.Web.UI.WebControls.Literal litVoucher;
		protected System.Web.UI.WebControls.Literal litDiscount;
		protected System.Web.UI.WebControls.Literal litTax;
		protected System.Web.UI.WebControls.Literal litInvoiceTitle;
		protected System.Web.UI.WebControls.Literal litPoints;
		protected System.Web.UI.WebControls.HyperLink hlkSentTimesPointPromotion;
		protected System.Web.UI.WebControls.Literal litTotalPrice;
        protected System.Web.UI.WebControls.Literal litActivityValue;
        protected System.Web.UI.WebControls.Literal litActivity;

		public OrderInfo Order
		{
			get
			{
				return this.order;
			}
			set
			{
				this.order = value;
			}
		}
		protected override void OnLoad(System.EventArgs e)
		{
			this.LoadControls();
		}
		public void LoadControls()
		{
			if (this.order.OrderStatus == OrderStatus.WaitBuyerPay || this.order.OrderStatus == OrderStatus.BuyerAlreadyPaid)
			{
				this.lkBtnEditshipingMode.Visible = true;
			}
			if (this.order.OrderStatus == OrderStatus.WaitBuyerPay)
			{
				this.lkBtnEditPayMode.Visible = true;
			}
			this.litFreight.Text = Globals.FormatMoney(this.order.AdjustedFreight);
			if (this.order.OrderStatus == OrderStatus.Finished || this.order.OrderStatus == OrderStatus.SellerAlreadySent)
			{
				this.litShippingMode.Text = this.order.RealModeName;
			}
			else
			{
				this.litShippingMode.Text = this.order.ModeName;
			}
			this.litPayMode.Text = this.order.PaymentType;
			if (this.order.IsFreightFree)
			{
				this.hlkFreightFreePromotion.Text = this.order.FreightFreePromotionName;
				this.hlkFreightFreePromotion.NavigateUrl = Globals.GetSiteUrls().UrlData.FormatUrl("FavourableDetails", new object[]
				{
					this.order.FreightFreePromotionId
				});
			}
			this.litPayCharge.Text = Globals.FormatMoney(this.order.PayCharge);
			if (!string.IsNullOrEmpty(this.order.CouponName))
			{
				this.litCoupon.Text = "[" + this.order.CouponName + "]-" + Globals.FormatMoney(this.order.CouponValue);
			}
			else
			{
				this.litCoupon.Text = "-" + Globals.FormatMoney(this.order.CouponValue);
			}
			this.litCouponValue.Text = "-" + Globals.FormatMoney(this.order.CouponValue);

            if (!string.IsNullOrEmpty(this.order.VoucherName))
            {
                this.litVoucher.Text = "[" + this.order.VoucherName + "]-" + Globals.FormatMoney(this.order.VoucherValue);
            }
            else
            {
                this.litVoucher.Text = "-" + Globals.FormatMoney(this.order.VoucherValue);
            }
            this.litVoucherValue.Text = "-" + Globals.FormatMoney(this.order.VoucherValue);
            //this.litActivityValue.Text = "-" + order.GetActivityPrice().ToString("F2");
            //this.litActivity.Text = "-" + order.GetActivityPrice().ToString("F2"); 
			this.litDiscount.Text = Globals.FormatMoney(this.order.AdjustedDiscount);
			this.litPoints.Text = this.order.Points.ToString(System.Globalization.CultureInfo.InvariantCulture);
			if (this.order.IsSendTimesPoint)
			{
				this.hlkSentTimesPointPromotion.Text = this.order.SentTimesPointPromotionName;
				this.hlkSentTimesPointPromotion.NavigateUrl = Globals.GetSiteUrls().UrlData.FormatUrl("FavourableDetails", new object[]
				{
					this.order.SentTimesPointPromotionId
				});
			}
			this.litTotalPrice.Text = Globals.FormatMoney(this.order.GetTotal());
			if (this.order.Tax > 0m)
			{
				this.litTax.Text = "<tr class=\"bg\"><td align=\"right\">税金(元)：</td><td colspan=\"2\"><span class='Name'>" + Globals.FormatMoney(this.order.Tax);
				System.Web.UI.WebControls.Literal expr_2C1 = this.litTax;
				expr_2C1.Text += "</span></td></tr>";
			}
			if (this.order.InvoiceTitle.Length > 0)
			{
				this.litInvoiceTitle.Text = "<tr class=\"bg\"><td align=\"right\">发票抬头：</td><td colspan=\"2\"><span class='Name'>" + this.order.InvoiceTitle;
				System.Web.UI.WebControls.Literal expr_30F = this.litInvoiceTitle;
				expr_30F.Text += "</span></td></tr>";
			}
		}
	}
}

using EcShop.Core;
using EcShop.Entities.Orders;
using EcShop.Entities.Sales;
using EcShop.Membership.Context;
using EcShop.SaleSystem.Shopping;
using EcShop.UI.Common.Controls;
using Ecdev.Plugins;
using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using EcShop.SaleSystem.Member;
namespace EcShop.UI.SaleSystem.CodeBehind
{
	[System.Web.UI.ParseChildren(true)]
	public class VFinishOrder : VMemberTemplatedWebControl
	{
		private string orderId;
		private System.Web.UI.WebControls.Literal litOrderId;
		private System.Web.UI.WebControls.Literal litOrderTotal;
		private System.Web.UI.HtmlControls.HtmlInputHidden litPaymentType;
		private System.Web.UI.WebControls.Literal litPaymentName;
		private System.Web.UI.WebControls.Literal litHelperText;
		private System.Web.UI.HtmlControls.HtmlGenericControl divhelper;
		private System.Web.UI.HtmlControls.HtmlAnchor btnToPay;
		private OrderInfo order;
		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-VFinishOrder.html";
			}
			base.OnInit(e);
		}
		protected override void AttachChildControls()
		{

			this.orderId = this.Page.Request.QueryString["orderId"];
			this.order = ShoppingProcessor.GetOrderInfo(this.orderId);
			if (this.order == null)
			{
				base.GotoResourceNotFound("");
			}
            if (!this.Page.IsPostBack)
            {
                //string text = System.Web.HttpContext.Current.Request.QueryString["action"];
                //if (!string.IsNullOrEmpty(text) && text == "topay")
                //{
                //    this.GotoPay();
                //}
                this.GotoPay();
            }
			this.litOrderId = (System.Web.UI.WebControls.Literal)this.FindControl("litOrderId");
			this.litOrderTotal = (System.Web.UI.WebControls.Literal)this.FindControl("litOrderTotal");
			this.litPaymentType = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("litPaymentType");
			this.litPaymentName = (System.Web.UI.WebControls.Literal)this.FindControl("litPaymentName");
			this.litPaymentType.SetWhenIsNotNull(this.order.PaymentTypeId.ToString());
			this.litOrderId.SetWhenIsNotNull(this.orderId);
			this.litOrderTotal.SetWhenIsNotNull(this.order.GetTotal().ToString("F2"));
            //if (TradeHelper.CheckIsUnpack(order.OrderId))
            //{
            //    decimal childOrdertotal = ShoppingProcessor.GetChildOrderTotal(order.OrderId);
            //    this.litOrderTotal.Text = childOrdertotal.ToString("F2");
            //}
			this.litPaymentName.Text = this.order.PaymentType;
			this.divhelper = (System.Web.UI.HtmlControls.HtmlGenericControl)this.FindControl("helper");
			if (this.order.Gateway != "ecdev.plugins.payment.bankrequest")
			{
				this.divhelper.Visible = false;
			}
			else
			{
				this.divhelper.Visible = true;
				this.litHelperText = (System.Web.UI.WebControls.Literal)this.FindControl("litHelperText");
				SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
				this.litHelperText.SetWhenIsNotNull(masterSettings.OffLinePayContent);
			}
			this.btnToPay = (System.Web.UI.HtmlControls.HtmlAnchor)this.FindControl("btnToPay");
			if (this.btnToPay != null)
			{
				this.btnToPay.HRef = "FinishOrder.aspx?orderId=" + this.orderId + "&action=topay";
			}
			else
			{
				this.GotoPay();
			}
			if (this.btnToPay != null && (this.order.Gateway == "ecdev.plugins.payment.podrequest" || this.order.Gateway == "ecdev.plugins.payment.bankrequest"))
			{
				this.btnToPay.Visible = false;
			}
			PageTitle.AddSiteNameTitle("下单成功");

		}
		private void GotoPay()
		{
			if (this.order.Gateway == "Ecdev.plugins.payment.advancerequest")
			{
				this.Page.Response.Redirect("TransactionPwd.aspx?orderId=" + this.Page.Request.QueryString["orderId"] + "&totalAmount=" + this.order.GetTotal().ToString("F2"));
			}
			if (!string.IsNullOrEmpty(this.order.Gateway) && this.order.Gateway == "Ecdev.plugins.payment.weixinrequest")
			{
				System.Web.HttpContext.Current.Response.Redirect("~/pay/wx_Submit.aspx?orderId=" + this.order.OrderId);
			}
			if (this.order.Gateway == "Ecdev.plugins.payment.ws_wappay.wswappayrequest")
			{
				PaymentModeInfo paymentMode = ShoppingProcessor.GetPaymentMode(this.order.Gateway);
				string attach = "";
				string showUrl = string.Format("http://{0}/vshop/", System.Web.HttpContext.Current.Request.Url.Host);
				PaymentRequest paymentRequest = PaymentRequest.CreateInstance(paymentMode.Gateway, HiCryptographer.Decrypt(paymentMode.Settings), this.order.OrderId, this.order.GetTotal(), "订单支付", "订单号-" + this.order.OrderId, this.order.EmailAddress, this.order.OrderDate, showUrl, Globals.FullPath("/pay/wap_alipay_return_url.aspx"), Globals.FullPath("/pay/wap_alipay_return_url.aspx"), attach);
				paymentRequest.SendRequest();
				return;
			}
			if (this.order.Gateway == "Ecdev.Plugins.Payment.ShengPayMobile.ShengPayMobileRequest")
			{
				PaymentModeInfo paymentMode2 = ShoppingProcessor.GetPaymentMode(this.order.Gateway);
				string attach2 = "";
				string text = string.Format("http://{0}/vshop/", System.Web.HttpContext.Current.Request.Url.Host);
				PaymentRequest paymentRequest2 = PaymentRequest.CreateInstance(paymentMode2.Gateway, HiCryptographer.Decrypt(paymentMode2.Settings), this.order.OrderId, this.order.GetTotal(), "订单支付", "订单号-" + this.order.OrderId, this.order.EmailAddress, this.order.OrderDate, text, text, Globals.FullPath("/pay/wap_sheng_return_url.aspx"), attach2);
				paymentRequest2.SendRequest();
			}
		}
	}
}

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
namespace EcShop.UI.SaleSystem.CodeBehind
{
	[System.Web.UI.ParseChildren(true)]
	public class AppFinishOrder : AppshopTemplatedWebControl
	{
		private string orderId;
		private System.Web.UI.WebControls.Literal litOrderId;
		private System.Web.UI.WebControls.Literal litOrderTotal;
		private System.Web.UI.HtmlControls.HtmlInputHidden litPaymentType;
		private System.Web.UI.WebControls.Literal litHelperText;
		private System.Web.UI.HtmlControls.HtmlGenericControl divhelper;
		private System.Web.UI.HtmlControls.HtmlAnchor btnToPay;
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
			OrderInfo orderInfo = ShoppingProcessor.GetOrderInfo(this.orderId);
			if (orderInfo == null)
			{
				base.GotoResourceNotFound("");
			}
			this.litOrderId = (System.Web.UI.WebControls.Literal)this.FindControl("litOrderId");
			this.litOrderTotal = (System.Web.UI.WebControls.Literal)this.FindControl("litOrderTotal");
			this.litPaymentType = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("litPaymentType");
			this.litPaymentType.SetWhenIsNotNull(orderInfo.PaymentTypeId.ToString());
			this.litOrderId.SetWhenIsNotNull(this.orderId);
			this.litOrderTotal.SetWhenIsNotNull(orderInfo.GetTotal().ToString("F2"));
			this.divhelper = (System.Web.UI.HtmlControls.HtmlGenericControl)this.FindControl("helper");
			if (orderInfo.Gateway != "ecdev.plugins.payment.bankrequest")
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
			if (this.btnToPay != null && (orderInfo.Gateway == "ecdev.plugins.payment.podrequest" || orderInfo.Gateway == "ecdev.plugins.payment.bankrequest"))
			{
				this.btnToPay.Visible = false;
			}
			PageTitle.AddSiteNameTitle("下单成功");
			if (!this.Page.IsPostBack)
			{
				string text = System.Web.HttpContext.Current.Request.QueryString["action"];
				if (!string.IsNullOrEmpty(text) && text == "topay")
				{
					this.GotoPay();
				}
			}
		}
		private void GotoPay()
		{
			OrderInfo orderInfo = ShoppingProcessor.GetOrderInfo(this.orderId);
			if (orderInfo.Gateway == "ecdev.plugins.payment.advancerequest")
			{
				this.Page.Response.Redirect("TransactionPwd.aspx?orderId=" + this.Page.Request.QueryString["orderId"] + "&totalAmount=" + orderInfo.GetTotal().ToString("F2"));
			}
			if (orderInfo.Gateway == "ecdev.plugins.payment.ws_apppay.wswappayrequest")
			{
				System.Web.HttpContext.Current.Response.Redirect("~/pay/app_alipay_Submit.aspx?orderId=" + orderInfo.OrderId);
			}
			if (orderInfo.Gateway == "ecdev.plugins.payment.ws_wappay.wswappayrequest")
			{
				PaymentModeInfo paymentMode = ShoppingProcessor.GetPaymentMode(orderInfo.Gateway);
				string attach = "";
				string showUrl = string.Format("http://{0}/AppShop/MemberOrders.aspx", System.Web.HttpContext.Current.Request.Url.Host);
				PaymentRequest paymentRequest = PaymentRequest.CreateInstance(paymentMode.Gateway, HiCryptographer.Decrypt(paymentMode.Settings), orderInfo.OrderId, orderInfo.GetTotal(), "订单支付", "订单号-" + orderInfo.OrderId, orderInfo.EmailAddress, orderInfo.OrderDate, showUrl, Globals.FullPath("/pay/wap_alipay_return_url.aspx"), Globals.FullPath("/pay/wap_alipay_return_url.aspx"), attach);
				paymentRequest.SendRequest();
				return;
			}
			if (orderInfo.Gateway == "Ecdev.Plugins.Payment.ShengPayMobile.ShengPayMobileRequest")
			{
				PaymentModeInfo paymentMode2 = ShoppingProcessor.GetPaymentMode(orderInfo.Gateway);
				string attach2 = "";
				string text = string.Format("http://{0}/AppShop/", System.Web.HttpContext.Current.Request.Url.Host);
				PaymentRequest paymentRequest2 = PaymentRequest.CreateInstance(paymentMode2.Gateway, HiCryptographer.Decrypt(paymentMode2.Settings), orderInfo.OrderId, orderInfo.GetTotal(), "订单支付", "订单号-" + orderInfo.OrderId, orderInfo.EmailAddress, orderInfo.OrderDate, text, text, Globals.FullPath("/pay/wap_sheng_return_url.aspx"), attach2);
				paymentRequest2.SendRequest();
			}
		}
	}
}

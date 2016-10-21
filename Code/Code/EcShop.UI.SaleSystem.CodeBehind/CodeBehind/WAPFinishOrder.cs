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
using Ecdev.Weixin.Pay.Pay;
using Ecdev.Weixin.Pay.Domain;
namespace EcShop.UI.SaleSystem.CodeBehind
{
	[System.Web.UI.ParseChildren(true)]
	public class WAPFinishOrder : WAPMemberTemplatedWebControl
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
                //判断为微信手机网页支付
                if (orderInfo.Gateway == "Ecdev.plugins.payment.weixinwaprequest")
                {
                    SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
                    VCodePayResponsEntity rpsEntity = VCodePayHelper.CreateWeixinPay(new Ecdev.Weixin.Pay.Domain.VCodePayEntity()
                    {
                        appid = masterSettings.WeixinAppId,
                        //appid = "wx45cd46b5f561deee",
                        //mch_id = "D4DE77CE6F6C9E3A",
                        mch_id = masterSettings.WeixinPartnerID,
                        body = orderInfo.OrderId,
                        nonce_str = VCodePayHelper.CreateRandom(20),
                        out_trade_no = orderInfo.OrderId,
                        fee_type = "CNY",
                        //1 = 1分
                        total_fee = (int)(orderInfo.GetTotal() * 100),
                        spbill_create_ip = "192.168.1.40",
                        notify_url = "http://" + base.Context.Request.Url.Host + "/pay/wx_Pay_notify_url.aspx",
                        product_id = orderInfo.OrderId.ToString()
                    });
                    string javascript = string.Format("weixin://wap/pay?appid={0}&noncestr={1}&package=WAP&prepayid={2}&timestamp={3}&sign={4}",
                       rpsEntity.appid, rpsEntity.Nonce_str, rpsEntity.prepay_id, ConvertDateTimeInt(DateTime.Now), rpsEntity.sign);
                    this.btnToPay.Target = "_blank";
                    this.btnToPay.HRef = javascript;
                }
                else
                {
                    this.btnToPay.HRef = "FinishOrder.aspx?orderId=" + this.orderId + "&action=topay";
                }
			}
			else
			{
				this.GotoPay();
			}
			if (this.btnToPay != null && (orderInfo.Gateway == "ecdev.plugins.payment.podrequest" || orderInfo.Gateway == "ecdev.plugins.payment.bankrequest"))
			{
				this.btnToPay.Visible = false;
			}
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
			if (orderInfo.Gateway == "Ecdev.plugins.payment.advancerequest")
			{
				this.Page.Response.Redirect("TransactionPwd.aspx?orderId=" + this.Page.Request.QueryString["orderId"] + "&totalAmount=" + orderInfo.GetTotal().ToString("F2"));
			}
			if (orderInfo.Gateway == "Ecdev.plugins.payment.ws_wappay.wswappayrequest")
			{
				PaymentModeInfo paymentMode = ShoppingProcessor.GetPaymentMode(orderInfo.Gateway);
				string attach = "";
				string showUrl = string.Format("http://{0}/Wapshop/", System.Web.HttpContext.Current.Request.Url.Host);
				PaymentRequest paymentRequest = PaymentRequest.CreateInstance(paymentMode.Gateway, HiCryptographer.Decrypt(paymentMode.Settings), orderInfo.OrderId, orderInfo.GetTotal(), "订单支付", "订单号-" + orderInfo.OrderId, orderInfo.EmailAddress, orderInfo.OrderDate, showUrl, Globals.FullPath("/pay/wap_alipay_return_url.aspx"), Globals.FullPath("/pay/wap_alipay_return_url.aspx"), attach);
				paymentRequest.SendRequest();
				return;
			}
			if (orderInfo.Gateway == "Ecdev.Plugins.Payment.ShengPayMobile.ShengPayMobileRequest")
			{
				PaymentModeInfo paymentMode2 = ShoppingProcessor.GetPaymentMode(orderInfo.Gateway);
				string attach2 = "";
				string text = string.Format("http://{0}/Wapshop/", System.Web.HttpContext.Current.Request.Url.Host);
				HiCryptographer.Decrypt(paymentMode2.Settings);
				PaymentRequest paymentRequest2 = PaymentRequest.CreateInstance(paymentMode2.Gateway, HiCryptographer.Decrypt(paymentMode2.Settings), orderInfo.OrderId, orderInfo.GetTotal(), "订单支付", "订单号-" + orderInfo.OrderId, orderInfo.EmailAddress, orderInfo.OrderDate, text, text, Globals.FullPath("/pay/wap_sheng_return_url.aspx"), attach2);
				paymentRequest2.SendRequest();
			}
           
		}
        /// <summary>  
        /// DateTime时间格式转换为Unix时间戳格式  
        /// </summary>  
        /// <param name="time"> DateTime时间格式</param>  
        /// <returns>Unix时间戳格式</returns>  
        private static int ConvertDateTimeInt(System.DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            return (int)(time - startTime).TotalSeconds;
        }  
	}
}

using EcShop.ControlPanel.Sales;
using EcShop.Core;
using EcShop.Entities.Orders;
using EcShop.Entities.Sales;
using System;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Xml;
namespace EcShop.UI.Web.pay
{
	public class app_alipay_Submit : System.Web.UI.Page
	{
		public string pay_json = string.Empty;
		protected System.Web.UI.HtmlControls.HtmlHead Head1;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			string text = base.Request.QueryString.Get("orderId");
			if (string.IsNullOrEmpty(text))
			{
				return;
			}
			OrderInfo orderInfo = OrderHelper.GetOrderInfo(text);
			if (orderInfo == null)
			{
				return;
			}
			PaymentModeInfo paymentMode = SalesHelper.GetPaymentMode("Ecdev.plugins.payment.ws_apppay.wswappayrequest");
			string xml = HiCryptographer.Decrypt(paymentMode.Settings);
			System.Xml.XmlDocument xmlDocument = new System.Xml.XmlDocument();
			xmlDocument.LoadXml(xml);
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			stringBuilder.Append("partner=\"");
			stringBuilder.Append(xmlDocument.GetElementsByTagName("Partner")[0].InnerText);
			stringBuilder.Append("\"&out_trade_no=\"");
			stringBuilder.Append(orderInfo.OrderId);
			stringBuilder.Append("\"&subject=\"");
			stringBuilder.Append("订单支付");
			stringBuilder.Append("\"&body=\"");
			stringBuilder.Append("订单号-").Append(orderInfo.OrderId);
			stringBuilder.Append("\"&total_fee=\"");
			stringBuilder.Append(orderInfo.GetTotal().ToString("F2"));
			stringBuilder.Append("\"&notify_url=\"");
			stringBuilder.Append(Globals.UrlEncode(Globals.FullPath("/pay/app_alipay_notify_url.aspx")));
			stringBuilder.Append("\"&service=\"mobile.securitypay.pay");
			stringBuilder.Append("\"&_input_charset=\"UTF-8");
			stringBuilder.Append("\"&return_url=\"");
			stringBuilder.Append(Globals.UrlEncode("http://m.alipay.com"));
			stringBuilder.Append("\"&payment_type=\"1");
			stringBuilder.Append("\"&seller_id=\"");
			stringBuilder.Append(xmlDocument.GetElementsByTagName("Seller_account_name")[0].InnerText);
			stringBuilder.Append("\"&it_b_pay=\"1m\"");
			string str = Globals.UrlEncode(RSAFromPkcs8.sign(stringBuilder.ToString(), xmlDocument.GetElementsByTagName("Key")[0].InnerText, "utf-8"));
			this.pay_json = stringBuilder.ToString() + "&sign=\"" + str + "\"&sign_type=\"RSA\"";
		}
	}
}

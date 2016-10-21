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
using System.Xml;
namespace EcShop.UI.Web.Admin.AliOH
{
	[PrivilegeCheck(Privilege.AliohMobileAlipaySet)]
	public class AliOHAliPaySet : AdminPage
	{
		protected YesNoRadioButtonList radEnableWapAliPay;
		protected System.Web.UI.WebControls.TextBox txtPartner;
		protected System.Web.UI.WebControls.TextBox txtKey;
		protected System.Web.UI.WebControls.TextBox txtAccount;
		protected System.Web.UI.WebControls.Button btnAdd;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!this.Page.IsPostBack)
			{
				SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
				this.radEnableWapAliPay.SelectedValue = masterSettings.EnableAliOHAliPay;
				PaymentModeInfo paymentMode = SalesHelper.GetPaymentMode("Ecdev.plugins.payment.ws_wappay.wswappayrequest");
				if (paymentMode != null)
				{
					string xml = HiCryptographer.Decrypt(paymentMode.Settings);
					System.Xml.XmlDocument xmlDocument = new System.Xml.XmlDocument();
					xmlDocument.LoadXml(xml);
					this.txtPartner.Text = xmlDocument.GetElementsByTagName("Partner")[0].InnerText;
					this.txtKey.Text = xmlDocument.GetElementsByTagName("Key")[0].InnerText;
					this.txtAccount.Text = xmlDocument.GetElementsByTagName("Seller_account_name")[0].InnerText;
				}
				PaymentModeInfo paymentMode2 = SalesHelper.GetPaymentMode("Ecdev.plugins.payment.ws_apppay.wswappayrequest");
				if (paymentMode2 != null)
				{
					string xml2 = HiCryptographer.Decrypt(paymentMode2.Settings);
					System.Xml.XmlDocument xmlDocument2 = new System.Xml.XmlDocument();
					xmlDocument2.LoadXml(xml2);
				}
			}
		}
		protected void btnOK_Click(object sender, System.EventArgs e)
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
			masterSettings.EnableAliOHAliPay = this.radEnableWapAliPay.SelectedValue;
			SettingsManager.Save(masterSettings);
			string text = string.Format("<xml><Partner>{0}</Partner><Key>{1}</Key><Seller_account_name>{2}</Seller_account_name></xml>", this.txtPartner.Text, this.txtKey.Text, this.txtAccount.Text);
			PaymentModeInfo paymentMode = SalesHelper.GetPaymentMode("Ecdev.plugins.payment.ws_wappay.wswappayrequest");
			if (paymentMode == null)
			{
				PaymentModeInfo paymentMode2 = new PaymentModeInfo
				{
					Name = "支付宝手机网页支付",
					Gateway = "Ecdev.plugins.payment.ws_wappay.wswappayrequest",
					Description = string.Empty,
					IsUseInpour = false,
					Charge = 0m,
					IsPercent = false,
					ApplicationType = PayApplicationType.payOnWAP,
					Settings = HiCryptographer.Encrypt(text)
				};
				SalesHelper.CreatePaymentMode(paymentMode2);
			}
			else
			{
				PaymentModeInfo paymentModeInfo = paymentMode;
				paymentModeInfo.Settings = HiCryptographer.Encrypt(text);
				paymentModeInfo.ApplicationType = PayApplicationType.payOnWAP;
				SalesHelper.UpdatePaymentMode(paymentModeInfo);
			}
			this.ShowMsg("修改成功", true);
		}
	}
}

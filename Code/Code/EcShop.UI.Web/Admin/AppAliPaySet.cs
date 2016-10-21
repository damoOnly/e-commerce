using EcShop.ControlPanel.Sales;
using EcShop.ControlPanel.Store;
using EcShop.Core;
using EcShop.Entities.Sales;
using EcShop.Entities.Store;
using EcShop.Membership.Context;
using EcShop.UI.Common.Controls;
using EcShop.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Xml;
namespace EcShop.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.AppMobileAlipaySet)]
	public class AppAliPaySet : AdminPage
	{
		protected YesNoRadioButtonList radEnableAppAliPay;
		protected System.Web.UI.WebControls.TextBox txtAppPartner;
		protected System.Web.UI.WebControls.TextBox txtAppKey;
		protected System.Web.UI.WebControls.TextBox txtAppAccount;
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
				this.radEnableAppAliPay.SelectedValue = masterSettings.EnableAppAliPay;
				this.radEnableWapAliPay.SelectedValue = masterSettings.EnableAppWapAliPay;

				PaymentModeInfo paymentModeApp = SalesHelper.GetPaymentMode("Ecdev.plugins.payment.ws_apppay.wswappayrequest");
				if (paymentModeApp != null)
				{
					string xmlApp = HiCryptographer.Decrypt(paymentModeApp.Settings);
					System.Xml.XmlDocument docApp = new System.Xml.XmlDocument();
					docApp.LoadXml(xmlApp);
                    this.txtAppPartner.Text = docApp.GetElementsByTagName("Partner")[0].InnerText;
                    this.txtAppKey.Text = docApp.GetElementsByTagName("Key")[0].InnerText;
                    this.txtAppAccount.Text = docApp.GetElementsByTagName("Seller_account_name")[0].InnerText;
				}

                PaymentModeInfo paymentModeWap = SalesHelper.GetPaymentMode("Ecdev.plugins.payment.ws_wappay.wswappayrequest");
                if (paymentModeWap != null)
                {
                    string xmlWap = HiCryptographer.Decrypt(paymentModeWap.Settings);
                    System.Xml.XmlDocument docWap = new System.Xml.XmlDocument();
                    docWap.LoadXml(xmlWap);
                    this.txtPartner.Text = docWap.GetElementsByTagName("Partner")[0].InnerText;
                    this.txtKey.Text = docWap.GetElementsByTagName("Key")[0].InnerText;
                    this.txtAccount.Text = docWap.GetElementsByTagName("Seller_account_name")[0].InnerText;
                }
			}
		}
		protected void btnOK_Click(object sender, System.EventArgs e)
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
			masterSettings.EnableAppAliPay = this.radEnableAppAliPay.SelectedValue;
			masterSettings.EnableAppWapAliPay = this.radEnableWapAliPay.SelectedValue;
			SettingsManager.Save(masterSettings);

			string wapSettings = string.Format("<xml><Partner>{0}</Partner><Key>{1}</Key><Seller_account_name>{2}</Seller_account_name></xml>", this.txtPartner.Text, this.txtKey.Text, this.txtAccount.Text);
            PaymentModeInfo paymentModeWap = SalesHelper.GetPaymentMode("Ecdev.plugins.payment.ws_wappay.wswappayrequest");
			if (paymentModeWap == null)
			{
                paymentModeWap = new PaymentModeInfo
				{
                    Name = "支付宝手机网页支付",
                    Gateway = "Ecdev.plugins.payment.ws_wappay.wswappayrequest",
					Description = string.Empty,
					IsUseInpour = false,
					Charge = 0m,
					IsPercent = false,
					ApplicationType = PayApplicationType.payOnWAP,
					Settings = HiCryptographer.Encrypt(wapSettings)
				};
                SalesHelper.CreatePaymentMode(paymentModeWap);
			}
			else
			{
				PaymentModeInfo paymentModeInfo = paymentModeWap;
				paymentModeInfo.Settings = HiCryptographer.Encrypt(wapSettings);
				paymentModeInfo.ApplicationType = PayApplicationType.payOnWAP;
				SalesHelper.UpdatePaymentMode(paymentModeInfo);
			}

			string appSettings = string.Format("<xml><Partner>{0}</Partner><Key>{1}</Key><Seller_account_name>{2}</Seller_account_name></xml>", this.txtAppPartner.Text, this.txtAppKey.Text, this.txtAppAccount.Text);
			System.Collections.Generic.IList<PaymentModeInfo> paymentModes = SalesHelper.GetPaymentModes(PayApplicationType.payOnApp);
			if (paymentModes == null || paymentModes.Count == 0)
			{
				PaymentModeInfo paymentMode3 = new PaymentModeInfo
				{
					Name = "支付宝手机应用内支付",
					Gateway = "Ecdev.plugins.payment.ws_apppay.wswappayrequest",
					Description = string.Empty,
					IsUseInpour = false,
					Charge = 0m,
					IsPercent = false,
					ApplicationType = PayApplicationType.payOnApp,
					Settings = HiCryptographer.Encrypt(appSettings)
				};
				SalesHelper.CreatePaymentMode(paymentMode3);
			}
			else
			{
				PaymentModeInfo paymentModeApp = paymentModes[0];
				paymentModeApp.Settings = HiCryptographer.Encrypt(appSettings);
				paymentModeApp.ApplicationType = PayApplicationType.payOnApp;
				SalesHelper.UpdatePaymentMode(paymentModeApp);
			}
			this.ShowMsg("修改成功", true);
		}
	}
}

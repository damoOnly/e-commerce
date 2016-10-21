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
	[PrivilegeCheck(Privilege.AliohShengPaySet)]
	public class ShengPaySet : AdminPage
	{
		protected YesNoRadioButtonList radEnableAliOHShengPay;
		protected System.Web.UI.WebControls.TextBox txtPartner;
		protected System.Web.UI.WebControls.TextBox txtKey;
		protected System.Web.UI.WebControls.Button btnOK;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			if (!base.IsPostBack)
			{
				SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
				this.radEnableAliOHShengPay.SelectedValue = masterSettings.EnableAliOHShengPay;
				PaymentModeInfo paymentMode = SalesHelper.GetPaymentMode("Ecdev.Plugins.Payment.ShengPayMobile.ShengPayMobileRequest");
				if (paymentMode != null)
				{
					string xml = HiCryptographer.Decrypt(paymentMode.Settings);
					System.Xml.XmlDocument xmlDocument = new System.Xml.XmlDocument();
					xmlDocument.LoadXml(xml);
					try
					{
						this.txtPartner.Text = xmlDocument.GetElementsByTagName("SenderId")[0].InnerText;
						this.txtKey.Text = xmlDocument.GetElementsByTagName("SellerKey")[0].InnerText;
					}
					catch
					{
						this.txtPartner.Text = "";
						this.txtKey.Text = "";
					}
				}
				PaymentModeInfo paymentMode2 = SalesHelper.GetPaymentMode("Ecdev.Plugins.Payment.ShengPayMobile.ShengPayMobileRequest");
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
			masterSettings.EnableAliOHShengPay = this.radEnableAliOHShengPay.SelectedValue;
			SettingsManager.Save(masterSettings);
			string text = string.Format("<xml><SenderId>{0}</SenderId><SellerKey>{1}</SellerKey><Seller_account_name></Seller_account_name></xml>", this.txtPartner.Text, this.txtKey.Text);
			PaymentModeInfo paymentMode = SalesHelper.GetPaymentMode("Ecdev.Plugins.Payment.ShengPayMobile.ShengPayMobileRequest");
			if (paymentMode == null)
			{
				PaymentModeInfo paymentMode2 = new PaymentModeInfo
				{
					Name = "盛付通手机网页支付",
					Gateway = "Ecdev.Plugins.Payment.ShengPayMobile.ShengPayMobileRequest",
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

using EcShop.ControlPanel.Sales;
using EcShop.ControlPanel.Store;
using EcShop.Core;
using EcShop.Entities.Sales;
using EcShop.Entities.Store;
using EcShop.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Xml;
namespace EcShop.UI.Web.Admin.WAPShop
{
	[PrivilegeCheck(Privilege.WapMobileAlipaySet)]
	public class BasicConfig : AdminPage
	{
		protected System.Web.UI.WebControls.TextBox txtPartner;
		protected System.Web.UI.WebControls.TextBox txtKey;
		protected System.Web.UI.WebControls.TextBox txtAccount;
		protected System.Web.UI.WebControls.Button btnOK;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			if (!base.IsPostBack)
			{
				System.Collections.Generic.IList<PaymentModeInfo> paymentModes = SalesHelper.GetPaymentModes(PayApplicationType.payOnVX);
				if (paymentModes == null || paymentModes.Count == 0)
				{
					return;
				}
				string xml = HiCryptographer.Decrypt(paymentModes[0].Settings);
				System.Xml.XmlDocument xmlDocument = new System.Xml.XmlDocument();
				xmlDocument.LoadXml(xml);
				this.txtPartner.Text = xmlDocument.GetElementsByTagName("Partner")[0].InnerText;
				this.txtKey.Text = xmlDocument.GetElementsByTagName("Key")[0].InnerText;
				this.txtAccount.Text = xmlDocument.GetElementsByTagName("Seller_account_name")[0].InnerText;
			}
		}
		protected void btnOK_Click(object sender, System.EventArgs e)
		{
			string text = string.Format("<xml><Partner>{0}</Partner><Key>{1}</Key><Seller_account_name>{2}</Seller_account_name></xml>", this.txtPartner.Text, this.txtKey.Text, this.txtAccount.Text);
			System.Collections.Generic.IList<PaymentModeInfo> paymentModes = SalesHelper.GetPaymentModes(PayApplicationType.payOnVX);
			if (paymentModes == null || paymentModes.Count == 0)
			{
				PaymentModeInfo paymentMode = new PaymentModeInfo
				{
					Name = "支付宝手机支付",
					Gateway = "Ecdev.plugins.payment.ws_wappay.wswappayrequest",
					Description = string.Empty,
					IsUseInpour = true,
					Charge = 0m,
					IsPercent = false,
					Settings = HiCryptographer.Encrypt(text)
				};
				if (SalesHelper.CreatePaymentMode(paymentMode) == PaymentModeActionStatus.Success)
				{
					this.ShowMsg("设置成功", true);
					return;
				}
				this.ShowMsg("设置失败", false);
				return;
			}
			else
			{
				PaymentModeInfo paymentModeInfo = paymentModes[0];
				paymentModeInfo.Settings = HiCryptographer.Encrypt(text);
				if (SalesHelper.UpdatePaymentMode(paymentModeInfo) == PaymentModeActionStatus.Success)
				{
					this.ShowMsg("设置成功", true);
					return;
				}
				this.ShowMsg("设置失败", false);
				return;
			}
		}
	}
}

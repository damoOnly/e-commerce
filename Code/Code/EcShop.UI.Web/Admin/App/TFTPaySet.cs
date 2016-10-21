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
namespace EcShop.UI.Web.Admin.App
{
	[PrivilegeCheck(Privilege.AppTFTPaySet)]
	public class TFTPaySet : AdminPage
	{
		protected YesNoRadioButtonList radEnableAppTFTPay;
		protected System.Web.UI.WebControls.TextBox txtCert;
		protected System.Web.UI.WebControls.TextBox txtKey;
        protected System.Web.UI.WebControls.TextBox txtPublicKey;
        protected System.Web.UI.WebControls.TextBox txtGateway;
		protected System.Web.UI.WebControls.Button btnOK;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);

			if (!base.IsPostBack)
			{
				SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
				this.radEnableAppTFTPay.SelectedValue = masterSettings.EnableAppTFTPay;

				PaymentModeInfo paymentMode = SalesHelper.GetPaymentMode("Ecdev.plugins.payment.tft_apppay.tftwappayrequest");

				if (paymentMode != null)
				{
				    this.txtCert.Text = "";
				    this.txtKey.Text = "";
                    this.txtPublicKey.Text = "";
                    this.txtGateway.Text = "";

                    if (paymentMode.Settings != "")
                    {
                        string xml = HiCryptographer.Decrypt(paymentMode.Settings);

                        try
                        {
                            System.Xml.XmlDocument xmlDocument = new System.Xml.XmlDocument();
                            xmlDocument.LoadXml(xml);

                            this.txtCert.Text = xmlDocument.GetElementsByTagName("Cert")[0].InnerText;
                            this.txtKey.Text = xmlDocument.GetElementsByTagName("CertKey")[0].InnerText;
                            this.txtPublicKey.Text = xmlDocument.GetElementsByTagName("PublicKey")[0].InnerText;
                            this.txtGateway.Text = xmlDocument.GetElementsByTagName("Gateway")[0].InnerText;
                        }
                        catch
                        {
                            this.txtCert.Text = "";
                            this.txtKey.Text = "";
                            this.txtPublicKey.Text = "";
                            this.txtGateway.Text = "";
                        }
                    }
				}

                //PaymentModeInfo paymentMode2 = SalesHelper.GetPaymentMode("Ecdev.plugins.payment.tft_apppay.tftwappayrequest");
                //if (paymentMode2 != null)
                //{
                //    string xml2 = HiCryptographer.Decrypt(paymentMode2.Settings);
                //    System.Xml.XmlDocument xmlDocument2 = new System.Xml.XmlDocument();
                //    xmlDocument2.LoadXml(xml2);
                //}
			}
		}
		protected void btnOK_Click(object sender, System.EventArgs e)
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
            masterSettings.EnableAppTFTPay = this.radEnableAppTFTPay.SelectedValue;
			SettingsManager.Save(masterSettings);

            string text = string.Format("<xml><Cert>{0}</Cert><CertKey>{1}</CertKey><PublicKey>{2}</PublicKey><Gateway>{3}</Gateway></xml>", this.txtCert.Text, this.txtKey.Text, this.txtPublicKey.Text, this.txtGateway.Text);
			
            PaymentModeInfo paymentMode = SalesHelper.GetPaymentMode("Ecdev.plugins.payment.tft_apppay.tftwappayrequest");
			if (paymentMode == null)
			{
                paymentMode = new PaymentModeInfo
				{
					Name = "腾付通移动支付",
					Gateway = "Ecdev.plugins.payment.tft_apppay.tftwappayrequest",
					Description = string.Empty,
					IsUseInpour = false,
					Charge = 0m,
					IsPercent = false,
					ApplicationType = PayApplicationType.payOnApp,
					Settings = HiCryptographer.Encrypt(text)
				};

                SalesHelper.CreatePaymentMode(paymentMode);
			}
			else
			{
				PaymentModeInfo paymentModeInfo = paymentMode;
				paymentModeInfo.Settings = HiCryptographer.Encrypt(text);
				paymentModeInfo.ApplicationType = PayApplicationType.payOnApp;
				SalesHelper.UpdatePaymentMode(paymentModeInfo);
			}
			this.ShowMsg("修改成功", true);
		}
	}
}

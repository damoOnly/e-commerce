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
    [PrivilegeCheck(Privilege.AppWxPaySet)]
    public class AppWxPaySet : AdminPage
	{
        protected YesNoRadioButtonList radEnableAppWxPay;
        protected System.Web.UI.WebControls.TextBox txtAppId;
        protected System.Web.UI.WebControls.TextBox txtAppSecret;
        protected System.Web.UI.WebControls.TextBox txtMch_id;
        protected System.Web.UI.WebControls.TextBox txtKey;
		protected System.Web.UI.WebControls.Button btnOK;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);

			if (!base.IsPostBack)
			{
				SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
                this.radEnableAppWxPay.SelectedValue = masterSettings.EnableAppWxPay;

                PaymentModeInfo paymentMode = SalesHelper.GetPaymentMode("Ecdev.plugins.payment.wx_apppay.wxwappayrequest");

				if (paymentMode != null)
				{
                    this.txtAppId.Text = "";
                    this.txtAppSecret.Text = "";
                    this.txtMch_id.Text = "";
                    this.txtKey.Text = "";

                    if (paymentMode.Settings != "")
                    {
                        string xml = HiCryptographer.Decrypt(paymentMode.Settings);

                        try
                        {
                            System.Xml.XmlDocument xmlDocument = new System.Xml.XmlDocument();
                            xmlDocument.LoadXml(xml);

                            this.txtAppId.Text = xmlDocument.GetElementsByTagName("AppId")[0].InnerText;
                            this.txtKey.Text = xmlDocument.GetElementsByTagName("Key")[0].InnerText;
                            this.txtAppSecret.Text = xmlDocument.GetElementsByTagName("AppSecret")[0].InnerText;
                            this.txtMch_id.Text = xmlDocument.GetElementsByTagName("Mch_id")[0].InnerText;
                        }
                        catch
                        {
                            this.txtAppId.Text = "";
                            this.txtKey.Text = "";
                            this.txtAppSecret.Text = "";
                            this.txtMch_id.Text = "";
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
            masterSettings.EnableAppWxPay = this.radEnableAppWxPay.SelectedValue;
			SettingsManager.Save(masterSettings);

            string text = string.Format("<xml><AppId>{0}</AppId><Key>{1}</Key><AppSecret>{2}</AppSecret><Mch_id>{3}</Mch_id></xml>", this.txtAppId.Text, this.txtKey.Text, this.txtAppSecret.Text, this.txtMch_id.Text);

            PaymentModeInfo paymentMode = SalesHelper.GetPaymentMode("Ecdev.plugins.payment.wx_apppay.wxwappayrequest");
			if (paymentMode == null)
			{
                paymentMode = new PaymentModeInfo
				{
					Name = "微信支付",
                    Gateway = "Ecdev.plugins.payment.wx_apppay.wxwappayrequest",
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

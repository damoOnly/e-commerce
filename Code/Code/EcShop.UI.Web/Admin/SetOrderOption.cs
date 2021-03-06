using EcShop.ControlPanel.Store;
using EcShop.Core;
using EcShop.Membership.Context;
using EcShop.UI.Common.Controls;
using EcShop.UI.ControlPanel.Utility;
using Ecdev.Components.Validation;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;
namespace EcShop.UI.Web.Admin
{
	[AdministerCheck(true)]
	public class SetOrderOption : AdminPage
	{
		protected System.Web.UI.WebControls.TextBox txtShowDays;
		protected System.Web.UI.HtmlControls.HtmlGenericControl txtShowDaysTip;
		protected System.Web.UI.WebControls.TextBox txtCloseOrderDays;
		protected System.Web.UI.HtmlControls.HtmlGenericControl txtCloseOrderDaysTip;
		protected System.Web.UI.WebControls.TextBox txtFinishOrderDays;
		protected System.Web.UI.HtmlControls.HtmlGenericControl txtFinishOrderDaysTip;
		protected System.Web.UI.WebControls.TextBox txtEndOrderDays;
		protected System.Web.UI.HtmlControls.HtmlGenericControl txtEndOrderDaysTip;
		protected System.Web.UI.WebControls.TextBox txtTaxRate;
		protected System.Web.UI.HtmlControls.HtmlGenericControl txtTaxRateTip;
		protected System.Web.UI.WebControls.Button btnOK;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!this.Page.IsPostBack)
			{
				SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
				this.txtShowDays.Text = masterSettings.OrderShowDays.ToString(System.Globalization.CultureInfo.InvariantCulture);
				this.txtCloseOrderDays.Text = masterSettings.CloseOrderDays.ToString(System.Globalization.CultureInfo.InvariantCulture);
				this.txtFinishOrderDays.Text = masterSettings.FinishOrderDays.ToString(System.Globalization.CultureInfo.InvariantCulture);
				this.txtEndOrderDays.Text = masterSettings.EndOrderDays.ToString(System.Globalization.CultureInfo.InvariantCulture);
				this.txtTaxRate.Text = masterSettings.TaxRate.ToString(System.Globalization.CultureInfo.InvariantCulture);
				System.Xml.XmlDocument xmlDocument = new System.Xml.XmlDocument();
				string filename = System.Web.HttpContext.Current.Request.MapPath("~/Express.xml");
				xmlDocument.Load(filename);
				xmlDocument.SelectSingleNode("companys");
			}
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
		}
		protected void btnOK_Click(object sender, System.EventArgs e)
		{
			int orderShowDays;
			int closeOrderDays;
			int finishOrderDays;
			int endOrderDays;
			decimal taxRate;
			if (!this.ValidateValues(out orderShowDays, out closeOrderDays, out finishOrderDays, out endOrderDays, out taxRate))
			{
				return;
			}
			SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
			masterSettings.OrderShowDays = orderShowDays;
			masterSettings.CloseOrderDays = closeOrderDays;
			masterSettings.FinishOrderDays = finishOrderDays;
			masterSettings.EndOrderDays = endOrderDays;
			masterSettings.TaxRate = taxRate;
			if (!this.ValidationSettings(masterSettings))
			{
				return;
			}
			Globals.EntityCoding(masterSettings, true);
			SettingsManager.Save(masterSettings);
			this.SavaKuaidi100Key();
			this.ShowMsg("保存成功", true);
		}
		private bool ValidateValues(out int showDays, out int closeOrderDays, out int finishOrderDays, out int endOrderDays, out decimal taxRate)
		{
			string text = string.Empty;
			if (!int.TryParse(this.txtShowDays.Text, out showDays))
			{
				text += Formatter.FormatErrorMessage("订单显示设置不能为空,必须为正整数,范围在1-90之间");
			}
			if (!int.TryParse(this.txtCloseOrderDays.Text, out closeOrderDays))
			{
				text += Formatter.FormatErrorMessage("过期几天自动关闭订单不能为空,必须为正整数,范围在1-90之间");
			}
			if (!int.TryParse(this.txtFinishOrderDays.Text, out finishOrderDays))
			{
				text += Formatter.FormatErrorMessage("发货几天自动完成订单不能为空,必须为正整数,范围在1-90之间");
			}
			if (!int.TryParse(this.txtEndOrderDays.Text, out endOrderDays))
			{
				text += Formatter.FormatErrorMessage("订单完成几天后，系统自动结束交易，不得再申请退换货服务,必须为正整数,范围在1-90之间");
			}
			if (!decimal.TryParse(this.txtTaxRate.Text, out taxRate))
			{
				text += Formatter.FormatErrorMessage("订单发票税率不能为空,为非负数字,范围在0-100之间");
			}
			if (!string.IsNullOrEmpty(text))
			{
				this.ShowMsg(text, false);
				return false;
			}
			return true;
		}
		private bool ValidationSettings(SiteSettings setting)
		{
			ValidationResults validationResults = Validation.Validate<SiteSettings>(setting, new string[]
			{
				"ValMasterSettings"
			});
			string text = string.Empty;
			if (!validationResults.IsValid)
			{
				foreach (ValidationResult current in (System.Collections.Generic.IEnumerable<ValidationResult>)validationResults)
				{
					text += Formatter.FormatErrorMessage(current.Message);
				}
				this.ShowMsg(text, false);
			}
			return validationResults.IsValid;
		}
		private void SavaKuaidi100Key()
		{
			System.Xml.XmlDocument xmlDocument = new System.Xml.XmlDocument();
			string filename = System.Web.HttpContext.Current.Request.MapPath("~/Express.xml");
			xmlDocument.Load(filename);
			xmlDocument.SelectSingleNode("companys");
			xmlDocument.Save(filename);
		}
	}
}

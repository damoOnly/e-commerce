using EcShop.ControlPanel.Sales;
using EcShop.Core;
using EcShop.Entities.Sales;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
namespace EcShop.UI.ControlPanel.Utility
{
	public class PaymentDropDownList : DropDownList
	{
		private bool allowNull = true;
		public bool AllowNull
		{
			get
			{
				return this.allowNull;
			}
			set
			{
				this.allowNull = value;
			}
		}
		public new int? SelectedValue
		{
			get
			{
				if (string.IsNullOrEmpty(base.SelectedValue))
				{
					return null;
				}
				return new int?(int.Parse(base.SelectedValue));
			}
			set
			{
				if (!value.HasValue)
				{
					base.SelectedValue = string.Empty;
					return;
				}
				base.SelectedValue = value.ToString();
			}
		}
		public override void DataBind()
		{
			base.Items.Clear();
			if (this.AllowNull)
			{
				base.Items.Add(new ListItem(string.Empty, string.Empty));
			}
			IList<PaymentModeInfo> paymentModes = SalesHelper.GetPaymentModes(PayApplicationType.payOnAll);
			foreach (PaymentModeInfo current in paymentModes)
			{
				base.Items.Add(new ListItem(Globals.HtmlDecode(current.Name), current.ModeId.ToString()));
			}
		}
	}
}

using EcShop.ControlPanel.Sales;
using EcShop.Entities.Sales;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI.WebControls;
namespace EcShop.UI.ControlPanel.Utility
{
	public class ShippersDropDownList : DropDownList
	{
		private bool includeDistributor;
		public new int SelectedValue
		{
			get
			{
				if (string.IsNullOrEmpty(base.SelectedValue))
				{
					return 0;
				}
				return int.Parse(base.SelectedValue, CultureInfo.InvariantCulture);
			}
			set
			{
				base.SelectedIndex = base.Items.IndexOf(base.Items.FindByValue(value.ToString(CultureInfo.InvariantCulture)));
			}
		}
		public bool IncludeDistributor
		{
			get
			{
				return this.includeDistributor;
			}
			set
			{
				this.includeDistributor = value;
			}
		}
		public override void DataBind()
		{
			this.Items.Clear();
			IList<ShippersInfo> shippers = SalesHelper.GetShippers(this.IncludeDistributor);
			foreach (ShippersInfo current in shippers)
			{
				this.Items.Add(new ListItem(current.ShipperTag, current.ShipperId.ToString()));
			}
		}
	}
}

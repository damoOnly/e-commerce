using System;
using System.Globalization;
using System.Web.UI.WebControls;
namespace EcShop.UI.ControlPanel.Utility
{
	public class SourceOrderDrowpDownList : DropDownList
	{
		private bool allowNull = true;
		private string nullToDisplay = "";
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
		public string NullToDisplay
		{
			get
			{
				return this.nullToDisplay;
			}
			set
			{
				this.nullToDisplay = value;
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
				return new int?(int.Parse(base.SelectedValue, CultureInfo.InvariantCulture));
			}
			set
			{
				if (value.HasValue)
				{
					base.SelectedIndex = base.Items.IndexOf(base.Items.FindByValue(value.Value.ToString(CultureInfo.InvariantCulture)));
				}
			}
		}
		public override void DataBind()
		{
			this.Items.Clear();
			if (this.AllowNull)
			{
				base.Items.Add(new ListItem(this.NullToDisplay, string.Empty));
			}
			base.Items.Add(new ListItem("pc端订单", "1"));
			base.Items.Add(new ListItem("淘宝订单", "2"));
			base.Items.Add(new ListItem("微信订单", "3"));
			base.Items.Add(new ListItem("触屏版订单", "4"));
			base.Items.Add(new ListItem("服务窗订单", "5"));
			base.Items.Add(new ListItem("App订单", "6"));
            base.Items.Add(new ListItem("Android订单", "11"));
            base.Items.Add(new ListItem("IOS订单", "12"));
		}
	}
}

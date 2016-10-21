using EcShop.ControlPanel.Sales;
using EcShop.Core;
using System;
using System.Data;
using System.Globalization;
using System.Web.UI.WebControls;
namespace EcShop.UI.ControlPanel.Utility
{
	public class ShippingTemplatesDropDownList : DropDownList
	{
		private bool allowNull = true;
        private string nullToDisplay = "请选择运费模版";
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
			DataTable shippingAllTemplates = SalesHelper.GetShippingAllTemplates();
			foreach (DataRow dataRow in shippingAllTemplates.Rows)
			{
				this.Items.Add(new ListItem(Globals.HtmlDecode(dataRow["TemplateName"].ToString()), dataRow["TemplateId"].ToString()));
			}
		}
	}
}

using EcShop.UI.Common.Controls;
using System;
using System.Data;
using System.Globalization;
using System.Web.UI.WebControls;
namespace EcShop.UI.ControlPanel.Utility
{
	public class BrandCategoriesCheckBoxList : CheckBoxList
	{
		private RepeatDirection repeatDirection;
		private int repeatColumns = 7;
		public override RepeatDirection RepeatDirection
		{
			get
			{
				return this.repeatDirection;
			}
			set
			{
				this.repeatDirection = value;
			}
		}
		public override int RepeatColumns
		{
			get
			{
				return this.repeatColumns;
			}
			set
			{
				this.repeatColumns = value;
			}
		}
		public override void DataBind()
		{
			this.Items.Clear();
			DataTable brandCategories = ControlProvider.Instance().GetBrandCategories();
			foreach (DataRow dataRow in brandCategories.Rows)
			{
				this.Items.Add(new ListItem((string)dataRow["BrandName"], ((int)dataRow["BrandId"]).ToString(CultureInfo.InvariantCulture)));
			}
		}
	}
}

using EcShop.Entities.Commodities;
using EcShop.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
namespace EcShop.UI.ControlPanel.Utility
{
	public class ProductTypesCheckBoxList : CheckBoxList
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
			IList<ProductTypeInfo> productTypes = ControlProvider.Instance().GetProductTypes();
			foreach (ProductTypeInfo current in productTypes)
			{
				base.Items.Add(new ListItem(current.TypeName, current.TypeId.ToString()));
			}
		}
	}
}

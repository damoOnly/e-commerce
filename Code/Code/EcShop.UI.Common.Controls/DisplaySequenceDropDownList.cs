using System;
using System.Web.UI.WebControls;
namespace EcShop.UI.Common.Controls
{
    public class DisplaySequenceDropDownList : DropDownList
	{
		public new int? SelectedValue
		{
			get
			{
				int value = 0;
				int.TryParse(base.SelectedValue, out value);
				return new int?(value);
			}
			set
			{
				if (value.HasValue)
				{
					base.SelectedValue = value.Value.ToString();
				}
			}
		}
		public override void DataBind()
		{
			this.Items.Clear();
			for (int i = 0; i <= 1000; i++)
			{
				this.Items.Add(new ListItem(i.ToString(), i.ToString()));
			}
		}
	}
}

using System;
using System.Web.UI.WebControls;
namespace EcShop.UI.Common.Controls
{
    public class OrderTimeTypeDropDownList : DropDownList
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

            ListItem item = new ListItem();
            item.Selected = true;
            item.Value = "1";
            item.Text = "下单时间";
            this.Items.Add(item);

            this.Items.Add(new ListItem("付款时间","2"));
            this.Items.Add(new ListItem("发货时间","3"));
		}
	}
}

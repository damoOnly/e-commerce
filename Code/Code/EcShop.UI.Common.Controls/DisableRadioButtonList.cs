using System;
using System.Web.UI.WebControls;
namespace EcShop.UI.Common.Controls
{
    public class DisableRadioButtonList : RadioButtonList
    {
        public DisableRadioButtonList()
		{
			this.Items.Clear();
			this.Items.Add(new ListItem("启用", "0"));
			this.Items.Add(new ListItem("禁用", "1"));
            this.RepeatDirection = RepeatDirection.Horizontal;
			this.Items[0].Selected = true;
		}
    }
}

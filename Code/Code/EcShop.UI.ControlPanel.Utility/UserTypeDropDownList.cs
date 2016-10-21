using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;

namespace EcShop.UI.ControlPanel.Utility
{
    public class UserTypeDropDownList: DropDownList
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
            base.Items.Add(new ListItem("pc端", "1"));
            base.Items.Add(new ListItem("微信端", "3"));
            base.Items.Add(new ListItem("Android端", "11"));
            base.Items.Add(new ListItem("IOS端", "12"));
            base.Items.Add(new ListItem("第三方绑定", "13"));
        }
    }
}

using Commodities;
using EcShop.ControlPanel.Comments;
using EcShop.ControlPanel.Commodities;
using EcShop.Core;
using EcShop.Entities.Commodities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;

namespace EcShop.UI.ControlPanel.Utility
{
    public class BaseCountryDropDownList : DropDownList
    {
        private bool allowNull = true;
        private string nullToDisplay = "--请选择国家--";
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
        /// <summary>
        /// 默认选项
        /// </summary>
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

        /// <summary>
        /// 获取值或设置选择项
        /// </summary>
        public new string SelectedValue
        {
            get
            {
                if (string.IsNullOrEmpty(base.SelectedValue))
                {
                    return null;
                }
                return base.SelectedValue;
            }
            set
            {
                if (base.Items == null || base.Items.Count <= 0 || value == null || value.Length == 0)
                {
                    base.SelectedIndex = -1;
                    return;
                }

                base.SelectedIndex = base.Items.IndexOf(base.Items.FindByValue(value));
            }
        }

        /// <summary>
        /// 绑定数据
        /// </summary>
        public override void DataBind()
        {
            this.Items.Clear();
            this.Items.Add(new ListItem(this.NullToDisplay, string.Empty));
            DataTable dtCountry = BaseCountryHelper.GetBaseCountry();
            if (dtCountry != null && dtCountry.Rows.Count > 0)
            {
                for (int i = 0; i < dtCountry.Rows.Count; i++)
                {
                    string itemValue = dtCountry.Rows[i]["HSCode"].ToString();
                    ListItem item = new ListItem(Globals.HtmlDecode(dtCountry.Rows[i]["CName"].ToString()), itemValue);
                    this.Items.Add(item);
                }
            }
        }
    }
}

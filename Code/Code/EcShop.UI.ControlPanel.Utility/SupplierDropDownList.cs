using Commodities;
using EcShop.ControlPanel.Comments;
using EcShop.ControlPanel.Commodities;
using EcShop.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;

namespace EcShop.UI.ControlPanel.Utility
{
    public class SupplierDropDownList : DropDownList
    {
        private bool allowNull = true;
        private string nullToDisplay = "--请选择供货商--";
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
                int? supplierId;
                string supplierCode;
                GetSupplierIdAndCode(out supplierId, out supplierCode);
                return supplierId;
            }
            set
            {
                if (base.Items.Count <= 0 || !value.HasValue)
                {
                    base.SelectedIndex = -1;
                    return;
                }

                for (int i = 0; i < base.Items.Count; i++)
                {
                    if (base.Items[i].Value.StartsWith(value.Value + "|"))
                    {
                        base.SelectedIndex = i;
                        break;
                    }
                }
            }
        }
        public string SupplierCode
        {
            get
            {
                int? supplierId;
                string supplierCode;
                GetSupplierIdAndCode(out supplierId, out supplierCode);
                return supplierCode;
            }
        }
        public string OnClientChange
        {
            set 
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    this.Attributes.Add("onchange", value);
                }
            }
        }
        public override void DataBind()
        {
            this.Items.Clear();
            this.Items.Add(new ListItem(this.NullToDisplay, string.Empty));
            DataTable supplierdt = SupplierHelper.GetSupplier();
            if (supplierdt != null && supplierdt.Rows.Count > 0)
            {
                for (int i = 0; i < supplierdt.Rows.Count; i++)
                {
                    string itemValue = supplierdt.Rows[i]["SupplierId"].ToString();
                    string code = Globals.HtmlDecode(supplierdt.Rows[i]["SupplierCode"].ToString());
                    if (string.IsNullOrEmpty(code))
                    {
                        code = "0";
                    }
                    itemValue += "|" + code;
                    ListItem item = new ListItem(Globals.HtmlDecode(supplierdt.Rows[i]["SupplierName"].ToString()), itemValue);
                    //item.Attributes.Add("Code", Globals.HtmlDecode(supplierdt.Rows[i]["SupplierCode"].ToString()));
                    this.Items.Add(item);
                }
            }
        }

        public void BindDataBind()
        {
            this.Items.Clear();
            this.Items.Add(new ListItem(this.NullToDisplay, string.Empty));
            DataTable supplierdt = SupplierHelper.GetSupplier();
            if (supplierdt != null && supplierdt.Rows.Count > 0)
            {
                for (int i = 0; i < supplierdt.Rows.Count; i++)
                {
                    string itemValue = supplierdt.Rows[i]["SupplierId"].ToString();
                    ListItem item = new ListItem(Globals.HtmlDecode(supplierdt.Rows[i]["SupplierName"].ToString()), itemValue);
                    this.Items.Add(item);
                }
            }
        }

        private void GetSupplierIdAndCode(out int? supplierId, out string supplierCode)
        {
            string value = base.SelectedValue;
            if (string.IsNullOrEmpty(value))
            {
                supplierId = null;
                supplierCode = "0";
                return;
            }
            else
            {
                if (value.Contains("|"))
                {
                    int separatorIndex = value.IndexOf("|");
                    supplierId = int.Parse(value.Substring(0, separatorIndex));
                    supplierCode = value.Substring(separatorIndex + 1);
                }
                else {
                    supplierId = int.Parse(value);
                    supplierCode = "0";
                }
            }
        }
    }
}

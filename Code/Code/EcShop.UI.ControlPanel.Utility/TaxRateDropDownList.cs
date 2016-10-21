using EcShop.ControlPanel.Comments;
using EcShop.Core;
using EcShop.Entities.Comments;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI.WebControls;
namespace EcShop.UI.ControlPanel.Utility
{
	public class TaxRateDropDownList : DropDownList
	{
		private bool allowNull = true;
		private string nullToDisplay = "--ÇëÑ¡ÔñË°ÂÊ--";
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
				return new int?(int.Parse(base.SelectedValue));
			}
			set
			{
				if (value.HasValue)
				{
					base.SelectedIndex = base.Items.IndexOf(base.Items.FindByValue(value.Value.ToString(CultureInfo.InvariantCulture)));
					return;
				}
				base.SelectedIndex = -1;
			}
		}
		public override void DataBind()
		{
			this.Items.Clear();
			this.Items.Add(new ListItem(this.NullToDisplay, string.Empty));
            IList<TaxRateInfo> mainTaxRate = TaxRateHelper.GetMainTaxRate();
            foreach (TaxRateInfo current in mainTaxRate)
			{
                if(current.TaxRate == (decimal)0.01)
                {
                    ListItem listItem = new ListItem(Globals.HtmlDecode(current.TaxRate.ToString()), current.TaxRateId.ToString());
                    listItem.Selected = true;
                    this.Items.Add(listItem);
                }
                else
                {
                     this.Items.Add(new ListItem(Globals.HtmlDecode(current.TaxRate.ToString()), current.TaxRateId.ToString()));
                }
			}
		}

        public void DataBind(int categoryId)
        {
            this.Items.Clear();
            this.Items.Add(new ListItem(this.NullToDisplay, string.Empty));
            IList<TaxRateInfo> mainTaxRate = TaxRateHelper.GetMainTaxRate(categoryId);
            foreach (TaxRateInfo current in mainTaxRate)
            {
                if (current.TaxRate == (decimal)0.01)
                {
                    ListItem listItem = new ListItem(Globals.HtmlDecode(current.TaxRate.ToString()), current.TaxRateId.ToString());
                    listItem.Selected = true;
                    this.Items.Add(listItem);
                }
                else
                {
                    this.Items.Add(new ListItem(Globals.HtmlDecode(current.TaxRate.ToString()), current.TaxRateId.ToString()));
                }
            }
        }
	}
}

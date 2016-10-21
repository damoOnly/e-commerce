using EcShop.ControlPanel.Commodities;
using EcShop.Entities.Sales;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;
namespace EcShop.UI.Common.Controls
{
	public class SitesDropDownList : DropDownList
	{
		private bool allowNull = true;
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
			get;
			set;
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
				if (!value.HasValue)
				{
					base.SelectedValue = string.Empty;
					return;
				}
				base.SelectedValue = value.ToString();
			}
		}
		public override void DataBind()
		{
			base.Items.Clear();
			if (this.AllowNull)
			{
				base.Items.Add(new ListItem(this.NullToDisplay, string.Empty));
            }
            else
            {
                base.Items.Add(new ListItem("全部","-1"));  
            }
            DataTable dtSites = SitesManagementHelper.GetSites();
            int count = dtSites.Rows.Count;
            for (int i = 0; i <count ; i++)
            {
                base.Items.Add(new ListItem(dtSites.Rows[i]["SitesName"].ToString(), dtSites.Rows[i]["SitesId"].ToString()));
            }
		}
	}
}

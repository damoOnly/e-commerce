using EcShop.Entities;
using EcShop.Entities.Sales;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
namespace EcShop.UI.ControlPanel.Utility
{
	public class ExpressCheckBoxList : CheckBoxList
	{
		private IList<string> expressCompany;
		public IList<string> ExpressCompany
		{
			get
			{
				if (this.expressCompany == null)
				{
					return new List<string>();
				}
				return this.expressCompany;
			}
			set
			{
				this.expressCompany = value;
			}
		}
		public void BindExpressCheckBoxList()
		{
			base.Items.Clear();
			IList<ExpressCompanyInfo> allExpress = ExpressHelper.GetAllExpress();
			foreach (ExpressCompanyInfo current in allExpress)
			{
				ListItem listItem = new ListItem(current.Name, current.Name);
				if (this.ExpressCompany != null)
				{
					foreach (string current2 in this.ExpressCompany)
					{
						if (string.Compare(listItem.Value, current2, false) == 0)
						{
							listItem.Selected = true;
						}
					}
				}
				base.Items.Add(listItem);
			}
		}
		public override void DataBind()
		{
			this.BindExpressCheckBoxList();
			base.DataBind();
		}
	}
}

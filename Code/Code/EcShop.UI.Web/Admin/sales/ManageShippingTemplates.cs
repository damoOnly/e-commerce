using ASPNET.WebControls;
using EcShop.ControlPanel.Sales;
using EcShop.ControlPanel.Store;
using EcShop.Core.Entities;
using EcShop.Core.Enums;
using EcShop.Entities.Store;
using EcShop.UI.ControlPanel.Utility;
using System;
using System.Web.UI.WebControls;
namespace EcShop.UI.Web.Admin.sales
{
	[PrivilegeCheck(Privilege.ShippingTemplets)]
	public class ManageShippingTemplates : AdminPage
	{
		protected Grid grdShippingTemplates;
		protected Pager pager;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.grdShippingTemplates.RowCommand += new System.Web.UI.WebControls.GridViewCommandEventHandler(this.grdShippingTemplates_RowCommand);
			if (!this.Page.IsPostBack)
			{
				this.BindShippingTemplates();
			}
		}
		private void BindShippingTemplates()
		{
			DbQueryResult shippingTemplates = SalesHelper.GetShippingTemplates(new Pagination
			{
				PageIndex = this.pager.PageIndex,
				PageSize = this.pager.PageSize,
				IsCount = true,
				SortBy = "TemplateId",
				SortOrder = SortAction.Desc
			});
			this.grdShippingTemplates.DataSource = shippingTemplates.Data;
			this.grdShippingTemplates.DataBind();
			this.pager.TotalRecords = shippingTemplates.TotalRecords;
		}
		private void grdShippingTemplates_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
		{
			int num = 0;
			if (e.CommandName == "DEL_Template")
			{
				int.TryParse(e.CommandArgument.ToString(), out num);
				if (num > 0)
				{
					SalesHelper.DeleteShippingTemplate(num);
					this.BindShippingTemplates();
				}
			}
		}
	}
}

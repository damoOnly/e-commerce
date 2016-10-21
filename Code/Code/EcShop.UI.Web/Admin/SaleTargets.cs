using ASPNET.WebControls;
using EcShop.ControlPanel.Sales;
using EcShop.ControlPanel.Store;
using EcShop.Core.Entities;
using EcShop.Entities.Store;
using EcShop.UI.ControlPanel.Utility;
using System;
namespace EcShop.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.SaleTargets)]
	public class SaleTargets : AdminPage
	{
		protected Grid grdOrderAvPrice;
		protected Grid grdVisitOrderAvPrice;
		protected Grid grdOrderTranslatePercentage;
		protected Grid grdUserOrderPercentage;
		protected Grid grdUserOrderAvNumb;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!this.Page.IsPostBack)
			{
				DbQueryResult saleTargets = SalesHelper.GetSaleTargets();
				this.grdOrderAvPrice.DataSource = saleTargets.Data;
				this.grdOrderAvPrice.DataBind();
				this.grdOrderTranslatePercentage.DataSource = saleTargets.Data;
				this.grdOrderTranslatePercentage.DataBind();
				this.grdUserOrderAvNumb.DataSource = saleTargets.Data;
				this.grdUserOrderAvNumb.DataBind();
				this.grdVisitOrderAvPrice.DataSource = saleTargets.Data;
				this.grdVisitOrderAvPrice.DataBind();
				this.grdUserOrderPercentage.DataSource = saleTargets.Data;
				this.grdUserOrderPercentage.DataBind();
			}
		}
	}
}

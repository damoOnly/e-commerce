using ASPNET.WebControls;
using EcShop;
using EcShop.ControlPanel.Commodities;
using EcShop.ControlPanel.Store;
using EcShop.Entities.VShop;
using EcShop.UI.Common.Controls;
using EcShop.UI.ControlPanel.Utility;
using System;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace EcShop.UI.Web.Admin.product
{
    public class PrintProduct: AdminPage
	{
		private int StoreId;
        private string ProductId;

        protected Grid grdproducts;
		protected System.Web.UI.WebControls.LinkButton btnFinish;
		protected void Page_Load(object sender, System.EventArgs e)
		{
            if (!int.TryParse(this.Page.Request.QueryString["StoreId"], out this.StoreId))
			{
                this.StoreId = 0;
			}
            if (this.Page.Request.QueryString["ProductsId"] == null)
            {
                base.GotoResourceNotFound();
                return;
            }
			if (!this.Page.IsPostBack)
			{
                this.ProductId = this.Page.Request.QueryString["ProductsId"].ToString();
				this.BindStoreProducts();
			}
		}

        private void BindStoreProducts()
		{
            string ArrProduct=this.ProductId.Substring(0,this.ProductId.Length-1);
           
            this.grdproducts.DataSource = StoreManagementHelper.PrintProducts(this.StoreId, ArrProduct);
            this.grdproducts.DataBind();
		}
	}
}

using ASPNET.WebControls;
using EcShop.ControlPanel.Commodities;
using EcShop.ControlPanel.Store;
using EcShop.Core;
using EcShop.Entities.Store;
using EcShop.UI.Common.Controls;
using EcShop.UI.ControlPanel.Utility;
using System;
using System.Data;
using System.Web.UI.WebControls;
namespace EcShop.UI.Web.Admin.product
{
    public class AdjustBrand : AdminPage
	{
        private string productIds = string.Empty;
        protected Grid grdSelectedProducts;
        protected System.Web.UI.WebControls.Button btnSaveInfo;
        protected BrandCategoriesDropDownList dropBrandCategoriesAll;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.productIds = this.Page.Request.QueryString["productIds"];
			this.btnSaveInfo.Click += new System.EventHandler(this.btnSaveInfo_Click);
            this.grdSelectedProducts.RowDataBound += new System.Web.UI.WebControls.GridViewRowEventHandler(this.grdSelectedProducts_RowDataBound);
			if (!this.Page.IsPostBack)
			{
				this.BindProduct();
                dropBrandCategoriesAll.DataBind();
			}
		}

		private void btnSaveInfo_Click(object sender, System.EventArgs e)
		{
			System.Data.DataTable dataTable = new System.Data.DataTable();
			dataTable.Columns.Add("ProductId");
            dataTable.Columns.Add("BrandId");
			if (this.grdSelectedProducts.Rows.Count > 0)
			{
				foreach (System.Web.UI.WebControls.GridViewRow gridViewRow in this.grdSelectedProducts.Rows)
				{
                    int? brandId = 0;
                    BrandCategoriesDropDownList brand = gridViewRow.FindControl("dropBrandCategories") as BrandCategoriesDropDownList;
                    if (brand != null)
                    {
                        if (brand.SelectedValue <= 0)
                        {
                            break;
                        }
                        brandId = brand.SelectedValue;
                    }

                    int num2 = (int)this.grdSelectedProducts.DataKeys[gridViewRow.RowIndex].Value;
                    System.Data.DataRow dataRow = dataTable.NewRow();
                    dataRow["ProductId"] = num2;
                    dataRow["BrandId"] = brandId;
                    dataTable.Rows.Add(dataRow);
				}
				if (ProductHelper.UpdateProductBrand(dataTable))
				{
					this.CloseWindow();
				}
				else
				{
					this.ShowMsg("批量修改商品品牌失败", false);
				}
				this.BindProduct();
			}
		}
		private void BindProduct()
		{
			string value = this.Page.Request.QueryString["ProductIds"];
			if (!string.IsNullOrEmpty(value))
			{
				this.grdSelectedProducts.DataSource = ProductHelper.GetProductBaseInfo(value);
				this.grdSelectedProducts.DataBind();
			}
		}

        private void grdSelectedProducts_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            if (e.Row.RowType == System.Web.UI.WebControls.DataControlRowType.DataRow)
            {
                int brandId = !string.IsNullOrEmpty(System.Web.UI.DataBinder.Eval(e.Row.DataItem, "BrandId").ToString()) ? int.Parse(System.Web.UI.DataBinder.Eval(e.Row.DataItem, "BrandId").ToString()) : 0;
                BrandCategoriesDropDownList dropBrand = (BrandCategoriesDropDownList)e.Row.FindControl("dropBrandCategories");
                if (dropBrand != null)
                {
                    dropBrand.SelectedValue = brandId;
                }
                
            }
        }
	}
}

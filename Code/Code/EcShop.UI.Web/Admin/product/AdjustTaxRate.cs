using ASPNET.WebControls;
using EcShop.ControlPanel.Commodities;
using EcShop.ControlPanel.Store;
using EcShop.Core;
using EcShop.Entities.Store;
using EcShop.UI.ControlPanel.Utility;
using System;
using System.Data;
using System.Web.UI.WebControls;
namespace EcShop.UI.Web.Admin.product
{

    public class AdjustTaxRate : AdminPage
	{
        private string productIds = string.Empty;
        protected Grid grdSelectedProducts;
        protected System.Web.UI.WebControls.Button btnSaveInfo;
        protected TaxRateDropDownList dropTaxRateAll;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.productIds = this.Page.Request.QueryString["productIds"];
			this.btnSaveInfo.Click += new System.EventHandler(this.btnSaveInfo_Click);
            this.grdSelectedProducts.RowDataBound += new System.Web.UI.WebControls.GridViewRowEventHandler(this.grdSelectedProducts_RowDataBound);
			if (!this.Page.IsPostBack)
			{
				this.BindProduct();
                dropTaxRateAll.DataBind();
			}
		}
		
		private void btnSaveInfo_Click(object sender, System.EventArgs e)
		{
			System.Data.DataTable dataTable = new System.Data.DataTable();
			dataTable.Columns.Add("ProductId");
            dataTable.Columns.Add("TaxRateId");
			if (this.grdSelectedProducts.Rows.Count > 0)
			{
				foreach (System.Web.UI.WebControls.GridViewRow gridViewRow in this.grdSelectedProducts.Rows)
				{
                    int? taxRateId = 0;
                    TaxRateDropDownList dropTaxRate = gridViewRow.FindControl("dropTaxRate") as TaxRateDropDownList;
                    if (dropTaxRate != null)
                    {
                        if (dropTaxRate.SelectedValue <= 0)
                        {
                            break;
                        }
                        taxRateId = dropTaxRate.SelectedValue;
                    }

					int num2 = (int)this.grdSelectedProducts.DataKeys[gridViewRow.RowIndex].Value;
					System.Data.DataRow dataRow = dataTable.NewRow();
					dataRow["ProductId"] = num2;
                    dataRow["TaxRateId"] = taxRateId;
					dataTable.Rows.Add(dataRow);
				}
                if (ProductHelper.UpdateProductTaxRate(dataTable))
				{
					this.CloseWindow();
				}
				else
				{
					this.ShowMsg("批量修改商品税率失败", false);
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
                int taxtId = !string.IsNullOrEmpty(System.Web.UI.DataBinder.Eval(e.Row.DataItem, "TaxRateId").ToString()) ? int.Parse(System.Web.UI.DataBinder.Eval(e.Row.DataItem, "TaxRateId").ToString()) : 0;
                TaxRateDropDownList dropTaxRate = (TaxRateDropDownList)e.Row.FindControl("dropTaxRate");
                if (dropTaxRate != null )
                {
                    dropTaxRate.SelectedValue = taxtId;
                }
                
            }
        }
	}
}

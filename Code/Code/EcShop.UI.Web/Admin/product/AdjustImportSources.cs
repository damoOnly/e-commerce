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
    public class AdjustImportSources : AdminPage
	{
		private string productIds = string.Empty;
		protected Grid grdSelectedProducts;
		protected System.Web.UI.WebControls.Button btnSaveInfo;
        protected ImportSourceTypeDropDownList ddlImportSourceTypeAll;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.productIds = this.Page.Request.QueryString["productIds"];
			this.btnSaveInfo.Click += new System.EventHandler(this.btnSaveInfo_Click);
            this.grdSelectedProducts.RowDataBound += new System.Web.UI.WebControls.GridViewRowEventHandler(this.grdSelectedProducts_RowDataBound);
			if (!this.Page.IsPostBack)
			{
				this.BindProduct();
                ddlImportSourceTypeAll.DataBind();
			}
		}


		private void btnSaveInfo_Click(object sender, System.EventArgs e)
		{
			System.Data.DataTable dataTable = new System.Data.DataTable();
			dataTable.Columns.Add("ProductId");
            dataTable.Columns.Add("ImportSourceId");
			if (this.grdSelectedProducts.Rows.Count > 0)
			{
				foreach (System.Web.UI.WebControls.GridViewRow gridViewRow in this.grdSelectedProducts.Rows)
				{
                    int? importSourceid = 0;
                    ImportSourceTypeDropDownList importSource = gridViewRow.FindControl("ddlImportSourceType") as ImportSourceTypeDropDownList;
                    if (importSource != null)
                    {
                        if (importSource.SelectedValue <= 0)
                        {
                            break;
                        }
                        importSourceid = importSource.SelectedValue;
                    }

                    int num2 = (int)this.grdSelectedProducts.DataKeys[gridViewRow.RowIndex].Value;
                    System.Data.DataRow dataRow = dataTable.NewRow();
                    dataRow["ProductId"] = num2;
                    dataRow["ImportSourceId"] = importSourceid;
                    dataTable.Rows.Add(dataRow);
				}
                if (ProductHelper.UpdateProductImportSource(dataTable))
                {
                    this.CloseWindow();
                }
                else
                {
                    this.ShowMsg("批量修改商品原产地失败", false);
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
                int importSourceId = !string.IsNullOrEmpty(System.Web.UI.DataBinder.Eval(e.Row.DataItem, "ImportSourceId").ToString()) ? int.Parse(System.Web.UI.DataBinder.Eval(e.Row.DataItem, "ImportSourceId").ToString()) : 0;
                ImportSourceTypeDropDownList importSource = (ImportSourceTypeDropDownList)e.Row.FindControl("ddlImportSourceType");
                if (importSource != null)
                {
                    importSource.SelectedValue = importSourceId;
                }
                
            }
        }
	}
}

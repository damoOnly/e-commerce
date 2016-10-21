using ASPNET.WebControls;
using EcShop.ControlPanel.Commodities;
using EcShop.ControlPanel.Store;
using EcShop.Entities.Store;
using EcShop.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
namespace EcShop.UI.Web.Admin.product
{
    [PrivilegeCheck(Privilege.ProductFractionChange)]
    public class ProductFractionChange : AdminPage
	{
		private string productIds = string.Empty;
		protected System.Web.UI.WebControls.TextBox txtTagetStock;
		protected System.Web.UI.WebControls.Button btnTargetOK;
		protected System.Web.UI.WebControls.TextBox txtAddStock;
		protected System.Web.UI.WebControls.Button btnOperationOK;
		protected Grid grdSelectedProducts;
		protected System.Web.UI.WebControls.Button btnSaveStock;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.productIds = this.Page.Request.QueryString["productIds"];
			this.btnSaveStock.Click += new System.EventHandler(this.btnSaveStock_Click);
			this.btnTargetOK.Click += new System.EventHandler(this.btnTargetOK_Click);
			this.btnOperationOK.Click += new System.EventHandler(this.btnOperationOK_Click);
			if (!this.Page.IsPostBack)
			{
				this.BindProduct();
			}
		}
		private void btnOperationOK_Click(object sender, System.EventArgs e)
		{
			if (string.IsNullOrEmpty(this.productIds))
			{
				this.ShowMsg("没有要修改的商品", false);
				return;
			}
			decimal AdminFraction = 0;
            if (!decimal.TryParse(this.txtAddStock.Text, out AdminFraction))
			{
				this.ShowMsg("请输入正确的权重格式", false);
				return;
			}
            if (ProductHelper.AddProductsAdminFraction(this.productIds, AdminFraction))
			{
				this.BindProduct();
                this.ShowMsg("修改商品的权重成功", true);
				return;
			}
            this.ShowMsg("修改商品的权重失败", false);
		}
		private void btnTargetOK_Click(object sender, System.EventArgs e)
		{
			if (string.IsNullOrEmpty(this.productIds))
			{
				this.ShowMsg("没有要修改的商品", false);
				return;
			}
			decimal num = 0;
            if (!decimal.TryParse(this.txtTagetStock.Text, out num))
			{
                this.ShowMsg("请输入正确的权重格式", false);
				return;
			}
            //if (num < 0)
            //{
            //    this.ShowMsg("商品库存不能小于0", false);
            //    return;
            //}
			if (ProductHelper.UpdataProductsAdminFraction(this.productIds, num))
			{
				this.BindProduct();
				this.ShowMsg("修改商品的权重成功", true);
				return;
			}
            this.ShowMsg("修改商品的权重失败", true);
		}
		private void btnSaveStock_Click(object sender, System.EventArgs e)
		{
			System.Collections.Generic.Dictionary<string, decimal> dictionary = null;
			if (this.grdSelectedProducts.Rows.Count > 0)
			{
                dictionary = new System.Collections.Generic.Dictionary<string, decimal>();
				foreach (System.Web.UI.WebControls.GridViewRow gridViewRow in this.grdSelectedProducts.Rows)
				{
                    decimal value = 0m;
                    //获取修改的权重
                    System.Web.UI.WebControls.TextBox textBox = gridViewRow.FindControl("txtAdminFraction") as System.Web.UI.WebControls.TextBox;
					if (decimal.TryParse(textBox.Text, out value))
					{
                        string key = this.grdSelectedProducts.DataKeys[gridViewRow.RowIndex].Value == null ? "" : this.grdSelectedProducts.DataKeys[gridViewRow.RowIndex].Value.ToString();
						dictionary.Add(key, value);
					}
				}
				if (dictionary.Count > 0)
                {
                    if (ProductHelper.UpdateProductFraction(dictionary))
                    {
                        this.CloseWindow();
                    }
                    else
                    {
                        this.ShowMsg("批量修改库存失败", false);
                    }
				}
				this.BindProduct();
			}
		}
		private void BindProduct()
		{
			string value = this.Page.Request.QueryString["ProductIds"];
			if (!string.IsNullOrEmpty(value))
			{
                this.grdSelectedProducts.DataSource = ProductHelper.GetProductsFractionChange(value);
				this.grdSelectedProducts.DataBind();
			}
		}
	}
}

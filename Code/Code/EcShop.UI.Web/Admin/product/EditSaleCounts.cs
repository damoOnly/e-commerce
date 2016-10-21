using ASPNET.WebControls;
using EcShop.ControlPanel.Commodities;
using EcShop.ControlPanel.Store;
using EcShop.Entities.Store;
using EcShop.UI.Common.Controls;
using EcShop.UI.ControlPanel.Utility;
using System;
using System.Data;
using System.Web.UI.WebControls;
namespace EcShop.UI.Web.Admin.product
{
	[PrivilegeCheck(Privilege.EditProducts)]
	public class EditSaleCounts : AdminPage
	{
		private string productIds = string.Empty;
		protected System.Web.UI.WebControls.TextBox txtSaleCounts;
		protected System.Web.UI.WebControls.Button btnAddOK;
		protected OperationDropDownList ddlOperation;
		protected System.Web.UI.WebControls.TextBox txtOperationSaleCounts;
		protected System.Web.UI.WebControls.Button btnOperationOK;
		protected Grid grdSelectedProducts;
		protected System.Web.UI.WebControls.Button btnSaveInfo;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.productIds = this.Page.Request.QueryString["productIds"];
			this.btnSaveInfo.Click += new System.EventHandler(this.btnSaveInfo_Click);
			this.btnAddOK.Click += new System.EventHandler(this.btnAddOK_Click);
			this.btnOperationOK.Click += new System.EventHandler(this.btnOperationOK_Click);
			if (!this.Page.IsPostBack)
			{
				this.ddlOperation.DataBind();
				this.ddlOperation.SelectedValue = "+";
				this.BindProduct();
			}
		}
		private void btnAddOK_Click(object sender, System.EventArgs e)
		{
			int num = 0;
			if (!int.TryParse(this.txtSaleCounts.Text.Trim(), out num) || num < 0)
			{
				this.ShowMsg("销售数量只能是正整数，请输入正确的销售数量", false);
				return;
			}
			if (ProductHelper.UpdateShowSaleCounts(this.productIds, num))
			{
				this.ShowMsg("成功调整了前台显示的销售数量", true);
			}
			else
			{
				this.ShowMsg("调整前台显示的销售数量失败", false);
			}
			this.BindProduct();
		}
		private void btnOperationOK_Click(object sender, System.EventArgs e)
		{
			int num = 0;
			if (!int.TryParse(this.txtOperationSaleCounts.Text.Trim(), out num) || num < 0)
			{
				this.ShowMsg("销售数量只能是正整数，请输入正确的销售数量", false);
				return;
			}
			if (ProductHelper.UpdateShowSaleCounts(this.productIds, num, this.ddlOperation.SelectedValue))
			{
				this.ShowMsg("成功调整了前台显示的销售数量", true);
			}
			else
			{
				this.ShowMsg("调整前台显示的销售数量失败", false);
			}
			this.BindProduct();
		}
		private void btnSaveInfo_Click(object sender, System.EventArgs e)
		{
			System.Data.DataTable dataTable = new System.Data.DataTable();
			dataTable.Columns.Add("ProductId");
			dataTable.Columns.Add("ShowSaleCounts");
			if (this.grdSelectedProducts.Rows.Count > 0)
			{
				int num = 0;
				foreach (System.Web.UI.WebControls.GridViewRow gridViewRow in this.grdSelectedProducts.Rows)
				{
					int num2 = (int)this.grdSelectedProducts.DataKeys[gridViewRow.RowIndex].Value;
					System.Web.UI.WebControls.TextBox textBox = gridViewRow.FindControl("txtShowSaleCounts") as System.Web.UI.WebControls.TextBox;
					if (int.TryParse(textBox.Text.Trim(), out num) && num >= 0)
					{
						System.Data.DataRow dataRow = dataTable.NewRow();
						dataRow["ProductId"] = num2;
						dataRow["ShowSaleCounts"] = num;
						dataTable.Rows.Add(dataRow);
					}
				}
				if (ProductHelper.UpdateShowSaleCounts(dataTable))
				{
					this.ShowMsg("成功调整了前台显示的销售数量", true);
				}
				else
				{
					this.ShowMsg("调整前台显示的销售数量失败", false);
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
	}
}

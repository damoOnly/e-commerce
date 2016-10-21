using ASPNET.WebControls;
using EcShop.Core.Entities;
using EcShop.Core.Enums;
using EcShop.Entities.Commodities;
using EcShop.SaleSystem.Catalog;
using EcShop.UI.Common.Controls;
using System;
using System.Web.UI;
namespace EcShop.UI.SaleSystem.CodeBehind
{
	[System.Web.UI.ParseChildren(true)]
	public class CountDownProducts : HtmlTemplatedWebControl
	{
		private ThemedTemplatedRepeater rptProduct;
		private Pager pager;
		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
                this.SkinName = "Skin-CountDownProducts.html";
			}
			base.OnInit(e);
		}
		protected override void AttachChildControls()
		{
			this.rptProduct = (ThemedTemplatedRepeater)this.FindControl("rptProduct");
			this.pager = (Pager)this.FindControl("pager");
			if (!this.Page.IsPostBack)
			{
				this.BindProduct();
			}
		}
		private void BindProduct()
		{
			ProductBrowseQuery productBrowseQuery = this.GetProductBrowseQuery();
			DbQueryResult countDownProductList = ProductBrowser.GetCountDownProductList(productBrowseQuery);
			this.rptProduct.DataSource = countDownProductList.Data;
			this.rptProduct.DataBind();
			this.pager.TotalRecords = countDownProductList.TotalRecords;
		}
		private ProductBrowseQuery GetProductBrowseQuery()
		{
			return new ProductBrowseQuery
			{
				IsCount = true,
				PageIndex = this.pager.PageIndex,
				PageSize = this.pager.PageSize,
                SortBy = "StartDate",
				SortOrder = SortAction.Asc
			};
		}
	}
}

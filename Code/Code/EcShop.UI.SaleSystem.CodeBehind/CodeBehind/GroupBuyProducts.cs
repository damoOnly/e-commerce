using ASPNET.WebControls;
using EcShop.Core.Enums;
using EcShop.Entities.Commodities;
using EcShop.SaleSystem.Catalog;
using EcShop.UI.Common.Controls;
using System;
using System.Data;
using System.Web.UI;
namespace EcShop.UI.SaleSystem.CodeBehind
{
	[System.Web.UI.ParseChildren(true)]
	public class GroupBuyProducts : HtmlTemplatedWebControl
	{
		private ThemedTemplatedRepeater rptProduct;
		private Pager pager;
		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-GroupBuyProducts.html";
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
			int totalRecords;
			DataSet groupByProductList = ProductBrowser.GetGroupByProductList(productBrowseQuery, out totalRecords);
			this.rptProduct.DataSource = groupByProductList;
			this.rptProduct.DataBind();
			this.pager.TotalRecords = totalRecords;
		}
		private ProductBrowseQuery GetProductBrowseQuery()
		{
			return new ProductBrowseQuery
			{
				PageIndex = this.pager.PageIndex,
				PageSize = this.pager.PageSize,
				SortBy = "DisplaySequence",
				SortOrder = SortAction.Desc
			};
		}
	}
}

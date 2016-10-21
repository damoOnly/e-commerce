using ASPNET.WebControls;
using EcShop.ControlPanel.Commodities;
using EcShop.ControlPanel.Store;
using EcShop.Core.Entities;
using EcShop.Core.Enums;
using EcShop.Entities.Commodities;
using EcShop.Entities.Store;
using EcShop.Membership.Context;
using EcShop.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Web;
using System.Web.UI.WebControls;
namespace EcShop.UI.Web.Admin
{
    public class SupplierEditReleteProducts : AdminPage
	{
		private string keywords;
		private int? categoryId;
		private int productId;
		protected System.Web.UI.WebControls.Panel Panel1;
		protected ProductCategoriesDropDownList dropCategories;
		protected System.Web.UI.WebControls.TextBox txtSearchText;
		protected System.Web.UI.WebControls.Button btnSearch;
		protected System.Web.UI.WebControls.DataList dlstProducts;
		protected System.Web.UI.WebControls.Button btnAddSearch;
		protected Pager pager;
		protected System.Web.UI.WebControls.Button btnClear;
		protected System.Web.UI.WebControls.DataList dlstSearchProducts;
		protected Pager pagerSubject;
		protected override void OnInitComplete(System.EventArgs e)
		{
			base.OnInitComplete(e);
			this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
			this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
			this.dlstProducts.ItemCommand += new System.Web.UI.WebControls.DataListCommandEventHandler(this.dlstProducts_ItemCommand);
			this.dlstSearchProducts.DeleteCommand += new System.Web.UI.WebControls.DataListCommandEventHandler(this.dlstSearchProducts_DeleteCommand);
			this.btnAddSearch.Click += new System.EventHandler(this.btnAddSearch_Click);
		}
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.LoadParameters();
			if (!this.Page.IsPostBack)
			{
				this.dropCategories.DataBind();
				this.BindProducts();
				this.BindRelatedProducts();
			}
		}
		private void btnSearch_Click(object sender, System.EventArgs e)
		{
			this.ReBindPage(true);
		}
		private void btnClear_Click(object sender, System.EventArgs e)
		{
			ProductHelper.ClearRelatedProducts(this.productId);
			base.Response.Redirect(System.Web.HttpContext.Current.Request.Url.ToString(), true);
		}
		private void dlstProducts_ItemCommand(object source, System.Web.UI.WebControls.DataListCommandEventArgs e)
		{
			if (e.CommandName == "check")
			{
				int num = int.Parse(this.dlstProducts.DataKeys[e.Item.ItemIndex].ToString(), System.Globalization.NumberStyles.None);
				if (this.productId != num)
				{
					ProductHelper.AddRelatedProduct(this.productId, num);
				}
				base.Response.Redirect(System.Web.HttpContext.Current.Request.Url.ToString(), true);
			}
		}
		private void dlstSearchProducts_DeleteCommand(object source, System.Web.UI.WebControls.DataListCommandEventArgs e)
		{
			int relatedProductId = int.Parse(this.dlstSearchProducts.DataKeys[e.Item.ItemIndex].ToString(), System.Globalization.NumberStyles.None);
			ProductHelper.RemoveRelatedProduct(this.productId, relatedProductId);
			base.Response.Redirect(System.Web.HttpContext.Current.Request.Url.ToString(), true);
		}
		private void btnAddSearch_Click(object sender, System.EventArgs e)
		{
			System.Collections.Generic.IList<int> productIds = ProductHelper.GetProductIds(new ProductQuery
			{
				Keywords = this.keywords,
				CategoryId = this.categoryId,
				SaleStatus = ProductSaleStatus.OnSale
			});
			foreach (int current in productIds)
			{
				if (this.productId != current)
				{
					ProductHelper.AddRelatedProduct(this.productId, current);
				}
			}
			base.Response.Redirect(System.Web.HttpContext.Current.Request.Url.ToString(), true);
		}
		private void ReBindPage(bool isSearch)
		{
			System.Collections.Specialized.NameValueCollection nameValueCollection = new System.Collections.Specialized.NameValueCollection();
			nameValueCollection.Add("productId", this.productId.ToString());
			nameValueCollection.Add("Keywords", this.txtSearchText.Text.Trim());
			nameValueCollection.Add("CategoryId", this.dropCategories.SelectedValue.ToString());
			if (!isSearch)
			{
				nameValueCollection.Add("pageIndex", this.pager.PageIndex.ToString());
			}
			nameValueCollection.Add("pageIndex1", this.pagerSubject.PageIndex.ToString());
			base.ReloadPage(nameValueCollection);
		}
		private void LoadParameters()
		{
			int.TryParse(base.Request.QueryString["productId"], out this.productId);
			if (!string.IsNullOrEmpty(base.Request.QueryString["Keywords"]))
			{
				this.keywords = base.Request.QueryString["Keywords"];
			}
			if (!string.IsNullOrEmpty(base.Request.QueryString["CategoryId"]))
			{
				int value = 0;
				if (int.TryParse(base.Request.QueryString["CategoryId"], out value))
				{
					this.categoryId = new int?(value);
				}
			}
		}
		private void BindProducts()
		{
			ProductQuery productQuery = new ProductQuery();
			productQuery.Keywords = this.keywords;
			productQuery.CategoryId = this.categoryId;
			if (this.categoryId.HasValue)
			{
				productQuery.MaiCategoryPath = CatalogHelper.GetCategory(this.categoryId.Value).Path;
			}
			productQuery.PageSize = 10;
			productQuery.PageIndex = this.pager.PageIndex;
			productQuery.SaleStatus = ProductSaleStatus.OnSale;
			productQuery.SortOrder = SortAction.Desc;
			productQuery.SortBy = "DisplaySequence";
            productQuery.SupplierId = HiContext.Current.User.UserId;
			DbQueryResult products = ProductHelper.GetProducts(productQuery);
			this.dlstProducts.DataSource = products.Data;
			this.dlstProducts.DataBind();
			this.pager.TotalRecords = products.TotalRecords;
		}
		private void BindRelatedProducts()
		{
            ProductQuery productQuery = new ProductQuery();
            productQuery.PageSize = 10;
            productQuery.PageIndex = this.pagerSubject.PageIndex;
            productQuery.SortOrder = SortAction.Desc;
            productQuery.SortBy = "DisplaySequence";
            productQuery.ProductId = this.productId;
            DbQueryResult relatedProducts = ProductHelper.GetRelatedProducts(productQuery);
			this.dlstSearchProducts.DataSource = relatedProducts.Data;
			this.dlstSearchProducts.DataBind();
			this.pagerSubject.TotalRecords = relatedProducts.TotalRecords;
		}
	}
}

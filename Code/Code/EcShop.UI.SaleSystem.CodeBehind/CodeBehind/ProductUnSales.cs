using ASPNET.WebControls;
using EcShop.Core;
using EcShop.Core.Entities;
using EcShop.Core.Enums;
using EcShop.Entities.Commodities;
using EcShop.SaleSystem.Catalog;
using EcShop.UI.Common.Controls;
using EcShop.UI.SaleSystem.Tags;
using System;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace EcShop.UI.SaleSystem.CodeBehind
{
	[System.Web.UI.ParseChildren(true)]
	public class ProductUnSales : HtmlTemplatedWebControl
	{
		private ThemedTemplatedRepeater rptProducts;
		private System.Web.UI.WebControls.Literal litSearchResultPage;
		private Pager pager;
		private Common_CutdownSearch cutdownSearch;
		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-ProductUnSales.html";
			}
			base.OnInit(e);
		}
		protected override void AttachChildControls()
		{
			this.rptProducts = (ThemedTemplatedRepeater)this.FindControl("rptProducts");
			this.pager = (Pager)this.FindControl("pager");
			this.litSearchResultPage = (System.Web.UI.WebControls.Literal)this.FindControl("litSearchResultPage");
			this.cutdownSearch = (Common_CutdownSearch)this.FindControl("search_Common_CutdownSearch");
			this.cutdownSearch.ReSearch += new Common_CutdownSearch.ReSearchEventHandler(this.cutdownSearch_ReSearch);
			if (!this.Page.IsPostBack)
			{
				string title = "商品下架区";
				PageTitle.AddSiteNameTitle(title);
				this.BindProducts();
			}
		}
		private void cutdownSearch_ReSearch(object sender, System.EventArgs e)
		{
			this.ReLoadSearch();
		}
		public void ReLoadSearch()
		{
			base.ReloadPage(new System.Collections.Specialized.NameValueCollection
			{

				{
					"keywords",
					Globals.UrlEncode(this.cutdownSearch.Item.Keywords)
				},

				{
					"tagIds",
					Globals.UrlEncode(this.cutdownSearch.Item.TagIds)
				},

				{
					"minSalePrice",
					Globals.UrlEncode(this.cutdownSearch.Item.MinSalePrice.ToString())
				},

				{
					"maxSalePrice",
					Globals.UrlEncode(this.cutdownSearch.Item.MaxSalePrice.ToString())
				},

				{
					"pageIndex",
					"1"
				}
			});
		}
		protected void BindProducts()
		{
			ProductBrowseQuery productBrowseQuery = this.GetProductBrowseQuery();
			DbQueryResult unSaleProductList = ProductBrowser.GetUnSaleProductList(productBrowseQuery);
			this.rptProducts.DataSource = unSaleProductList.Data;
			this.rptProducts.DataBind();
			int totalRecords = unSaleProductList.TotalRecords;
			this.pager.TotalRecords = totalRecords;
			int num;
			if (totalRecords % this.pager.PageSize > 0)
			{
				num = totalRecords / this.pager.PageSize + 1;
			}
			else
			{
				num = totalRecords / this.pager.PageSize;
			}
			this.litSearchResultPage.Text = string.Format("总共有{0}件商品,{1}件商品为一页,共{2}页第 {3}页", new object[]
			{
				totalRecords,
				this.pager.PageSize,
				num,
				this.pager.PageIndex
			});
		}
		protected ProductBrowseQuery GetProductBrowseQuery()
		{
			ProductBrowseQuery productBrowseQuery = new ProductBrowseQuery();
			productBrowseQuery.PageIndex = this.pager.PageIndex;
			productBrowseQuery.PageSize = this.pager.PageSize;
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["keywords"]))
			{
				productBrowseQuery.Keywords = Globals.UrlDecode(this.Page.Request.QueryString["keywords"]);
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["minSalePrice"]))
			{
				decimal value = 0m;
				if (decimal.TryParse(Globals.UrlDecode(this.Page.Request.QueryString["minSalePrice"]), out value))
				{
					productBrowseQuery.MinSalePrice = new decimal?(value);
				}
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["maxSalePrice"]))
			{
				decimal value2 = 0m;
				if (decimal.TryParse(Globals.UrlDecode(this.Page.Request.QueryString["maxSalePrice"]), out value2))
				{
					productBrowseQuery.MaxSalePrice = new decimal?(value2);
				}
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["sortOrderBy"]))
			{
				productBrowseQuery.SortBy = this.Page.Request.QueryString["sortOrderBy"];
			}
			else
			{
				productBrowseQuery.SortBy = "DisplaySequence";
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["TagIds"]))
			{
				productBrowseQuery.TagIds = this.Page.Request.QueryString["TagIds"].Replace("_", ",");
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["sortOrder"]))
			{
				productBrowseQuery.SortOrder = (SortAction)System.Enum.Parse(typeof(SortAction), this.Page.Request.QueryString["sortOrder"]);
			}
			else
			{
				productBrowseQuery.SortOrder = SortAction.Desc;
			}
			Globals.EntityCoding(productBrowseQuery, true);
			return productBrowseQuery;
		}
	}
}

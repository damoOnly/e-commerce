using ASPNET.WebControls;
using EcShop.ControlPanel.Commodities;
using EcShop.Core;
using EcShop.Core.Entities;
using EcShop.Core.Enums;
using EcShop.Entities.Commodities;
using EcShop.UI.Common.Controls;
using EcShop.UI.ControlPanel.Utility;
using System;
using System.Collections.Specialized;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace EcShop.UI.Web.Admin
{
	public class EditeShareDetails : AdminPage
	{
		private int shareId;
		private int pagesize = 10;
		private int pageindex = 1;
		private string productName;
		private string productCode;
		private int? categoryId;
		private int? tagId;
		private int? typeId;
		private bool isAlert;
		protected System.Web.UI.WebControls.TextBox txtSearchText;
		protected ProductCategoriesDropDownList dropCategories;
		protected BrandCategoriesDropDownList dropBrandList;
		protected ProductTagsDropDownList dropTagList;
		protected ProductTypeDownList dropType;
		protected System.Web.UI.WebControls.CheckBox chkIsAlert;
		protected System.Web.UI.WebControls.Button btnSearch;
		protected PageSize hrefPageSize;
		protected Pager pager1;
		protected System.Web.UI.WebControls.LinkButton btnBatchDelete;
		protected System.Web.UI.WebControls.Repeater rp_shareproduct;
		protected Pager pager;
		protected System.Web.UI.WebControls.Button btnSharePage;
		protected System.Web.UI.HtmlControls.HtmlInputText txturl;
		protected HiImage imgurl;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
			this.btnBatchDelete.Click += new System.EventHandler(this.btnBatchDelete_Click);
			this.rp_shareproduct.ItemCommand += new System.Web.UI.WebControls.RepeaterCommandEventHandler(this.rp_shareproduct_ItemCommand);
			this.rp_shareproduct.ItemDataBound += new System.Web.UI.WebControls.RepeaterItemEventHandler(this.rp_shareproduct_ItemDataBound);
			int.TryParse(this.Page.Request.QueryString["shareId"], out this.shareId);
			if (!base.IsPostBack && !string.IsNullOrEmpty(this.Page.Request.QueryString["shareId"]))
			{
				this.dropCategories.DataBind();
				this.dropBrandList.DataBind();
				this.dropTagList.DataBind();
				this.dropType.DataBind();
				this.BindShareProducts();
			}
		}
		private void rp_shareproduct_ItemCommand(object source, System.Web.UI.WebControls.RepeaterCommandEventArgs e)
		{
			if (e.CommandName == "delete")
			{
				if (ProductHelper.DeleteShareProducts(this.shareId, e.CommandArgument.ToString()))
				{
					this.ShowMsg("删除成功！", true);
					this.BindShareProducts();
					return;
				}
				this.ShowMsg("删除失败！", false);
			}
		}
		private void btnBatchDelete_Click(object sender, System.EventArgs e)
		{
			string text = this.Page.Request.Form["CheckBoxGroup"];
			if (string.IsNullOrEmpty(text))
			{
				this.ShowMsg("请先选择要删除的商品", false);
				return;
			}
			if (ProductHelper.DeleteShareProducts(this.shareId, text))
			{
				this.ShowMsg("删除成功！", true);
				this.ReloadProductOnSales(false);
				return;
			}
			this.ShowMsg("删除失败！", false);
		}
		private void btnSearch_Click(object sender, System.EventArgs e)
		{
			this.ReloadProductOnSales(true);
		}
		private void LoadParams()
		{
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["productName"]))
			{
				this.productName = Globals.UrlDecode(this.Page.Request.QueryString["productName"]);
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["productCode"]))
			{
				this.productCode = Globals.UrlDecode(this.Page.Request.QueryString["productCode"]);
			}
			int value = 0;
			if (int.TryParse(this.Page.Request.QueryString["categoryId"], out value))
			{
				this.categoryId = new int?(value);
			}
			int value2 = 0;
			if (int.TryParse(this.Page.Request.QueryString["brandId"], out value2))
			{
				this.dropBrandList.SelectedValue = new int?(value2);
			}
			int value3 = 0;
			if (int.TryParse(this.Page.Request.QueryString["tagId"], out value3))
			{
				this.tagId = new int?(value3);
			}
			int value4 = 0;
			if (int.TryParse(this.Page.Request.QueryString["typeId"], out value4))
			{
				this.typeId = new int?(value4);
			}
			bool.TryParse(this.Page.Request.QueryString["isAlert"], out this.isAlert);
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["pageSize"]))
			{
				int.TryParse(this.Page.Request.QueryString["pageSize"], out this.pagesize);
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["pageIndex"]))
			{
				int.TryParse(this.Page.Request.QueryString["pageIndex"], out this.pageindex);
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["shareId"]))
			{
				int.TryParse(this.Page.Request.QueryString["shareId"], out this.shareId);
			}
			this.txtSearchText.Text = this.productName;
			this.dropCategories.SelectedValue = this.categoryId;
			this.dropBrandList.SelectedValue = new int?(value2);
			this.dropTagList.SelectedValue = this.tagId;
			this.dropType.SelectedValue = this.typeId;
			if (this.isAlert)
			{
				this.chkIsAlert.Checked = true;
			}
		}
		private void BindShareProducts()
		{
			this.LoadParams();
			ProductQuery productQuery = new ProductQuery
			{
				SaleStatus = ProductSaleStatus.OnSale,
				Keywords = this.productName,
				ProductCode = this.productCode,
				CategoryId = this.categoryId,
				PageSize = this.pager.PageSize,
				PageIndex = this.pager.PageIndex,
				SortOrder = SortAction.Desc,
				SortBy = "DisplaySequence",
				BrandId = this.dropBrandList.SelectedValue.HasValue ? this.dropBrandList.SelectedValue : null,
				TagId = this.dropTagList.SelectedValue.HasValue ? this.dropTagList.SelectedValue : null,
				TypeId = this.typeId,
				IsAlert = this.isAlert
			};
			if (this.categoryId.HasValue)
			{
				productQuery.MaiCategoryPath = CatalogHelper.GetCategory(this.categoryId.Value).Path;
			}
			Globals.EntityCoding(productQuery, true);
			ShareProductInfo shareProductInfoById = ProductHelper.GetShareProductInfoById(this.shareId);
			string value = Globals.HostPath(System.Web.HttpContext.Current.Request.Url) + "/WapShop/ShareProducts.aspx?Id=" + this.shareId;
			this.txturl.Value = value;
			this.imgurl.ImageUrl = shareProductInfoById.ShareUrl;
			DbQueryResult shareProducts = ProductHelper.GetShareProducts(productQuery, true, this.shareId);
			this.rp_shareproduct.DataSource = shareProducts.Data;
			this.rp_shareproduct.DataBind();
			this.pager1.TotalRecords = (this.pager.TotalRecords = shareProducts.TotalRecords);
		}
		private void rp_shareproduct_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
		{
			if (e.Item.ItemType == System.Web.UI.WebControls.ListItemType.Item || e.Item.ItemType == System.Web.UI.WebControls.ListItemType.AlternatingItem)
			{
				System.Web.UI.WebControls.Literal literal = (System.Web.UI.WebControls.Literal)e.Item.FindControl("litSaleStatus");
				if (literal.Text == "1")
				{
					literal.Text = "出售中";
					return;
				}
				if (literal.Text == "2")
				{
					literal.Text = "下架区";
					return;
				}
				literal.Text = "仓库中";
			}
		}
		private void ReloadProductOnSales(bool isSearch)
		{
			System.Collections.Specialized.NameValueCollection nameValueCollection = new System.Collections.Specialized.NameValueCollection();
			nameValueCollection.Add("shareId", this.shareId.ToString());
			nameValueCollection.Add("productName", Globals.UrlEncode(this.txtSearchText.Text.Trim()));
			if (this.dropCategories.SelectedValue.HasValue)
			{
				nameValueCollection.Add("categoryId", this.dropCategories.SelectedValue.ToString());
			}
			nameValueCollection.Add("pageSize", this.pager.PageSize.ToString());
			if (!isSearch)
			{
				nameValueCollection.Add("pageIndex", this.pager.PageIndex.ToString());
			}
			if (this.dropBrandList.SelectedValue.HasValue)
			{
				nameValueCollection.Add("brandId", this.dropBrandList.SelectedValue.ToString());
			}
			if (this.dropTagList.SelectedValue.HasValue)
			{
				nameValueCollection.Add("tagId", this.dropTagList.SelectedValue.ToString());
			}
			if (this.dropType.SelectedValue.HasValue)
			{
				nameValueCollection.Add("typeId", this.dropType.SelectedValue.ToString());
			}
			nameValueCollection.Add("isAlert", this.chkIsAlert.Checked.ToString());
			base.ReloadPage(nameValueCollection);
		}
	}
}

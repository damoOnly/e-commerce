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
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace EcShop.UI.Web.Admin
{
	public class AddShareProducts : AdminPage
	{
		private int shareId;
		private string productName;
		private string productCode;
		private int? categoryId;
		private int? lineId;
		private int? tagId;
		private int? typeId;
		private int? distributorId;
		private bool isAlert;
		private ProductSaleStatus saleStatus = ProductSaleStatus.OnSale;
		private PenetrationStatus penetrationStatus;
		private System.DateTime? startDate;
		private System.DateTime? endDate;
		protected System.Web.UI.WebControls.TextBox txtSearchText;
		protected ProductCategoriesDropDownList dropCategories;
		protected BrandCategoriesDropDownList dropBrandList;
		protected ProductTagsDropDownList dropTagList;
		protected ProductTypeDownList dropType;
		protected WebCalendar calendarStartDate;
		protected WebCalendar calendarEndDate;
		protected System.Web.UI.WebControls.CheckBox chkIsAlert;
		protected System.Web.UI.WebControls.Button btnSearch;
		protected PageSize hrefPageSize;
		protected Pager pager1;
		protected System.Web.UI.WebControls.LinkButton btnBtachAdd;
		protected System.Web.UI.WebControls.Repeater rp_shareproduct;
		protected Pager pager;
		protected System.Web.UI.HtmlControls.HtmlInputText txttitle;
		protected System.Web.UI.WebControls.Button btnSharePage;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
			this.btnBtachAdd.Click += new System.EventHandler(this.btnBtachAdd_Click);
			this.rp_shareproduct.ItemDataBound += new System.Web.UI.WebControls.RepeaterItemEventHandler(this.rp_shareproduct_ItemDataBound);
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["shareId"]))
			{
				int.TryParse(this.Page.Request.QueryString["shareId"], out this.shareId);
			}
			if (!this.Page.IsPostBack)
			{
				this.dropCategories.DataBind();
				this.dropBrandList.DataBind();
				this.dropTagList.DataBind();
				this.dropType.DataBind();
				this.BindProducts();
			}
		}
		private void rp_shareproduct_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
		{
			System.Web.UI.WebControls.Literal literal = (System.Web.UI.WebControls.Literal)e.Item.FindControl("litMakeState");
			if (literal.Text != "" && System.Convert.ToInt32(literal.Text) > 0)
			{
				literal.Text = "已分享";
				return;
			}
			literal.Text = "未分享";
		}
		private void btnBtachAdd_Click(object sender, System.EventArgs e)
		{
			string text = base.Request.Form["CheckBoxGroup"];
			if (string.IsNullOrEmpty(text))
			{
				this.ShowMsg("请先选择要批量添加的商品", false);
				return;
			}
			ProductHelper.AddShareProducts(this.shareId, text);
			this.CloseWindow();
		}
		private void btnSearch_Click(object sender, System.EventArgs e)
		{
			this.ReloadProductOnSales(true);
		}
		private void BindProducts()
		{
			this.LoadParameters();
			ProductQuery productQuery = new ProductQuery
			{
				Keywords = this.productName,
				ProductCode = this.productCode,
				CategoryId = this.categoryId,
				ProductLineId = this.lineId,
				PageSize = this.pager.PageSize,
				PageIndex = this.pager.PageIndex,
				SortOrder = SortAction.Desc,
				SortBy = "DisplaySequence",
				StartDate = this.startDate,
				BrandId = this.dropBrandList.SelectedValue.HasValue ? this.dropBrandList.SelectedValue : null,
				TagId = this.dropTagList.SelectedValue.HasValue ? this.dropTagList.SelectedValue : null,
				TypeId = this.typeId,
				UserId = this.distributorId,
				IsAlert = this.isAlert,
				SaleStatus = this.saleStatus,
				PenetrationStatus = this.penetrationStatus,
				EndDate = this.endDate
			};
			if (this.categoryId.HasValue)
			{
				productQuery.MaiCategoryPath = CatalogHelper.GetCategory(this.categoryId.Value).Path;
			}
			Globals.EntityCoding(productQuery, true);
			DbQueryResult shareProducts = ProductHelper.GetShareProducts(productQuery, false, this.shareId);
			this.rp_shareproduct.DataSource = shareProducts.Data;
			this.rp_shareproduct.DataBind();
			this.txtSearchText.Text = productQuery.Keywords;
			this.dropCategories.SelectedValue = productQuery.CategoryId;
			this.dropType.SelectedValue = productQuery.TypeId;
			this.chkIsAlert.Checked = productQuery.IsAlert;
			this.pager1.TotalRecords = (this.pager.TotalRecords = shareProducts.TotalRecords);
		}
		private void LoadParameters()
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
			if (int.TryParse(this.Page.Request.QueryString["lineId"], out value3))
			{
				this.lineId = new int?(value3);
			}
			int value4 = 0;
			if (int.TryParse(this.Page.Request.QueryString["tagId"], out value4))
			{
				this.tagId = new int?(value4);
			}
			int value5 = 0;
			if (int.TryParse(this.Page.Request.QueryString["typeId"], out value5))
			{
				this.typeId = new int?(value5);
			}
			int value6 = 0;
			if (int.TryParse(this.Page.Request.QueryString["distributorId"], out value6))
			{
				this.distributorId = new int?(value6);
			}
			bool.TryParse(this.Page.Request.QueryString["isAlert"], out this.isAlert);
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["SaleStatus"]))
			{
				this.saleStatus = (ProductSaleStatus)System.Enum.Parse(typeof(ProductSaleStatus), this.Page.Request.QueryString["SaleStatus"]);
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["PenetrationStatus"]))
			{
				this.penetrationStatus = (PenetrationStatus)System.Enum.Parse(typeof(PenetrationStatus), this.Page.Request.QueryString["PenetrationStatus"]);
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["startDate"]))
			{
				this.startDate = new System.DateTime?(System.DateTime.Parse(this.Page.Request.QueryString["startDate"]));
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["endDate"]))
			{
				this.endDate = new System.DateTime?(System.DateTime.Parse(this.Page.Request.QueryString["endDate"]));
			}
			this.txtSearchText.Text = this.productName;
			this.dropCategories.DataBind();
			this.dropCategories.SelectedValue = this.categoryId;
			this.dropTagList.DataBind();
			this.dropTagList.SelectedValue = this.tagId;
			this.calendarStartDate.SelectedDate = this.startDate;
			this.calendarEndDate.SelectedDate = this.endDate;
			this.dropType.SelectedValue = this.typeId;
			this.chkIsAlert.Checked = this.isAlert;
		}
		private void ReloadProductOnSales(bool isSearch)
		{
			System.Collections.Specialized.NameValueCollection nameValueCollection = new System.Collections.Specialized.NameValueCollection();
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
			if (this.calendarStartDate.SelectedDate.HasValue)
			{
				nameValueCollection.Add("startDate", this.calendarStartDate.SelectedDate.Value.ToString());
			}
			if (this.calendarEndDate.SelectedDate.HasValue)
			{
				nameValueCollection.Add("endDate", this.calendarEndDate.SelectedDate.Value.ToString());
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
			nameValueCollection.Add("shareId", this.Page.Request.QueryString["shareId"]);
			base.ReloadPage(nameValueCollection);
		}
	}
}

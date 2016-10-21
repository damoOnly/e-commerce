using ASPNET.WebControls;
using EcShop.ControlPanel.Commodities;
using EcShop.ControlPanel.Promotions;
using EcShop.ControlPanel.Store;
using EcShop.Core;
using EcShop.Core.Entities;
using EcShop.Entities.Commodities;
using EcShop.Entities.Store;
using EcShop.UI.Common.Controls;
using EcShop.UI.ControlPanel.Utility;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.WebControls;
namespace EcShop.UI.Web.Admin
{
    [PrivilegeCheck(Privilege.SearchPromotionProduct)]
	public class SearchPromotionProduct : AdminPage
	{
		private string productName;
		private int? categoryId;
		private int? brandId;
		private int activityId;
        private int? supplierId;
		protected System.Web.UI.WebControls.TextBox txtSearchText;
		protected ProductCategoriesDropDownList dropCategories;
		protected BrandCategoriesDropDownList dropBrandList;
        protected SupplierDropDownList dropSupplier;
		protected System.Web.UI.WebControls.Button btnSearch;
		protected PageSize hrefPageSize;
		protected Pager pager;
		protected ImageLinkButton btnAdd;
		protected Grid grdproducts;
		protected Pager pager1;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!int.TryParse(this.Page.Request.QueryString["activityId"], out this.activityId))
			{
				base.GotoResourceNotFound();
				return;
			}
			if (!base.IsPostBack)
			{
				this.DoCallback();
			}
			this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
			this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
			CheckBoxColumn.RegisterClientCheckEvents(this.Page, this.Page.Form.ClientID);
		}
		protected void btnSearch_Click(object sender, System.EventArgs e)
		{
			this.ReloadProductOnSales(true);
		}
		protected void btnAdd_Click(object sender, System.EventArgs e)
		{
			string text = base.Request.Form["CheckBoxGroup"];
			if (string.IsNullOrEmpty(text))
			{
				this.ShowMsg("请选择一件商品！", false);
				return;
			}
			if (PromoteHelper.AddPromotionProducts(this.activityId, text))
			{
				this.CloseWindow();
				return;
			}
			this.ShowMsg("选择的商品已经参加其他促销活动，不能添加到此促销活动中！", false);
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
			if (this.dropBrandList.SelectedValue.HasValue)
			{
				nameValueCollection.Add("brandId", this.dropBrandList.SelectedValue.ToString());
			}
            if (this.dropSupplier.SelectedValue.HasValue)
            {
                nameValueCollection.Add("supplierId", this.dropSupplier.SelectedValue.ToString());
            }
			nameValueCollection.Add("activityId", this.activityId.ToString());
			base.ReloadPage(nameValueCollection);
		}
		protected void DoCallback()
		{
			this.LoadParameters();
			ProductQuery productQuery = new ProductQuery();
			productQuery.PageSize = this.pager.PageSize;
			productQuery.PageIndex = this.pager.PageIndex;
			productQuery.SaleStatus = ProductSaleStatus.OnSale;
			productQuery.IsIncludePromotionProduct = new bool?(false);
			productQuery.IsIncludeBundlingProduct = new bool?(false);
			productQuery.Keywords = this.productName;

            //促销活动Id
            productQuery.ActivityId = this.activityId;

			if (this.brandId.HasValue)
			{
				productQuery.BrandId = new int?(this.brandId.Value);
			}
			productQuery.CategoryId = this.categoryId;
			if (this.categoryId.HasValue)
			{
				productQuery.MaiCategoryPath = CatalogHelper.GetCategory(this.categoryId.Value).Path;
			}
            if (this.supplierId.HasValue)
            {
                productQuery.SupplierId = this.supplierId;
            }
			DbQueryResult products = ProductHelper.GetProducts(productQuery);
			System.Data.DataTable dataSource = (System.Data.DataTable)products.Data;
			this.pager1.TotalRecords = (this.pager.TotalRecords = products.TotalRecords);
			this.grdproducts.DataSource = dataSource;
			this.grdproducts.DataBind();
		}
		private void LoadParameters()
		{
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["productName"]))
			{
				this.productName = Globals.UrlDecode(this.Page.Request.QueryString["productName"]);
			}
			int value = 0;
			if (int.TryParse(this.Page.Request.QueryString["categoryId"], out value))
			{
				this.categoryId = new int?(value);
			}
			int value2 = 0;
			if (int.TryParse(this.Page.Request.QueryString["brandId"], out value2))
			{
				this.brandId = new int?(value2);
			}
            int value3 = 0;
            if (int.TryParse(this.Page.Request.QueryString["supplierId"], out value3))
            {
                this.supplierId = new int?(value3);
            }
			this.txtSearchText.Text = this.productName;
			this.dropCategories.DataBind();
			this.dropCategories.SelectedValue = this.categoryId;
			this.dropBrandList.DataBind();
			this.dropBrandList.SelectedValue = new int?(value2);
            //绑定供货商
            this.dropSupplier.DataBind();
            this.dropSupplier.SelectedValue = new int?(value3);
		}
	}
}

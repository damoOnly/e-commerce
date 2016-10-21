using ASPNET.WebControls;
using EcShop.ControlPanel.Commodities;
using EcShop.ControlPanel.Promotions;
using EcShop.ControlPanel.Store;
using EcShop.Core;
using EcShop.Core.Entities;
using EcShop.Entities.Commodities;
using EcShop.Entities.Store;
using EcShop.Membership.Context;
using EcShop.UI.Common.Controls;
using EcShop.UI.ControlPanel.Utility;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.WebControls;
namespace EcShop.UI.Web.Admin.PO
{
    public class SearchSuppliearchProduct : AdminPage
    {
        private string productName;
        private string SkuId;
        private string Barcode;
        private int? categoryId;
        private int? brandId;
        private int POId;
        private int supplierId;
        protected System.Web.UI.WebControls.TextBox txtSearchText;
        protected System.Web.UI.WebControls.TextBox txtSkuId;
        protected System.Web.UI.WebControls.TextBox txtBarcode;
        protected ProductCategoriesDropDownList dropCategories;
        protected BrandCategoriesDropDownList dropBrandList;
        protected System.Web.UI.WebControls.Button btnSearch;
        protected PageSize hrefPageSize;
        protected Pager pager;
        protected ImageLinkButton btnAdd;
        protected Grid grdproducts;
        protected Pager pager1;
        protected void Page_Load(object sender, System.EventArgs e)
        {
            ManagerHelper.CheckPrivilege(Privilege.POAdd);
            if (!int.TryParse(this.Page.Request.QueryString["POId"], out this.POId))
            {
                base.GotoResourceNotFound();
                return;
            }
            int.TryParse(this.Page.Request.QueryString["supplierId"], out this.supplierId);
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
            var member = HiContext.Current.User;
            if (member == null || member.IsLockedOut)
            {
                this.ShowMsg("获取用户信息失败，请重新登录", false);
                return;
            }
            if (PurchaseOrderHelper.POAddProducts(this.POId, text, member.UserId))
            {
                this.CloseWindow();
                return;
            }
            else
            {
                this.ShowMsg("该单状态不允许操作或您无权操作该单或已存在该商品", false);
            }
        }
        private void ReloadProductOnSales(bool isSearch)
        {
            System.Collections.Specialized.NameValueCollection nameValueCollection = new System.Collections.Specialized.NameValueCollection();
            nameValueCollection.Add("productName", Globals.UrlEncode(this.txtSearchText.Text.Trim()));
            nameValueCollection.Add("SkuId", Globals.UrlEncode(this.txtSkuId.Text.Trim()));
            nameValueCollection.Add("Barcode", Globals.UrlEncode(this.txtBarcode.Text.Trim()));
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
            nameValueCollection.Add("supplierId", this.supplierId.ToString());
            nameValueCollection.Add("POId", this.POId.ToString());
            
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
            productQuery.SkuId = this.SkuId;
            productQuery.BarCode = this.Barcode;

            if (this.brandId.HasValue)
            {
                productQuery.BrandId = new int?(this.brandId.Value);
            }
            productQuery.CategoryId = this.categoryId;
            if (this.categoryId.HasValue)
            {
                productQuery.MaiCategoryPath = CatalogHelper.GetCategory(this.categoryId.Value).Path;
            }
            productQuery.SupplierId = this.supplierId;
            DbQueryResult products = ProductHelper.GetProducts(productQuery);
            System.Data.DataTable dataSource = (System.Data.DataTable)products.Data;
            this.pager1.TotalRecords = (this.pager.TotalRecords = products.TotalRecords);
            this.grdproducts.DataSource = dataSource;
            this.grdproducts.DataBind();
        }
        private void LoadParameters()
        {
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["SkuId"]))
            {
                this.SkuId = Globals.UrlDecode(this.Page.Request.QueryString["SkuId"]);
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["Barcode"]))
            {
                this.Barcode = Globals.UrlDecode(this.Page.Request.QueryString["Barcode"]);
            }
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
            this.txtSkuId.Text = this.SkuId;
            this.txtBarcode.Text = this.Barcode;
            this.txtSearchText.Text = this.productName;
            this.dropCategories.DataBind();
            this.dropCategories.SelectedValue = this.categoryId;
            this.dropBrandList.DataBind();
            this.dropBrandList.SelectedValue = new int?(value2);
        }
    }
}

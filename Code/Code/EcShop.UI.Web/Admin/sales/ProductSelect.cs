using ASPNET.WebControls;
using EcShop.ControlPanel.Commodities;
using EcShop.ControlPanel.Store;
using EcShop.Core;
using EcShop.Core.Entities;
using EcShop.Core.Enums;
using EcShop.Entities;
using EcShop.Entities.Commodities;
using EcShop.Entities.Store;
using EcShop.Membership.Context;
using EcShop.UI.Common.Controls;
using EcShop.UI.ControlPanel.Utility;
using EcShop.UI.SaleSystem.Tags;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Globalization;
using System.Web.UI.WebControls;

namespace EcShop.UI.Web.Admin.sales
{
    [PrivilegeCheck(Privilege.SkusSelect)]
    public class ProductSelect : AdminPage
    {
        private string productName;
        private string productCode;
        private int? categoryId;
        private int? typeId;
        private int? importSourceId;
        private int? supplierId;
        //状态为出售中
        private ProductSaleStatus saleStatus = ProductSaleStatus.OnSale;
        protected System.Web.UI.WebControls.TextBox txtSearchText;
        protected ProductCategoriesDropDownList dropCategories;
        protected BrandCategoriesDropDownList dropBrandList;
        protected ProductTypeDownList dropType;
        protected System.Web.UI.WebControls.TextBox txtSKU;
        protected System.Web.UI.WebControls.Button btnSearch;
        protected PageSize hrefPageSize;
        protected Pager pager1;
        protected ImportSourceTypeDropDownList ddlImportSourceType;
        protected SupplierDropDownList ddlSupplier;
        protected Grid grdProductSkus;
        protected Pager pager;

        protected void Page_Load(object sender, System.EventArgs e)
        {
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            this.grdProductSkus.RowDataBound += new System.Web.UI.WebControls.GridViewRowEventHandler(this.grdProductSkus_RowDataBound);
            if (!this.Page.IsPostBack)
            {
                this.dropCategories.DataBind();
                this.dropBrandList.DataBind();
                this.dropType.DataBind();
                this.ddlImportSourceType.DataBind();
                this.ddlSupplier.DataBind();
                this.BindProductSelect();
            }
            CheckBoxColumn.RegisterClientCheckEvents(this.Page, this.Page.Form.ClientID);
        }

        private void btnSearch_Click(object sender, System.EventArgs e)
        {
            this.ReloadProductOnSales(true);
        }

        private void grdProductSkus_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            if (e.Row.RowType == System.Web.UI.WebControls.DataControlRowType.DataRow)
            {
                System.Web.UI.WebControls.Literal literal2 = (System.Web.UI.WebControls.Literal)e.Row.FindControl("litMarketPrice");
                if (string.IsNullOrEmpty(literal2.Text))
                {
                    literal2.Text = "-";
                }
            }
        }

        private void BindProductSelect()
        {
            this.LoadParameters();
            ProductQuery productQuery = new ProductQuery
            {
                Keywords = this.productName,
                ProductCode = this.productCode,
                CategoryId = this.categoryId,
                PageSize = this.pager.PageSize,
                PageIndex = this.pager.PageIndex,
                SortOrder = SortAction.Desc,
                SortBy = "DisplaySequence",
                BrandId = this.dropBrandList.SelectedValue.HasValue ? this.dropBrandList.SelectedValue : null,
                TypeId = this.typeId,
                SaleStatus = this.saleStatus,
                ImportSourceId = this.ddlImportSourceType.SelectedValue.HasValue ? this.ddlImportSourceType.SelectedValue : null,
                SupplierId = this.ddlSupplier.SelectedValue.HasValue ? this.ddlSupplier.SelectedValue : null
            };
            if (this.categoryId.HasValue)
            {
                productQuery.MaiCategoryPath = CatalogHelper.GetCategory(this.categoryId.Value).Path;
            }
            Globals.EntityCoding(productQuery, true);
            DbQueryResult products = ProductHelper.GetProductSelect(productQuery);

            this.grdProductSkus.DataSource = products.Data;
            this.grdProductSkus.DataBind();
            this.txtSearchText.Text = productQuery.Keywords;
            this.txtSKU.Text = productQuery.ProductCode;
            this.dropCategories.SelectedValue = productQuery.CategoryId;
            this.dropType.SelectedValue = productQuery.TypeId;
            this.ddlImportSourceType.SelectedValue = productQuery.ImportSourceId;
            this.ddlSupplier.SelectedValue = productQuery.SupplierId;
            this.pager1.TotalRecords = (this.pager.TotalRecords = products.TotalRecords);
        }
        private void ReloadProductOnSales(bool isSearch)
        {
            System.Collections.Specialized.NameValueCollection nameValueCollection = new System.Collections.Specialized.NameValueCollection();
            nameValueCollection.Add("productName", Globals.UrlEncode(this.txtSearchText.Text.Trim()));
            if (this.dropCategories.SelectedValue.HasValue)
            {
                nameValueCollection.Add("categoryId", this.dropCategories.SelectedValue.ToString());
            }
            nameValueCollection.Add("productCode", Globals.UrlEncode(Globals.HtmlEncode(this.txtSKU.Text.Trim())));
            nameValueCollection.Add("pageSize", this.pager.PageSize.ToString());
            if (!isSearch)
            {
                nameValueCollection.Add("pageIndex", this.pager.PageIndex.ToString());
            }
            if (this.dropBrandList.SelectedValue.HasValue)
            {
                nameValueCollection.Add("brandId", this.dropBrandList.SelectedValue.ToString());
            }
            if (this.dropType.SelectedValue.HasValue)
            {
                nameValueCollection.Add("typeId", this.dropType.SelectedValue.ToString());
            }
            if (this.ddlImportSourceType.SelectedValue.HasValue)
            {
                nameValueCollection.Add("importSourceId", this.ddlImportSourceType.SelectedValue.ToString());
            }
            if (this.ddlSupplier.SelectedValue.HasValue)
            {
                nameValueCollection.Add("supplierId", this.ddlSupplier.SelectedValue.ToString());
            }
            base.ReloadPage(nameValueCollection);
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
            int value4 = 0;
            if (int.TryParse(this.Page.Request.QueryString["typeId"], out value4))
            {
                this.typeId = new int?(value4);
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["SaleStatus"]))
            {
                this.saleStatus = (ProductSaleStatus)System.Enum.Parse(typeof(ProductSaleStatus), this.Page.Request.QueryString["SaleStatus"]);
            }
            int iSourceId = 0;
            if (int.TryParse(this.Page.Request.QueryString["importSourceId"], out iSourceId))
            {
                this.importSourceId = new int?(iSourceId);
            }

            int iSupplier = 0;
            if (int.TryParse(this.Page.Request.QueryString["supplierId"], out iSupplier))
            {
                this.supplierId = new int?(iSupplier);
            }

            this.txtSearchText.Text = this.productName;
            this.txtSKU.Text = this.productCode;
            this.dropCategories.DataBind();
            this.dropCategories.SelectedValue = this.categoryId;

            this.ddlImportSourceType.DataBind();
            this.ddlImportSourceType.SelectedValue = this.importSourceId;

            this.ddlSupplier.DataBind();
            this.ddlSupplier.SelectedValue = this.supplierId;

            this.dropType.SelectedValue = this.typeId;
        }
    }
}

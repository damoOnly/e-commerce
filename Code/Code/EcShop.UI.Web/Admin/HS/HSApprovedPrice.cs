using ASPNET.WebControls;
using EcShop.ControlPanel.Commodities;
using EcShop.ControlPanel.Store;
using EcShop.Core;
using EcShop.Core.Entities;
using EcShop.Core.Enums;
using EcShop.Entities.Commodities;
using EcShop.Entities.Store;
using EcShop.UI.Common.Controls;
using EcShop.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;

namespace EcShop.UI.Web.Admin
{
     [PrivilegeCheck(Privilege.HSApprovedPrice)]
    public class HSApprovedPrice : AdminPage
    {
        private string productName;
        private int? categoryId;
        private int? typeId;
        private int? importSourceId;
        private int? supplierId;
        private int? IsApprovedPrice;

        protected System.Web.UI.WebControls.TextBox txtSearchText;
        protected ProductCategoriesDropDownList dropCategories;
        protected BrandCategoriesDropDownList dropBrandList;
        protected ProductTypeDownList dropType;
        protected System.Web.UI.WebControls.Button btnSearch;
        protected ImportSourceTypeDropDownList ddlImportSourceType;
        protected SupplierDropDownList ddlSupplier;
        protected System.Web.UI.WebControls.DropDownList dropIsApprovedPrice;

        protected Grid grdProducts;
        protected Pager pager;

        protected System.Web.UI.HtmlControls.HtmlInputHidden hidProductId;
        protected System.Web.UI.HtmlControls.HtmlInputHidden hidProductName;
        protected System.Web.UI.HtmlControls.HtmlInputHidden hidRefusalReason;
        protected System.Web.UI.WebControls.Button btnAccept;
        protected System.Web.UI.WebControls.Button btnRefuse;

        protected void Page_Load(object sender, System.EventArgs e)
        {
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);

            this.btnAccept.Click += new System.EventHandler(this.btnAccept_Click);
            this.btnRefuse.Click += new System.EventHandler(this.btnRefuse_Click);

            this.grdProducts.RowDataBound += new System.Web.UI.WebControls.GridViewRowEventHandler(this.grdProducts_RowDataBound);
          
            if (!this.Page.IsPostBack)
            {
                this.dropCategories.DataBind();
                this.dropBrandList.DataBind();
                this.dropType.DataBind();
                this.ddlImportSourceType.DataBind();
                this.ddlSupplier.DataBind();
                this.BindProducts();
               
            }
            //CheckBoxColumn.RegisterClientCheckEvents(this.Page, this.Page.Form.ClientID);
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
                SaleStatus=ProductSaleStatus.All,
                Keywords = this.productName,
                CategoryId = this.categoryId,
                PageSize = this.pager.PageSize,
                PageIndex = this.pager.PageIndex,
                SortOrder = SortAction.Desc,
                SortBy = "DisplaySequence",
                BrandId = this.dropBrandList.SelectedValue.HasValue ? this.dropBrandList.SelectedValue : null,
                TypeId = this.typeId,
                ImportSourceId = this.ddlImportSourceType.SelectedValue.HasValue ? this.ddlImportSourceType.SelectedValue : null,
                IsApprovedPrice = this.IsApprovedPrice
            };
            if (this.ddlSupplier.SelectedValue.HasValue)
            {
                productQuery.SupplierId = this.ddlSupplier.SelectedValue;
            }
            if (this.categoryId.HasValue)
            {
                productQuery.MaiCategoryPath = CatalogHelper.GetCategory(this.categoryId.Value).Path;
            }

            productQuery.SaleType = 1;

            Globals.EntityCoding(productQuery, true);
            DbQueryResult products = ProductHelper.GetProductsAdmin(productQuery);
            this.grdProducts.DataSource = products.Data;
            this.grdProducts.DataBind();
            this.txtSearchText.Text = productQuery.Keywords;
            this.dropCategories.SelectedValue = productQuery.CategoryId;
            this.dropType.SelectedValue = productQuery.TypeId;
            this.ddlImportSourceType.SelectedValue = productQuery.ImportSourceId;
            this.ddlSupplier.SelectedValue = productQuery.SupplierId;



            if (productQuery.IsApprovedPrice.HasValue)
            {
                this.dropIsApprovedPrice.SelectedValue = productQuery.IsApprovedPrice.Value.ToString();
            }


            this.pager.TotalRecords = products.TotalRecords;
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
            if (this.dropIsApprovedPrice.SelectedValue != "-1")
            {
                nameValueCollection.Add("IsApprovedPrice", this.dropIsApprovedPrice.SelectedValue);
            }
            base.ReloadPage(nameValueCollection);
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
                this.dropBrandList.SelectedValue = new int?(value2);
            }
            int value4 = 0;
            if (int.TryParse(this.Page.Request.QueryString["typeId"], out value4))
            {
                this.typeId = new int?(value4);
            }

            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["IsApprovedPrice"]))//是否审价过滤
            {
                int tmpIsApprovedPrice = 0;
                if (int.TryParse(this.Page.Request.QueryString["IsApprovedPrice"], out tmpIsApprovedPrice))
                {
                    this.IsApprovedPrice = new Int32?(tmpIsApprovedPrice);
                }
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
            this.dropCategories.DataBind();
            this.dropCategories.SelectedValue = this.categoryId;

            this.ddlImportSourceType.DataBind();
            this.ddlImportSourceType.SelectedValue = this.importSourceId;

            this.ddlSupplier.DataBind();
            this.ddlSupplier.SelectedValue = this.supplierId;
        }

        protected void btnAccept_Click(object sender, System.EventArgs e)
        {
            int productId = 0;
            int.TryParse(this.hidProductId.Value, out productId);
            string approvedPriceDescription = this.hidRefusalReason.Value;

            if(string.IsNullOrWhiteSpace(approvedPriceDescription))
            {
                approvedPriceDescription = "审核通过";
            }
            
            if (ProductHelper.IsPriceApproved(productId))
            {
                
                this.ShowMsg("商品已经审核通过", true);
                return;
            }

            if (ProductHelper.AcceptPriceApprove(productId,approvedPriceDescription))
            {
                EventLogs.WriteOperationLog(Privilege.HSApprovedPrice, string.Format(CultureInfo.InvariantCulture, "商品{0}审价成功,商品id为{1}", new object[]
					{
                        this.hidProductName.Value,
                        productId
						
                        
					}));
                this.BindProducts();
                this.ShowMsg("商品的成功通过价格审核申请", true);
                
            }
            else
            {
                this.ShowMsg("审核通过失败", true);
            }
            
        }
        private void btnRefuse_Click(object sender, System.EventArgs e)
        {
            int productId = 0;
            int.TryParse(this.hidProductId.Value, out productId);
            if (ProductHelper.RefusePriceApprove(productId, this.hidRefusalReason.Value))
            {
                EventLogs.WriteOperationLog(Privilege.HSApprovedPrice, string.Format(CultureInfo.InvariantCulture, "商品{0}审价不通过,商品id为{1}，拒绝原因：{2}", new object[]
					{
                        this.hidProductName.Value,
						
                        productId,
						
                        this.hidRefusalReason.Value
					}));
                this.BindProducts();
                this.ShowMsg("拒绝了商品的价格审核申请", true);
               
                return;
            }
            this.ShowMsg("拒绝通过失败", true);
        }

        public string ProductDetails(string isprove, string productid, string productname)
        {
            string resultstr = string.Empty;
            if (isprove == "True")
            {
                resultstr = "<a href='../../ProductDetails.aspx?productId=" + productid + "' target=\"_blank\">" + productname + "</a>";
            }
            else
            {
                resultstr = "" + productname + "";
            }
            return resultstr;
        }

        


        private void grdProducts_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            if (e.Row.RowType == System.Web.UI.WebControls.DataControlRowType.DataRow)
            {
                Repeater repeterSkuItemsList = (Repeater)e.Row.FindControl("repeterSkuItems");
                int  productId;
                if (repeterSkuItemsList != null)
                {
                    if(int.TryParse(System.Web.UI.DataBinder.Eval(e.Row.DataItem, "ProductId").ToString(),out productId))
                    {
                        DataTable skuItems = ProductHelper.GetExtendSkusByProductId(productId);
                        repeterSkuItemsList.DataSource = skuItems;
                        repeterSkuItemsList.DataBind();
                    }

                }
            }
        }
    }
}

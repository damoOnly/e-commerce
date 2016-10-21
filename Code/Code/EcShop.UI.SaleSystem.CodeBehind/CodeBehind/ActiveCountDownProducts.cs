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
    public class ActiveCountDownProducts : HtmlTemplatedWebControl
    {
        private ThemedTemplatedRepeater rptProduct;
        private ThemedTemplatedRepeater rptProduct2;
        private ThemedTemplatedRepeater rptProduct3;
        private ThemedTemplatedRepeater rptProduct4;
        private ThemedTemplatedRepeater rptProduct5;
        private Pager pager;
        protected override void OnInit(System.EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-ActiveCountDownProducts.html";
            }
            base.OnInit(e);
        }
        protected override void AttachChildControls()
        {
            this.rptProduct = (ThemedTemplatedRepeater)this.FindControl("rptProduct");
            this.rptProduct2 = (ThemedTemplatedRepeater)this.FindControl("rptProduct2");
            this.rptProduct3 = (ThemedTemplatedRepeater)this.FindControl("rptProduct3");
            this.rptProduct4 = (ThemedTemplatedRepeater)this.FindControl("rptProduct4");
            this.rptProduct5 = (ThemedTemplatedRepeater)this.FindControl("rptProduct5");
            this.pager = (Pager)this.FindControl("pager");
            if (!this.Page.IsPostBack)
            {
                this.BindProduct();
            }
        }
        private void BindProduct()
        {
            ProductBrowseQuery productBrowseQuery = this.GetProductBrowseQuery();
            DbQueryResult countDownProductList = ProductBrowser.GetCountDownProductList_Active_temp(productBrowseQuery);
            this.rptProduct.DataSource = countDownProductList.Data;
            this.rptProduct.DataBind();

            DbQueryResult countDownProductList2 = ProductBrowser.GetCountDownProductList_Active_temp2(productBrowseQuery);
            this.rptProduct2.DataSource = countDownProductList2.Data;
            this.rptProduct2.DataBind();

            DbQueryResult countDownProductList3 = ProductBrowser.GetCountDownProductList_Active_temp3(productBrowseQuery);
            this.rptProduct3.DataSource = countDownProductList3.Data;
            this.rptProduct3.DataBind();

            DbQueryResult countDownProductList4 = ProductBrowser.GetCountDownProductList_Active_temp4(productBrowseQuery);
            this.rptProduct4.DataSource = countDownProductList4.Data;
            this.rptProduct4.DataBind();

            DbQueryResult countDownProductList5 = ProductBrowser.GetCountDownProductList_Active_temp5(productBrowseQuery);
            this.rptProduct5.DataSource = countDownProductList5.Data;
            this.rptProduct5.DataBind();

            this.pager.TotalRecords = countDownProductList.TotalRecords;
        }
        private ProductBrowseQuery GetProductBrowseQuery()
        {
            return new ProductBrowseQuery
            {
                IsCount = true,
                PageIndex = this.pager.PageIndex,
                PageSize = this.pager.PageSize,
                SortBy = "CurHours",
                SortOrder = SortAction.Asc
            };
        }
    }
}

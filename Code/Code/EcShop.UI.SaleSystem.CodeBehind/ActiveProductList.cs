using ASPNET.WebControls;
using Commodities;
using EcShop.ControlPanel.Commodities;
using EcShop.ControlPanel.Sales;
using EcShop.Core;
using EcShop.Core.Entities;
using EcShop.Core.Enums;
using EcShop.Entities.Commodities;
using EcShop.SaleSystem.Catalog;
using EcShop.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
namespace EcShop.UI.SaleSystem.CodeBehind
{
    [System.Web.UI.ParseChildren(true)]
    public class ActiveProductList : HtmlTemplatedWebControl
    {


        private ThemedTemplatedRepeater rptProducts;

        private System.Web.UI.WebControls.Literal litProductCount;
        private System.Web.UI.WebControls.Literal litSupplierDescribe;
        private System.Web.UI.HtmlControls.HtmlInputHidden serach_text;
        private System.Web.UI.HtmlControls.HtmlInputHidden search_Subtext;
        private Pager pager;
        protected override void OnInit(System.EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-ActiveProductList.html";
            }
            base.OnInit(e);
        }

        protected override void AttachChildControls()
        {
            this.serach_text = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("search_text");
            this.search_Subtext = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("search_Subtext");
            this.rptProducts = (ThemedTemplatedRepeater)this.FindControl("rptProducts");

            this.pager = (Pager)this.FindControl("pager");
            this.litProductCount = (System.Web.UI.WebControls.Literal)this.FindControl("litProductCount");
            this.litSupplierDescribe = (System.Web.UI.WebControls.Literal)this.FindControl("litSupplierDescribe");


            if (!this.Page.IsPostBack)
            {

                this.BindSearch();
            }

        }


        protected void BindSearch()
        {

            ProductBrowseQuery productBrowseQuery = this.GetCurrProductBrowseQuery();
            if (this.serach_text != null)
            {
                this.serach_text.Value = productBrowseQuery.Keywords;
            }
            if (search_Subtext != null)
            {
                search_Subtext.Value = productBrowseQuery.SubKeywords;
            }

            var browseProductList = ActiveHelper.GetCurrBrowseActiveProductListByTopicId(productBrowseQuery);// ProductBrowser.GetCurrBrowseProductList(productBrowseQuery);
            this.rptProducts.DataSource = browseProductList.Data;
            this.rptProducts.DataBind();


        }
        protected ProductBrowseQuery GetCurrProductBrowseQuery()
        {
            ProductBrowseQuery productBrowseQuery = GetProductBrowseQuery();

            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["keywords"]))
            {
                productBrowseQuery.Keywords = DataHelper.CleanSearchString(Globals.HtmlEncode(Globals.UrlDecode(this.Page.Request.QueryString["keywords"])));
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["topic"]))
            {
                productBrowseQuery.TopId = int.Parse(this.Page.Request.QueryString["topic"]);
            }

            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["sortOrderBy"]))
            {
                string key = Globals.HtmlEncode(Globals.UrlDecode(this.Page.Request.QueryString["sortOrderBy"]));
                switch (key)
                {
                    case "SalePrice":
                        break;
                    case "ShowSaleCounts":
                        break;
                    case "AddedDate":
                        break;
                    case "DisplaySequence":
                        break;
                    default:
                        key = "DisplaySequence";
                        break;
                }
                productBrowseQuery.SortBy = Globals.HtmlEncode(key);
            }
            else
            {
                productBrowseQuery.SortBy = "DisplaySequence";
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["sortOrder"]))
            {
                string sortOrder = Globals.HtmlEncode(this.Page.Request.QueryString["sortOrder"]);
                if (!sortOrder.ToLower().Equals("desc") && !sortOrder.ToLower().Equals("asc"))
                {
                    sortOrder = "Desc";
                }
                productBrowseQuery.SortOrder = (SortAction)System.Enum.Parse(typeof(SortAction), sortOrder);
            }
            else
            {
                productBrowseQuery.SortOrder = SortAction.Desc;
            }

            productBrowseQuery.PageIndex = this.pager == null ? 1 : this.pager.PageIndex;
            productBrowseQuery.PageSize = this.pager == null ? 100 : this.pager.PageSize;

            Globals.EntityCoding(productBrowseQuery, true);
            return productBrowseQuery;
        }

        protected ProductBrowseQuery GetProductBrowseQuery()
        {
            ProductBrowseQuery productBrowseQuery = new ProductBrowseQuery();
            productBrowseQuery.PageIndex = 1;
            productBrowseQuery.PageSize = 100;
            productBrowseQuery.SortBy = "DisplaySequence";
            productBrowseQuery.SortOrder = SortAction.Desc;
            Globals.EntityCoding(productBrowseQuery, true);
            return productBrowseQuery;
        }
    }
}

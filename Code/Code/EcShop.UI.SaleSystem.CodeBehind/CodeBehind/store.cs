using ASPNET.WebControls;
using Commodities;
using EcShop.ControlPanel.Commodities;
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
    public class store : HtmlTemplatedWebControl
    {
        int supplierId;

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
                this.SkinName = "Skin-Store.html";
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

            if (!int.TryParse(this.Page.Request.QueryString["supplierId"], out supplierId))
            {
                base.GotoResourceNotFound();
                return;
            }

            SupplierInfo shipper = SupplierHelper.GetSupplier(this.supplierId);
            if (shipper == null)
            {
                base.GotoResourceNotFound();
                return;
            }
            else {
                litSupplierDescribe.Text = shipper.Description;
            }
            
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
            productBrowseQuery.supplierid = this.supplierId;

            DbQueryResult browseProductList = ProductBrowser.GetCurrBrowseProductList(productBrowseQuery);
            this.rptProducts.DataSource = browseProductList.Data;
            this.rptProducts.DataBind();

            this.pager.TotalRecords = browseProductList.TotalRecords;
            int pageCount = browseProductList.TotalRecords / this.pager.PageSize;
            if (browseProductList.TotalRecords % this.pager.PageSize > 0)
            {
                pageCount++;
            }

            if (pageCount <= 1)
            {
                this.litProductCount.Text = string.Format("<span class=\"fb\">共<span class=\"total-count\">{0}</span>件商品</span><span class=\"pager\"><span class=\"cur-page\">{1}</span>/<span class=\"total-page\">{1}页</span></span>", browseProductList.TotalRecords, pageCount);
                return;
            }
            #region 分页控件代码

            int pageIndex = 1;
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["pageindex"]))
            {
                int.TryParse(this.Page.Request.QueryString["pageindex"], out pageIndex);
            }
            if (browseProductList.TotalRecords <= 0)
            {
                pageIndex = 0;
            }

            string previousPager = "";
            string nextPager = "";

            Regex regex = new Regex(@"(pageindex=\d+)");
            MatchCollection tempMatches = regex.Matches(this.Page.Request.RawUrl);
            if (tempMatches.Count == 0)
            {
                previousPager = this.Page.Request.RawUrl + (this.Page.Request.RawUrl.Contains("?") ? "&" : "?") + "pageindex=1";
                if (pageCount > 1)
                {
                    nextPager = this.Page.Request.RawUrl + (this.Page.Request.RawUrl.Contains("?") ? "&" : "?") + "pageindex=2";
                }
                else
                {
                    nextPager = previousPager;
                }
            }
            if (tempMatches.Count == 1)
            {
                foreach (Match item in tempMatches)
                {
                    string pagerInfo = item.Value;
                    if (pageIndex <= pageCount)
                    {
                        if ((pageIndex - 1) > 0)
                        {
                            previousPager = this.Page.Request.RawUrl.Replace(pagerInfo, "") + "pageindex=" + (pageIndex - 1);
                        }
                        else
                        {
                            previousPager = this.Page.Request.RawUrl.Replace(pagerInfo, "") + "pageindex=1";
                        }
                        if ((pageIndex + 1) <= pageCount)
                        {
                            nextPager = this.Page.Request.RawUrl.Replace(pagerInfo, "") + "pageindex=" + (pageIndex + 1);
                        }
                        else
                        {
                            nextPager = this.Page.Request.RawUrl.Replace(pagerInfo, "") + "pageindex=" + pageCount;
                        }
                    }
                }
            }

            #endregion

            if (pageCount > 1 && pageIndex == 1)
            {
                this.litProductCount.Text = string.Format("<span class=\"fb\">共<span>{0}</span>件商品</span><span class=\"pager\"><span class=\"cur-page\">{2}</span>/<span class=\"total-page\">{3}页</span><a class=\"prev-page disabled\" href=\"{1}\">上一页</a><a class=\"next-page\" href=\"{4}\">下一页</a></span>", new object[]
			{
				browseProductList.TotalRecords,
				previousPager,
                pageIndex,
				pageCount,
				nextPager
			});
            }
            else
            {
                this.litProductCount.Text = string.Format("<span class=\"fb\">共<span>{0}</span>件商品</span><span class=\"pager\"><span class=\"cur-page\">{2}</span>/<span class=\"total-page\">{3}页</span><a class=\"prev-page\" href=\"{1}\">上一页</a><a class=\"next-page\" href=\"{4}\">下一页</a></span>", new object[]
			{
				browseProductList.TotalRecords,
				previousPager,
                pageIndex,
				pageCount,
				nextPager
			});
            }

        }
        protected ProductBrowseQuery GetCurrProductBrowseQuery()
        {
            ProductBrowseQuery productBrowseQuery = new ProductBrowseQuery();

            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["keywords"]))
            {
                productBrowseQuery.Keywords = DataHelper.CleanSearchString(Globals.HtmlEncode(Globals.UrlDecode(this.Page.Request.QueryString["keywords"])));
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
            productBrowseQuery.PageSize = this.pager == null ? 20 : this.pager.PageSize;
           
            Globals.EntityCoding(productBrowseQuery, true);
            return productBrowseQuery;
        }
    }
}

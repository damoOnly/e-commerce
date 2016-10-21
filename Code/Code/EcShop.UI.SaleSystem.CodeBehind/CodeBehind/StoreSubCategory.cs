using ASPNET.WebControls;
using Commodities;
using EcShop.ControlPanel.Commodities;
using EcShop.Core;
using EcShop.Core.Entities;
using EcShop.Core.Enums;
using EcShop.Entities.Commodities;
using EcShop.SaleSystem.Catalog;
using EcShop.UI.Common.Controls;
using EcShop.UI.SaleSystem.Tags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace EcShop.UI.SaleSystem.CodeBehind
{
    public class StoreSubCategory : HtmlTemplatedWebControl
    {
        int supplierId;

        private System.Web.UI.WebControls.Literal litSearchResultPage;
        private Pager pager;
        private ThemedTemplatedRepeater rptProducts;

        private Common_GoodsList_HotSale hotSale;//销售排行
        protected override void OnInit(System.EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-StoreSubCategory.html";
            }
            base.OnInit(e);
        }

        protected override void AttachChildControls()
        {
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

            this.rptProducts = (ThemedTemplatedRepeater)this.FindControl("rptProducts");
            this.hotSale = (Common_GoodsList_HotSale)this.FindControl("list_Common_GoodsList_HotSale");//销售排行
            this.pager = (Pager)this.FindControl("pager");
            this.litSearchResultPage = (System.Web.UI.WebControls.Literal)this.FindControl("litSearchResultPage");


            if (!this.Page.IsPostBack)
            {
               
                this.BindSearch();
            }
        }


        protected void BindSearch()
        {

            ProductBrowseQuery productBrowseQuery = this.GetCurrProductBrowseQuery();

            productBrowseQuery.supplierid = this.supplierId;
            //if (this.serach_text != null)
            //{
            //    this.serach_text.Value = productBrowseQuery.Keywords;
            //}
            //if (search_Subtext != null)
            //{
            //    search_Subtext.Value = productBrowseQuery.SubKeywords;
            //}
            DbQueryResult browseProductList = ProductBrowser.GetCurrBrowseProductList(productBrowseQuery);
            this.rptProducts.DataSource = browseProductList.Data;
            this.rptProducts.DataBind();

            #region 获取控件品牌、产地过滤条件
            //List<string> search_BrandStr = new List<string>();
            //List<string> search_ProducingStr = new List<string>();
            //if (common_attributeslist != null)
            //{
            //    DataTable dt = ProductBrowser.GetBrowseProductImportSourceIdAndBrandIdList(productBrowseQuery);
            //    foreach (DataRow r in dt.Rows)
            //    {
            //        string str = r["BrandId"].ToString();
            //        if (!search_BrandStr.Contains(str) && !string.IsNullOrWhiteSpace(str))
            //        {
            //            search_BrandStr.Add(str);
            //        }
            //        str = r["ImportSourceId"].ToString();
            //        if (!search_ProducingStr.Contains(str) && !string.IsNullOrWhiteSpace(str) && !str.Equals("0"))
            //        {
            //            search_ProducingStr.Add(str);
            //        }
            //    }
            //    //更新品牌、产地控件显示值

            //    this.common_attributeslist.search_BrandStr = search_BrandStr;
            //    this.common_attributeslist.search_ProducingStr = search_ProducingStr;
            //}
            #endregion

            this.hotSale = (Common_GoodsList_HotSale)this.FindControl("list_Common_GoodsList_HotSale");//销售排行
            if (this.hotSale != null)
            {
                int cid = 0;
                int brandId = 0;
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["categoryId"]))
                {
                    int.TryParse(this.Page.Request.QueryString["categoryId"], out cid);
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["brand"]))
                {
                    int.TryParse(this.Page.Request.QueryString["brand"], out brandId);
                }
                this.hotSale.DataSource = ProductBrowser.GetBrowseHotProductList(cid, brandId,supplierId).DBHotSale;
                this.hotSale.DataBind();
            }

            this.pager.TotalRecords = browseProductList.TotalRecords;
            int pageCount = browseProductList.TotalRecords / this.pager.PageSize;
            if (browseProductList.TotalRecords % this.pager.PageSize > 0)
            {
                pageCount++;
            }

            if (pageCount <= 1)
            {
                //this.litSearchResultPage.Text = string.Format("共<span>{0}</span>件商品<span class=\"pager\">&lt;<span class=\"cur-page\">{1}</span>/<span class=\"total-page\">{1}</span>&gt;</span>", browseProductList.TotalRecords, pageCount);
                this.litSearchResultPage.Text = string.Format("<span class=\"fb\">共<span class=\"total-count\">{0}</span>件商品</span><span class=\"pager\"><span class=\"cur-page\">{1}</span>/<span class=\"total-page\">{1}页</span></span>", browseProductList.TotalRecords, pageCount);
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
                this.litSearchResultPage.Text = string.Format("<span class=\"fb\">共<span>{0}</span>件商品</span><span class=\"pager\"><span class=\"cur-page\">{2}</span>/<span class=\"total-page\">{3}页</span><a class=\"prev-page disabled\" href=\"{1}\">上一页</a><a class=\"next-page\" href=\"{4}\">下一页</a></span>", new object[]
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
                this.litSearchResultPage.Text = string.Format("<span class=\"fb\">共<span>{0}</span>件商品</span><span class=\"pager\"><span class=\"cur-page\">{2}</span>/<span class=\"total-page\">{3}页</span><a class=\"prev-page\" href=\"{1}\">上一页</a><a class=\"next-page\" href=\"{4}\">下一页</a></span>", new object[]
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
            if (!string.IsNullOrWhiteSpace(this.Page.Request.QueryString["categoryId"]))
            {
                productBrowseQuery.StrCategoryId = this.Page.Request.QueryString["categoryId"];
            }
            if (!string.IsNullOrWhiteSpace(this.Page.Request.QueryString["brand"]))
            {
                productBrowseQuery.StrBrandId = this.Page.Request.QueryString["brand"];
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["valueStr"]))
            {
                System.Collections.Generic.IList<AttributeValueInfo> list = new System.Collections.Generic.List<AttributeValueInfo>();
                string text = Globals.UrlDecode(this.Page.Request.QueryString["valueStr"]);
                text = Globals.HtmlEncode(text);
                string[] array = text.Split(new char[]
				{
					'-'
				});
                if (!string.IsNullOrEmpty(text))
                {
                    for (int i = 0; i < array.Length; i++)
                    {
                        string[] array2 = array[i].Split(new char[]
						{
							'_'
						});
                        if (array2.Length > 0 && !string.IsNullOrEmpty(array2[1]) && array2[1] != "0")
                        {
                            list.Add(new AttributeValueInfo
                            {
                                AttributeId = System.Convert.ToInt32(array2[0]),
                                ValueId = System.Convert.ToInt32(array2[1])
                            });
                        }
                    }
                }
                productBrowseQuery.AttributeValues = list;
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["isPrecise"]))
            {
                productBrowseQuery.IsPrecise = bool.Parse(Globals.UrlDecode(this.Page.Request.QueryString["isPrecise"]));
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["keywords"]))
            {
                productBrowseQuery.Keywords = DataHelper.CleanSearchString(Globals.HtmlEncode(Globals.UrlDecode(this.Page.Request.QueryString["keywords"])));
            }

            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["subKeywords"]))
            {
                productBrowseQuery.SubKeywords = DataHelper.CleanSearchString(Globals.HtmlEncode(Globals.UrlDecode(this.Page.Request.QueryString["SubKeywords"])));
            }

            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["minSalePrice"]))
            {
                decimal value3 = 0m;
                if (decimal.TryParse(Globals.UrlDecode(this.Page.Request.QueryString["minSalePrice"]), out value3))
                {
                    productBrowseQuery.MinSalePrice = new decimal?(value3);
                }
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["maxSalePrice"]))
            {
                decimal value4 = 0m;
                if (decimal.TryParse(Globals.UrlDecode(this.Page.Request.QueryString["maxSalePrice"]), out value4))
                {
                    productBrowseQuery.MaxSalePrice = new decimal?(value4);
                }
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["TagIds"]))
            {
                productBrowseQuery.TagIds = Globals.UrlDecode(this.Page.Request.QueryString["TagIds"]);
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["productCode"]))
            {
                productBrowseQuery.ProductCode = Globals.HtmlEncode(Globals.UrlDecode(this.Page.Request.QueryString["productCode"]));
            }
            if (!string.IsNullOrWhiteSpace(this.Page.Request.QueryString["importsourceid"]))//新增原产地
            {
                productBrowseQuery.StrImportsourceId = this.Page.Request.QueryString["importsourceid"];
            }
            productBrowseQuery.PageIndex = this.pager == null ? 1 : this.pager.PageIndex;
            productBrowseQuery.PageSize = this.pager == null ? 5 : this.pager.PageSize;
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


            //筛选是否有货
            if (string.IsNullOrEmpty(this.Page.Request.QueryString["Stock"]))
            {

                productBrowseQuery.HasStock = false;
            }
            else
            {
                if (this.Page.Request.QueryString["Stock"].ToString().ToLower() == "has")
                {
                    productBrowseQuery.HasStock = true;
                }
                else
                {
                    productBrowseQuery.HasStock = false;
                }

            }
            Globals.EntityCoding(productBrowseQuery, true);
            return productBrowseQuery;
        }
    }
}


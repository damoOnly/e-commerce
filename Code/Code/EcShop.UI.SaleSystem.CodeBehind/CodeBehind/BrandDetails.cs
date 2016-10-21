using ASPNET.WebControls;
using EcShop.Core;
using EcShop.Core.Entities;
using EcShop.Core.Enums;
using EcShop.Entities.Commodities;
using EcShop.Membership.Context;
using EcShop.SaleSystem.Catalog;
using EcShop.UI.Common.Controls;
using EcShop.UI.SaleSystem.Tags;
using System;
using System.Collections.Specialized;
using System.IO;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace EcShop.UI.SaleSystem.CodeBehind
{
	[System.Web.UI.ParseChildren(true)]
	public class BrandDetails : HtmlTemplatedWebControl
	{
		private System.Web.UI.WebControls.Literal litBrandName;
        private ThemedTemplatedRepeater rptProducts;
		private Pager pager;
		private System.Web.UI.WebControls.Literal litBrandRemark;
		private Common_CutdownSearch cutdownSearch;
		private Common_Search_SortPrice btnSortPrice;
		private Common_Search_SortTime btnSortTime;
		private Common_Search_SortPopularity btnSortPopularity;
		private Common_Search_SortSaleCounts btnSortSaleCounts;
		private int brandId;
        private Common_GoodsList_HotSale hotSale;//销售排行
        private System.Web.UI.WebControls.Literal litSearchResultPage;
		public BrandDetails()
		{
			if (!int.TryParse(this.Page.Request.QueryString["brandId"], out this.brandId))
			{
				base.GotoResourceNotFound();
			}
			BrandCategoryInfo brandCategory = CategoryBrowser.GetBrandCategory(this.brandId);
			if (brandCategory != null && !string.IsNullOrEmpty(brandCategory.Theme) && System.IO.File.Exists(HiContext.Current.Context.Request.MapPath(HiContext.Current.GetSkinPath() + "/brandcategorythemes/" + brandCategory.Theme)))
			{
				this.SkinName = "/brandcategorythemes/" + brandCategory.Theme;
			}
		}
		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-BrandDetails.html";
			}
			base.OnInit(e);
		}
		protected override void AttachChildControls()
		{
			this.litBrandName = (System.Web.UI.WebControls.Literal)this.FindControl("litBrandName");
			this.litBrandRemark = (System.Web.UI.WebControls.Literal)this.FindControl("litBrandRemark");
            this.rptProducts = (ThemedTemplatedRepeater)this.FindControl("rptProducts");
			this.pager = (Pager)this.FindControl("pager");
			this.cutdownSearch = (Common_CutdownSearch)this.FindControl("search_Common_CutdownSearch");
			this.btnSortPrice = (Common_Search_SortPrice)this.FindControl("btn_Common_Search_SortPrice");
			this.btnSortTime = (Common_Search_SortTime)this.FindControl("btn_Common_Search_SortTime");
			this.btnSortPopularity = (Common_Search_SortPopularity)this.FindControl("btn_Common_Search_SortPopularity");
			this.btnSortSaleCounts = (Common_Search_SortSaleCounts)this.FindControl("btn_Common_Search_SortSaleCounts");
			this.cutdownSearch.ReSearch += new Common_CutdownSearch.ReSearchEventHandler(this.cutdownSearch_ReSearch);
			this.btnSortPrice.Sorting += new Common_Search_SortTime.SortingHandler(this.btnSortPrice_Sorting);
			this.btnSortTime.Sorting += new Common_Search_SortTime.SortingHandler(this.btnSortTime_Sorting);
            this.hotSale = (Common_GoodsList_HotSale)this.FindControl("list_Common_GoodsList_HotSale");//销售排行
            this.litSearchResultPage = (System.Web.UI.WebControls.Literal)this.FindControl("litSearchResultPage");
			if (this.btnSortPopularity != null)
			{
				this.btnSortPopularity.Sorting += new Common_Search_SortPopularity.SortingHandler(this.btnSortPopularity_Sorting);
			}
			if (this.btnSortSaleCounts != null)
			{
				this.btnSortSaleCounts.Sorting += new Common_Search_SortSaleCounts.SortingHandler(this.btnSortSaleCounts_Sorting);
			}
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
                this.hotSale.DataSource = ProductBrowser.GetBrowseHotProductList(cid, brandId).DBHotSale;
                this.hotSale.DataBind();
            }
			if (!this.Page.IsPostBack)
			{
				BrandCategoryInfo brandCategory = CategoryBrowser.GetBrandCategory(this.brandId);
				if (brandCategory == null)
				{
					this.Page.Response.Redirect(Globals.ApplicationPath + "/ResourceNotFound.aspx?errorMsg=" + Globals.UrlEncode("该品牌已经不存在"));
					return;
				}
				this.LoadCategoryHead(brandCategory);
				this.litBrandName.Text = brandCategory.BrandName;
				this.litBrandRemark.Text = brandCategory.Description;
				PageTitle.AddSiteNameTitle(brandCategory.BrandName);
				this.BindBrandProduct();
			}
		}
		private void cutdownSearch_ReSearch(object sender, System.EventArgs e)
		{
			this.ReloadBrand(string.Empty, string.Empty);
		}
		private void btnSortTime_Sorting(string sortOrder, string sortOrderBy)
		{
			this.ReloadBrand(sortOrder, sortOrderBy);
		}
		private void btnSortSaleCounts_Sorting(string sortOrder, string sortOrderBy)
		{
			this.ReloadBrand(sortOrder, sortOrderBy);
		}
		private void btnSortPopularity_Sorting(string sortOrder, string sortOrderBy)
		{
			this.ReloadBrand(sortOrder, sortOrderBy);
		}
		private void btnSortPrice_Sorting(string sortOrder, string sortOrderBy)
		{
			this.ReloadBrand(sortOrder, sortOrderBy);
		}
		private void LoadCategoryHead(BrandCategoryInfo brandcategory)
		{
			if (!string.IsNullOrEmpty(brandcategory.MetaKeywords))
			{
				MetaTags.AddMetaKeywords(brandcategory.MetaKeywords, HiContext.Current.Context);
			}
			if (!string.IsNullOrEmpty(brandcategory.MetaKeywords))
			{
				MetaTags.AddMetaDescription(brandcategory.MetaDescription, HiContext.Current.Context);
			}
			if (!string.IsNullOrEmpty(brandcategory.BrandName))
			{
				PageTitle.AddSiteNameTitle(brandcategory.BrandName);
			}
		}
		private void BindBrandProduct()
		{
			ProductBrowseQuery productBrowseQuery = this.GetProductBrowseQuery();
			DbQueryResult browseProductList = ProductBrowser.GetBrowseProductList(productBrowseQuery);
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
		private void ReloadBrand(string sortOrder, string sortOrderBy)
		{
			base.ReloadPage(new System.Collections.Specialized.NameValueCollection
			{

				{
					"brandId",
					this.brandId.ToString()
				},

				{
					"TagIds",
					Globals.UrlEncode(this.cutdownSearch.Item.TagIds)
				},

				{
					"keywords",
					Globals.UrlEncode(this.cutdownSearch.Item.Keywords)
				},

				{
					"minSalePrice",
					Globals.UrlEncode(this.cutdownSearch.Item.MinSalePrice.ToString())
				},

				{
					"maxSalePrice",
					Globals.UrlEncode(this.cutdownSearch.Item.MaxSalePrice.ToString())
				},

				{
					"productCode",
					Globals.UrlEncode(this.cutdownSearch.Item.ProductCode)
				},

				{
					"pageIndex",
					this.pager.PageIndex.ToString()
				},

				{
					"sortOrderBy",
					sortOrderBy
				},

				{
					"sortOrder",
					sortOrder
				}
			});
		}
		private ProductBrowseQuery GetProductBrowseQuery()
		{
			ProductBrowseQuery productBrowseQuery = new ProductBrowseQuery();
            productBrowseQuery.IsApproved = 1;
			productBrowseQuery.BrandId = new int?(this.brandId);
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["TagIds"]))
			{
				productBrowseQuery.TagIds = Globals.UrlDecode(this.Page.Request.QueryString["TagIds"]);
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["keywords"]))
			{
				productBrowseQuery.Keywords = Globals.UrlDecode(this.Page.Request.QueryString["keywords"]);
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["minSalePrice"]))
			{
				decimal value = 0m;
				if (decimal.TryParse(Globals.UrlDecode(this.Page.Request.QueryString["minSalePrice"]), out value))
				{
					productBrowseQuery.MinSalePrice = new decimal?(value);
				}
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["maxSalePrice"]))
			{
				decimal value2 = 0m;
				if (decimal.TryParse(Globals.UrlDecode(this.Page.Request.QueryString["maxSalePrice"]), out value2))
				{
					productBrowseQuery.MaxSalePrice = new decimal?(value2);
				}
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["productCode"]))
			{
				productBrowseQuery.ProductCode = Globals.UrlDecode(this.Page.Request.QueryString["productCode"]);
			}
			productBrowseQuery.PageIndex = this.pager.PageIndex;
			productBrowseQuery.PageSize = this.pager.PageSize;
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["sortOrderBy"]))
			{
				productBrowseQuery.SortBy = this.Page.Request.QueryString["sortOrderBy"];
			}
			else
			{
				productBrowseQuery.SortBy = "DisplaySequence";
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["sortOrder"]))
			{
				productBrowseQuery.SortOrder = (SortAction)System.Enum.Parse(typeof(SortAction), this.Page.Request.QueryString["sortOrder"]);
			}
			else
			{
				productBrowseQuery.SortOrder = SortAction.Desc;
			}
			Globals.EntityCoding(productBrowseQuery, true);
			return productBrowseQuery;
		}
	}
}

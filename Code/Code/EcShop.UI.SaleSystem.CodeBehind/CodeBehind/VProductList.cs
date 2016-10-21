using EcShop.ControlPanel.Store;
using EcShop.Core.Entities;
using EcShop.Entities;
using EcShop.Entities.Commodities;
using EcShop.Membership.Context;
using EcShop.SaleSystem.Catalog;
using EcShop.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace EcShop.UI.SaleSystem.CodeBehind
{
    [System.Web.UI.ParseChildren(true)]
    public class VProductList : VshopTemplatedWebControl
    {
        private int categoryId;
        private int supplierId;
        private int TopicId;
        private int brandid;
        private int importsourceid;
        private string keyWord;
        private HiImage imgUrl;
        private System.Web.UI.WebControls.Literal litContent;
        private System.Web.UI.WebControls.Literal litSiblingsCategories;
        private VshopTemplatedRepeater rptProducts;
        private VshopTemplatedRepeater rptCategories;
        private System.Web.UI.HtmlControls.HtmlInputHidden txtTotalPages;
        private VshopTemplatedRepeater rpthistorysearch;

        protected override void OnInit(System.EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-VProductList.html";
            }
            base.OnInit(e);
        }
        protected override void AttachChildControls()
        {
            int.TryParse(this.Page.Request.QueryString["categoryId"], out this.categoryId);
            int.TryParse(this.Page.Request.QueryString["SupplierId"], out this.supplierId);
            int.TryParse(this.Page.Request.QueryString["TopicId"], out this.TopicId);
            int.TryParse(this.Page.Request.QueryString["brandid"], out this.brandid);
            int.TryParse(this.Page.Request.QueryString["importsourceid"], out this.importsourceid);
            this.keyWord = this.Page.Request.QueryString["keyWord"];
            if (!string.IsNullOrWhiteSpace(this.keyWord))
            {
                this.keyWord = this.keyWord.Trim();
                if (HiContext.Current.User.UserId > 0)
                {
                    HistorySearchHelp.NewSearchHistory(this.keyWord.Trim(), HiContext.Current.User.UserId, Entities.ClientType.VShop);
                }
            }
            this.imgUrl = (HiImage)this.FindControl("imgUrl");
            this.litContent = (System.Web.UI.WebControls.Literal)this.FindControl("litContent");
            this.rptProducts = (VshopTemplatedRepeater)this.FindControl("rptProducts");
            this.rptCategories = (VshopTemplatedRepeater)this.FindControl("rptCategories");
            this.txtTotalPages = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("txtTotal");
            this.litSiblingsCategories = (System.Web.UI.WebControls.Literal)this.FindControl("litSiblingsCategories");
            this.rpthistorysearch = (VshopTemplatedRepeater)this.FindControl("rpthistorysearch");
            string text = this.Page.Request.QueryString["sort"];
            if (string.IsNullOrWhiteSpace(text))
            {
                text = "DisplaySequence";
            }
            string text2 = this.Page.Request.QueryString["order"];
            if (string.IsNullOrWhiteSpace(text2))
            {
                text2 = "desc";
            }
            int pageNumber;
            if (!int.TryParse(this.Page.Request.QueryString["page"], out pageNumber))
            {
                pageNumber = 1;
            }
            int maxNum;
            if (!int.TryParse(this.Page.Request.QueryString["size"], out maxNum))
            {
                maxNum = 8;
            }
            //获取其他分类

            System.Collections.Generic.IList<CategoryInfo> maxSubCategories = CategoryBrowser.GetMaxSubCategories(this.categoryId, 1000);
            this.rptCategories.DataSource = maxSubCategories;
            this.rptCategories.DataBind();

            IList<CategoryInfo> siblingsCategories = CategoryBrowser.GetSiblingsCategories(this.categoryId, 1000);
            //<li><a class="link" href="/Vshop/ProductList.aspx?categoryId=8000036">进口零食</a></li>
            StringBuilder stringBuilderCategories = new StringBuilder();
            foreach (CategoryInfo item in siblingsCategories)
            {
                stringBuilderCategories.AppendFormat("<li><a class=\"link\" href=\"/Vshop/ProductList.aspx?categoryId={0}\">{1}</a></li>", item.CategoryId, item.Name);
            }
            this.litSiblingsCategories.Text = stringBuilderCategories.ToString();
            int num;

            ProductBrowseQuery query = new ProductBrowseQuery();
            if(this.categoryId>0)
            {
            query.StrCategoryId = this.categoryId.ToString();
            }

            if(this.supplierId>0)
            {
            query.supplierid = this.supplierId;
            }

            if(this.brandid>0)
            {
                query.StrBrandId=this.brandid.ToString();
            }

            if(this.importsourceid>0)
            {
                query.StrImportsourceId=this.importsourceid.ToString();
            }

          
            if (!string.IsNullOrWhiteSpace(this.keyWord))
            {
                query.Keywords = this.keyWord;
            }

            query.PageIndex=pageNumber;
            query.PageSize=maxNum;
            query.SortBy=text;

            if(text2.ToLower()=="desc")
            {
            query.SortOrder=EcShop.Core.Enums.SortAction.Desc;
            }
            else
            {
                query.SortOrder=EcShop.Core.Enums.SortAction.Asc;
            }
            DbQueryResult dr = ProductBrowser.GetCurrBrowseProductList(query);

            //this.rptProducts.DataSource = ProductBrowser.GetProducts(null, new int?(this.categoryId), new int?(this.supplierId), this.keyWord, pageNumber, maxNum, out num, text, true, text2, true, new int?(this.brandid), new int?(this.importsourceid));

            this.rptProducts.DataSource = dr.Data;
            this.rptProducts.DataBind();
            this.txtTotalPages.SetWhenIsNotNull(dr.TotalRecords.ToString());

            int userId = HiContext.Current.User.UserId;
            if (userId > 0 && this.rpthistorysearch != null)
            {
                this.rpthistorysearch.DataSource = HistorySearchHelp.GetSearchHistory(userId, ClientType.VShop, 6);
                this.rpthistorysearch.DataBind();
            }

            PageTitle.AddSiteNameTitle("分类搜索页");
        }
    }
}

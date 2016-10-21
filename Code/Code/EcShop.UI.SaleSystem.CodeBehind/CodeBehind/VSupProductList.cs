using EcShop.ControlPanel.Store;
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
using Commodities;
using EcShop.ControlPanel.Commodities;
using EcShop.Entities;
using EcShop.Core.Entities;
namespace EcShop.UI.SaleSystem.CodeBehind
{
    [System.Web.UI.ParseChildren(true)]
    public class VSupProductList : VshopTemplatedWebControl
    {
        private int categoryId;
        private int supplierId;
        private string keyWord;
        private HiImage imgUrl;
        private int TopicId;
        private int brandid;
        private int importsourceid;

        private System.Web.UI.WebControls.Literal litContent;
        private System.Web.UI.WebControls.Literal litSiblingsCategories;
        private VshopTemplatedRepeater rptProducts;
        private VshopTemplatedRepeater rptCategories;
        private System.Web.UI.HtmlControls.HtmlInputHidden txtTotalPages;
        //店铺收藏数
        private System.Web.UI.WebControls.Literal litSupplierFavCount;
        //店铺logo
        private HiImage imgSupplierLogo;


        //店铺名称
        private System.Web.UI.WebControls.Literal litSupplierName;
        //店主名称
        private System.Web.UI.WebControls.Literal litSupplierOwner;
        //店铺地址
        private System.Web.UI.WebControls.Literal litSupplierAddress;

        private System.Web.UI.WebControls.Literal isCollect;
        //开店时间
        private System.Web.UI.WebControls.Literal litSupplierCreateTime;
        protected override void OnInit(System.EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-VSupProductList.html";
            }
            base.OnInit(e);
        }
        protected override void AttachChildControls()
        {
            #region == 店铺信息
            if (!int.TryParse(this.Page.Request.QueryString["supplierId"], out this.supplierId))
            {
                base.GotoResourceNotFound("店铺已不存在");
            }
            //店铺收藏数
            this.litSupplierFavCount = (System.Web.UI.WebControls.Literal)this.FindControl("litSupplierFavCount");
            //店铺logo
            this.imgSupplierLogo = (HiImage)this.FindControl("imgSupplierLogo");

            this.isCollect = (System.Web.UI.WebControls.Literal)this.FindControl("isCollect");

            //店铺名称
            this.litSupplierName = (System.Web.UI.WebControls.Literal)this.FindControl("litSupplierName");
            //店主名称
            this.litSupplierOwner = (System.Web.UI.WebControls.Literal)this.FindControl("litSupplierOwner");
            //店铺地址
            this.litSupplierAddress = (System.Web.UI.WebControls.Literal)this.FindControl("litSupplierAddress");
            //开店时间
            this.litSupplierCreateTime = (System.Web.UI.WebControls.Literal)this.FindControl("litSupplierCreateTime");

            Member member = HiContext.Current.User as Member;
            int userId = 0;
            if (member != null)
            {
                userId = member.UserId;
            }
            AppSupplierInfo info = SupplierHelper.GetAppSupplier(supplierId, userId);

            if (info != null)
            {
                this.litSupplierFavCount.Text = info.CollectCount > 0 ? info.CollectCount.ToString() : "0";
                this.imgSupplierLogo.ImageUrl = info.Logo;
                this.litSupplierName.Text = info.ShopName;
                this.litSupplierOwner.Text = info.ShopOwner;
                //this.isCollect.Text = info.IsCollect.ToString();
                if (info.CreateDate.HasValue)
                {
                    this.litSupplierAddress.Text = RegionHelper.GetFullRegion(info.County, ",").Split(',')[0];
                }
                if (info.County > 0)
                {
                    this.litSupplierCreateTime.Text = info.CreateDate.Value.ToString("yyyy-MM-dd HH:mm:ss");
                }

            }
            else
            {
                base.GotoResourceNotFound("店铺已不存在");
            }
            #endregion
            this.imgUrl = (HiImage)this.FindControl("imgUrl");
            this.litContent = (System.Web.UI.WebControls.Literal)this.FindControl("litContent");
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
            this.rptProducts = (VshopTemplatedRepeater)this.FindControl("rptProducts");
            this.rptCategories = (VshopTemplatedRepeater)this.FindControl("rptCategories");
            this.txtTotalPages = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("txtTotal");
            this.litSiblingsCategories = (System.Web.UI.WebControls.Literal)this.FindControl("litSiblingsCategories");
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
            if (this.categoryId > 0)
            {
                query.StrCategoryId = this.categoryId.ToString();
            }

            if (this.supplierId > 0)
            {
                query.supplierid = this.supplierId;
            }

            if (this.brandid > 0)
            {
                query.StrBrandId = this.brandid.ToString();
            }

            if (this.importsourceid > 0)
            {
                query.StrImportsourceId = this.importsourceid.ToString();
            }


            if (!string.IsNullOrWhiteSpace(this.keyWord))
            {
                query.Keywords = this.keyWord;
            }

            query.PageIndex = pageNumber;
            query.PageSize = maxNum;
            query.SortBy = text;

            if (text2.ToLower() == "desc")
            {
                query.SortOrder = EcShop.Core.Enums.SortAction.Desc;
            }
            else
            {
                query.SortOrder = EcShop.Core.Enums.SortAction.Asc;
            }
            DbQueryResult dr = ProductBrowser.GetCurrBrowseProductList(query);

            //this.rptProducts.DataSource = ProductBrowser.GetProducts(null, new int?(this.categoryId), new int?(this.supplierId), this.keyWord, pageNumber, maxNum, out num, text, true, text2, true, new int?(this.brandid), new int?(this.importsourceid));

            this.rptProducts.DataSource = dr.Data;
            this.rptProducts.DataBind();
            this.txtTotalPages.SetWhenIsNotNull(dr.TotalRecords.ToString());

            PageTitle.AddSiteNameTitle("店内搜索");
        }
    }
}

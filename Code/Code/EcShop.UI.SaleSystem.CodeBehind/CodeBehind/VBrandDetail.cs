using EcShop.Core.Entities;
using EcShop.Core.Enums;
using EcShop.Entities.Commodities;
using EcShop.SaleSystem.Catalog;
using EcShop.UI.Common.Controls;
using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace EcShop.UI.SaleSystem.CodeBehind
{
    [System.Web.UI.ParseChildren(true)]
    public class VBrandDetail : VshopTemplatedWebControl
    {
        private int BrandId;
        private HiImage imgUrl;
        private VshopTemplatedRepeater rptProducts;
        private System.Web.UI.WebControls.Literal litContent;
        private System.Web.UI.WebControls.Literal litBrandDetail;
        private System.Web.UI.HtmlControls.HtmlInputHidden txtTotal;
        protected override void OnInit(System.EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-VBrandDetail.html";
            }
            base.OnInit(e);
        }
        protected override void AttachChildControls()
        {
            if (!int.TryParse(this.Page.Request.QueryString["BrandId"], out this.BrandId))
            {
                base.GotoResourceNotFound("");
            }
            this.imgUrl = (HiImage)this.FindControl("imgUrl");
            this.rptProducts = (VshopTemplatedRepeater)this.FindControl("rptProducts");
            this.litBrandDetail = (System.Web.UI.WebControls.Literal)this.FindControl("litBrandDetail");
            this.txtTotal = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("txtTotal");
            this.litContent = (System.Web.UI.WebControls.Literal)this.FindControl("litContent");
            BrandCategoryInfo brandCategory = CategoryBrowser.GetBrandCategory(this.BrandId);
            this.litBrandDetail.SetWhenIsNotNull(brandCategory.Description);
            this.imgUrl.ImageUrl = brandCategory.Logo;
            int pageNumber;
            this.litContent.Text = brandCategory.BrandName;
            if (!int.TryParse(this.Page.Request.QueryString["page"], out pageNumber))
            {
                pageNumber = 1;
            }
            int maxNum;
            if (!int.TryParse(this.Page.Request.QueryString["size"], out maxNum))
            {
                maxNum = 20;
            }
            int num;
            ProductBrowseQuery query = new ProductBrowseQuery();
            query.SortOrder = SortAction.Desc;
            query.SortBy = "DisplaySequence";
            query.PageIndex = 1;
            query.PageSize = 12;
            //this.rptProducts.DataSource = ProductBrowser.GetBrandProducts(new int?(this.BrandId), pageNumber, maxNum, out num);
            DbQueryResult dataProducts = ProductBrowser.GetBrandProducts(new int?(this.BrandId), query);
            this.rptProducts.DataSource = dataProducts.Data;
            this.rptProducts.DataBind();
            this.txtTotal.SetWhenIsNotNull(dataProducts.TotalRecords.ToString());
            PageTitle.AddSiteNameTitle("品牌详情");
        }
    }
}
